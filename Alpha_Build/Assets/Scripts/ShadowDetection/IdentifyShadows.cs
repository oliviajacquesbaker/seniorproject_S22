using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using System;

public class IdentifyShadows : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    int resizeAmount = 3;
    [SerializeField]
    GameObject[] labels = new GameObject[5];

    List<Shadow> shadows;
    //Pixel[] BW;
    ShadowType currAvailShadow;
    Vector3 pastPos;

    void Start()
    {
        pastPos = this.gameObject.transform.position;
        DetectShadows();
        Debug.Log("STARTED");
    }

    void Update()
    {
        float dist = Vector3.Distance(this.gameObject.transform.position, pastPos);
        //Debug.Log(dist);
        if (dist > 4)
        {
            Debug.Log("-----------------------------------------------------------------------------------------");
            pastPos = this.gameObject.transform.position;
            RemoveLabels();
            DetectShadows();
        }
    }

    public void RemoveLabels()
    {
        GameObject labelList = GameObject.FindWithTag("labeler");
        foreach (Label lbl in labelList.GetComponents<Label>())
        {
            lbl.RemoveFromScene();
            Destroy(lbl);
        }
    }

    public void DetectShadows()
    {
        shadows = new List<Shadow>();
        RenderTexture inbetween = new RenderTexture(3 * Screen.width / 4, Screen.height, 24);
        Texture2D image = new Texture2D(3 * Screen.width / 4, Screen.height, TextureFormat.RGB24, false);
        Texture2D imageCheck = new Texture2D(3 * Screen.width / 4, Screen.height, TextureFormat.RGBA32, false);
        //Texture2D imageResized;

        //grab what the ortho cam sees and put it on a texture we can read
        if (cam.targetTexture != null) cam.targetTexture.Release();
        cam.targetTexture = inbetween;
        cam.Render();
        RenderTexture.active = inbetween;
        image.ReadPixels(new UnityEngine.Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = null;

        int[] pixels = ApplyGreyscaleFilter(image);

        //CreateImage(image.GetPixels(), "BW_ShadowBlobs");

        int maxOutContours = 20;
        int maxVertsPerContour = 20;
        CvVertex[] outContours = new CvVertex[400];
        int[] numVertsPerContour = new int[20];
        int numContours = 0;

        try
        {
            unsafe
            {
                fixed (int* _pixels = pixels, _numVertsPerContour = numVertsPerContour)
                {
                    fixed (CvVertex* _outContours = outContours)
                    {
                        //Debug.Log("stand in");
                        OpenCVInterop.DetectContours(_outContours, _numVertsPerContour, ref numContours, _pixels, image.height, image.width, maxOutContours, maxVertsPerContour);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log("EXCEPTION: ");
            Debug.LogException(ex);
        }
        Debug.Log("FOUND " + numContours + " SHADOWS");
        int startInd = 0;
        for (int i = 0; i < numContours; ++i)
        {
            if(numVertsPerContour[i] > 0)
            {
                Vector2[] pointsOrdered = ReorderContourPoints(ref outContours, startInd, numVertsPerContour[i]);
                Vector2[] points = RemoveOutliers(ref pointsOrdered);
                Shadow thisShadow = new Shadow(ShadowType.unknown, image.width, image.height, points);
                thisShadow.Relabel(labels);
                shadows.Add(thisShadow);
                startInd += numVertsPerContour[i];
            }
        }


        Color[] pixelsOut = image.GetPixels();
        for (int i = 0; i < numContours; ++i)
        {
            CvVertex[] pts = outContours;
            for (int j = 0; j < pts.Length; ++j)
            {
                CvVertex pt = pts[j];
                int ind = (int)pt.X + ((int)pt.Y) * imageCheck.width;
                if (ind < pixelsOut.Length)
                {
                    pixelsOut[ind].r = 1f;
                    pixelsOut[ind].g = 0.5f;
                    pixelsOut[ind].b = 0f;
                }
            }
        }

        for (int i = 0; i < shadows.Count; ++i)
        {
            for (int j = 0; j < shadows[i].contourPoints.Length; ++j)
            {
                Vector2 pt = shadows[i].contourPoints[j];
                int ind = (int)pt.x + ((int)pt.y) * imageCheck.width;
                if (ind < pixelsOut.Length)
                {
                    pixelsOut[ind].r = 0f;
                    pixelsOut[ind].g = 1f;
                    pixelsOut[ind].b = 1f;
                }
            }
        }
        CreateImage(pixelsOut, "ContourPoints2");
    }

    Vector2[] ReorderContourPoints(ref CvVertex[] contours, int startingInd, int size)
    {
        Vector2[] output = new Vector2[size];
        int x, y, xCenter = 0, yCenter = 0;
        for (int i = startingInd; i < startingInd + size; ++i)
        {
            x = (int)contours[i].X;
            y = (int)contours[i].Y;
            xCenter += x;
            yCenter += y;
            output[i - startingInd] = new Vector2(x, y);
        }
        xCenter /= size;
        yCenter /= size;

        output = output.OrderByDescending(point => Mathf.Atan2((float)point.x - xCenter, (float)point.y - yCenter)).ToArray();
        return output;
    }

    Vector2[] RemoveOutliers(ref Vector2[] points)
    {
        if (points.Length != 5) return points;
        int outInd = -1;
        int prev, next;
        double area, minArea;
        minArea = GetPolygonalArea(ref points) / 20;
        for(int i=0; i<points.Length; ++i)
        {
            prev = (i - 1) % points.Length;
            if (prev < 0) prev += points.Length;
            next = (i + 1) % points.Length;
            area = (points[prev].x*points[i].y + points[i].x*points[next].y + points[next].x*points[prev].y - points[prev].y*points[i].x - points[i].y*points[next].x - points[next].y*points[prev].x) / 2.0;
            if (area < minArea)
            {
                if(points[prev].x+points[i].x+points[next].x < points[prev].y + points[i].y + points[next].y)
                {
                    if (Mathf.Abs(points[prev].x - points[i].x) < Mathf.Abs(points[next].x - points[i].x)) outInd = prev;
                    else outInd = next;
                }
                else
                {
                    if (Mathf.Abs(points[prev].y - points[i].y) < Mathf.Abs(points[next].y - points[i].y)) outInd = prev;
                    else outInd = next;
                }
            }
        }

        if(outInd == -1) return points;
        else
        {
            Vector2[] updatedPoints = new Vector2[points.Length - 1];
            int j = 0;
            for (int i = 0; i < points.Length; ++i)
            {
                if (i != outInd) { updatedPoints[j] = points[i]; ++j; }

            }
            return updatedPoints;
        }
    }

    float GetPolygonalArea(ref Vector2[] points)
    {
        float area = 0, add;
        int next;
        for (int i = 0; i < points.Length; ++i)
        {
            next = (i + 1) % points.Length;
            add = points[i].x * points[next].y - points[i].y * points[next].x;
            area += add;
        }
        return area;
    }

    Texture2D ResizeTexture(Texture2D input, int finalW, int finalH)
    {
        Texture2D toReturn = new Texture2D(finalW, finalH, TextureFormat.RGB24, false);
        for (int i = 0; i < finalW * finalH; i++)
        {
            int y = Mathf.FloorToInt(i / finalW);
            int x = i % finalW;
            toReturn.SetPixel(x, y, input.GetPixelBilinear(x * 1.0f / finalW * 1.0f, y * 1.0f / finalH * 1.0f));
        }
        toReturn.Apply();
        return toReturn;
    }

    int[] ApplyGreyscaleFilter(Texture2D image)
    {
        Color[] pixels = image.GetPixels();
        int[] toReturn = new int[pixels.Length];
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r + pixels[i].g + pixels[i].b > 1.25)
            {
                toReturn[i] = 0;
            }
            else
            {
                toReturn[i] = 255;
            }
        }
        return toReturn;
    }


    void CreateImage(Color[] pixels, string fileName)
    {
        //Texture2D image = new Texture2D(3 * Screen.width / 4 / resizeAmount, Screen.height / resizeAmount, TextureFormat.RGB24, false);
        Texture2D image = new Texture2D(3 * Screen.width / 4, Screen.height, TextureFormat.RGB24, false);
        image.SetPixels(pixels);
        image.Apply();
        byte[] bytes = image.EncodeToPNG();
        string dirPath = Application.dataPath + "/./SaveImages/";
        File.WriteAllBytes(dirPath + fileName + ".png", bytes);
    }
}

internal static class OpenCVInterop
{
    [DllImport("openCVPlugin", EntryPoint = "DetectContours")]
    internal unsafe static extern void DetectContours(CvVertex* outContours, int* numVertsPerContour, ref int numContours, int* pixelsIn, int imageHeight, int imageWidth, int maxOutContours, int maxVertsPerContour);
}

// Size =  byte size (2 ints = 4 bytes * 2 = 8 bytes)
[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct CvVertex
{
    public int X, Y;
}