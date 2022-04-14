using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using OpenCVForUnity.CoreModule;

public enum ShadowType
{
    sword,
    bow,
    shield,
    rope,
    unknown,
    none
}

public class Shadow
{
    //public Point[] contourPoints;
    public Vector2[] contourPoints;
    public ShadowType label;
    public int imgWidth;
    public int imgHeight;
    public float largestSpannedDist;
    Label labelObj;

    public Shadow(ShadowType label_, int width, int height)
    {
        label = label_;
        imgWidth = width;
        imgHeight = height;
    }

   // public Shadow(ShadowType label_, int width, int height, Point[] contour)
    public Shadow(ShadowType label_, int width, int height, Vector2[] contour)
    {
        label = label_;
        imgWidth = width;
        imgHeight = height;
        contourPoints = contour;
        largestSpannedDist = 0;
        int x1 = width, x2 = 0, y1 = height, y2 = 0;
        for(int i=0; i < contour.Length; ++i)
        {
            x1 = (int)Mathf.Min(x1, contour[i].x);
            y1 = (int)Mathf.Min(y1, contour[i].y);
            x2 = (int)Mathf.Max(x2, contour[i].x);
            y2 = (int)Mathf.Max(y2, contour[i].y);
        }
        largestSpannedDist = Vector2.Distance(new Vector2(x1,y1), new Vector2(x2, y2));
    }

    public void Relabel(GameObject[] labelPrefabs)
    {
        if (contourPoints.Length == 2) label = ShadowType.sword;
        else if (contourPoints.Length == 3)
        {
            int yPos_0 = (int)contourPoints[0].y;
            int xPos_0 = (int)contourPoints[0].x;
            int yPos_1 = (int)contourPoints[1].y;
            int xPos_1 = (int)contourPoints[1].x;
            int yPos_2 = (int)contourPoints[2].y;
            int xPos_2 = (int)contourPoints[2].x;

            float side0 = Mathf.Sqrt(Mathf.Pow(xPos_1 - xPos_0, 2.0f) + Mathf.Pow(yPos_1 - yPos_0, 2.0f));
            float side1 = Mathf.Sqrt(Mathf.Pow(xPos_2 - xPos_1, 2.0f) + Mathf.Pow(yPos_2 - yPos_1, 2.0f));
            float side2 = Mathf.Sqrt(Mathf.Pow(xPos_0 - xPos_2, 2.0f) + Mathf.Pow(yPos_0 - yPos_2, 2.0f));

            float largest = side0, smallest = side0;
            if (side1 > largest) largest = side1;
            if (side2 > largest) largest = side2;
            if (side1 < smallest) smallest = side1;
            if (side2 < smallest) smallest = side2;

            if (smallest / largest < 0.25) label = ShadowType.sword;
            else if(Vector3.Angle(contourPoints[1]-contourPoints[0], contourPoints[2] - contourPoints[0]) < 15 || Vector3.Angle(contourPoints[0] - contourPoints[1], contourPoints[2] - contourPoints[1]) < 15 || Vector3.Angle(contourPoints[1] - contourPoints[2], contourPoints[0] - contourPoints[2]) < 15) label = ShadowType.sword;
            else label = ShadowType.bow;
        }
        else if (contourPoints.Length == 4)
        {
            int yPos_0 = (int)contourPoints[0].y;
            int xPos_0 = (int)contourPoints[0].x;
            int yPos_1 = (int)contourPoints[1].y;
            int xPos_1 = (int)contourPoints[1].x;
            int yPos_2 = (int)contourPoints[2].y;
            int xPos_2 = (int)contourPoints[2].x;
            int yPos_3 = (int)contourPoints[3].y;
            int xPos_3 = (int)contourPoints[3].x;

            float side0 = Mathf.Sqrt(Mathf.Pow(xPos_1 - xPos_0, 2.0f) + Mathf.Pow(yPos_1 - yPos_0, 2.0f));
            float side1 = Mathf.Sqrt(Mathf.Pow(xPos_2 - xPos_1, 2.0f) + Mathf.Pow(yPos_2 - yPos_1, 2.0f));
            float side2 = Mathf.Sqrt(Mathf.Pow(xPos_3 - xPos_2, 2.0f) + Mathf.Pow(yPos_3 - yPos_2, 2.0f));
            float side3 = Mathf.Sqrt(Mathf.Pow(xPos_0 - xPos_3, 2.0f) + Mathf.Pow(yPos_0 - yPos_3, 2.0f));
            float cross1 = (side0 + side2) / 2;
            float cross2 = (side1 + side3) / 2;

            if (side0 / side2 > Mathf.Max(1.5f, 1.5f * 15 / cross1) || side2 / side0 > Mathf.Max(1.5f, 1.5f * 15 / cross1) || side1 / side3 > Mathf.Max(1.5f, 1.5f * 15 / cross2) || side3 / side1 > Mathf.Max(1.5f, 1.5f * 15 / cross2)) label = ShadowType.unknown;
            else
            {
                float ratio = cross1 / cross2;
                if (ratio >= 0.35 && ratio < 2) label = ShadowType.shield;
                else if (ratio < 0.35 || ratio >= 2) label = ShadowType.sword;
                else { Debug.Log("bad ratio: " + ratio); label = ShadowType.unknown; }
            }
        }
        else if (contourPoints.Length == 5)
        {
            int yPos_0 = (int)contourPoints[0].y;
            int xPos_0 = (int)contourPoints[0].x;
            int yPos_1 = (int)contourPoints[1].y;
            int xPos_1 = (int)contourPoints[1].x;
            int yPos_2 = (int)contourPoints[2].y;
            int xPos_2 = (int)contourPoints[2].x;
            int yPos_3 = (int)contourPoints[3].y;
            int xPos_3 = (int)contourPoints[3].x;
            int yPos_4 = (int)contourPoints[4].y;
            int xPos_4 = (int)contourPoints[4].x;

            float side0 = Mathf.Sqrt(Mathf.Pow(xPos_0 - xPos_1, 2.0f) + Mathf.Pow(yPos_0 - yPos_1, 2.0f));
            float side1 = Mathf.Sqrt(Mathf.Pow(xPos_2 - xPos_1, 2.0f) + Mathf.Pow(yPos_2 - yPos_1, 2.0f));
            float side2 = Mathf.Sqrt(Mathf.Pow(xPos_3 - xPos_2, 2.0f) + Mathf.Pow(yPos_3 - yPos_2, 2.0f));
            float side3 = Mathf.Sqrt(Mathf.Pow(xPos_3 - xPos_4, 2.0f) + Mathf.Pow(yPos_3 - yPos_4, 2.0f));
            float side4 = Mathf.Sqrt(Mathf.Pow(xPos_4 - xPos_0, 2.0f) + Mathf.Pow(yPos_4 - yPos_0, 2.0f));


        }
        else
        {
            //first check if its actually round
            int dirChange = 0; float slope = 0, prevSlope = 0;
            Vector2 dot1, dot2; float dot;
            int next = 0;
            int curr = contourPoints.Length - 1;
            int y0 = (int)contourPoints[curr].y;
            int x0 = (int)contourPoints[curr].x;
            int y1 = (int)contourPoints[next].y;
            int x1 = (int)contourPoints[next].x;
            dot2 = new Vector2(x1 - x0, y1 - y0);
            List<Vector2> rectCheck = new List<Vector2>();
            rectCheck.Add(dot2.normalized);
            for (int i = 0; i < contourPoints.Length; i++)
            {
                next = (i + 1) % contourPoints.Length;
                curr = i;
                y0 = (int)contourPoints[curr].y;
                x0 = (int)contourPoints[curr].x;
                y1 = (int)contourPoints[next].y;
                x1 = (int)contourPoints[next].x;
                slope = (y1 - y0) / (x1 - x0 + 0.000000001f);
                dot1 = new Vector2(x1 - x0, y1 - y0);
                dot = Vector2.Dot(dot1, dot2);
                if (contourPoints.Length == 8) rectCheck.Add(dot1.normalized);
                else if (dot < 0.5)
                {
                    //Debug.Log("BAD DOT");
                    label = ShadowType.unknown;
                    CreateInGameLabel(labelPrefabs);
                    return;
                }
                if (Mathf.Sign(slope) != Mathf.Sign(prevSlope)) dirChange++;
                prevSlope = slope;
                dot2 = dot1;
            }
            if (!(dirChange == 3 || dirChange == 4)) { label = ShadowType.unknown; /*Debug.Log("DIR CHANGE: " + dirChange)*/; }
            else
            {
                //now circle vs oval
                int yPos_0 = (int)contourPoints[0].y;
                int xPos_0 = (int)contourPoints[0].x;
                int yPos_1 = (int)contourPoints[1].y;
                int xPos_1 = (int)contourPoints[1].x;
                int yPos_2 = (int)contourPoints[2].y;
                int xPos_2 = (int)contourPoints[2].x;
                int yPos_3 = (int)contourPoints[3].y;
                int xPos_3 = (int)contourPoints[3].x;

                float cross1 = Mathf.Sqrt(Mathf.Pow(xPos_2 - xPos_0, 2.0f) + Mathf.Pow(yPos_2 - yPos_0, 2.0f));
                float cross2 = Mathf.Sqrt(Mathf.Pow(xPos_3 - xPos_1, 2.0f) + Mathf.Pow(yPos_3 - yPos_1, 2.0f));
                //Debug.Log("cross1: " + cross1 + " cross2: " + cross2 );
                float ratio = cross1 / cross2;
                if (ratio > 0.60 && ratio < 2) label = ShadowType.rope;
                else label = ShadowType.unknown;
            }
        }
        Debug.Log("-----------------------------------------------label for this shadow: " + label);

        CreateInGameLabel(labelPrefabs);
    }

    private void CreateInGameLabel(GameObject[] labelPrefabs)
    {
        GameObject labelPrefab;
        if (label == ShadowType.rope) labelPrefab = labelPrefabs[0];
        else if (label == ShadowType.bow) labelPrefab = labelPrefabs[1];
        else if (label == ShadowType.sword) labelPrefab = labelPrefabs[2];
        else if (label == ShadowType.shield) labelPrefab = labelPrefabs[3];
        else labelPrefab = labelPrefabs[4];

        if(label != ShadowType.unknown && label != ShadowType.none)
        {
            int xCenter = 0, yCenter = 0, minX = 10000, maxX = 0, minY = 0, maxY = 10000;
            for (int i = 0; i < contourPoints.Length; ++i)
            {
                xCenter += (int)contourPoints[i].x;
                yCenter += (int)contourPoints[i].y;
                if (contourPoints[i].x < minX) minX = (int)contourPoints[i].x;
                if (contourPoints[i].y < minY) minY = (int)contourPoints[i].y;
                if (contourPoints[i].x > maxX) maxX = (int)contourPoints[i].x;
                if (contourPoints[i].y > maxY) maxY = (int)contourPoints[i].y;
            }
            xCenter /= contourPoints.Length;
            yCenter /= contourPoints.Length;

            GameObject labelList = GameObject.FindWithTag("labeler");
            labelObj = labelList.AddComponent<Label>();
            labelObj.SetLabelPrefab(labelPrefab);
            labelObj.SetArea(minX, maxX, minY, maxY);
            labelObj.SetTransforms(xCenter, yCenter);
            labelObj.SetInScene();
        }
    }
    private void SetGameshadow()
    {
        Camera cam = GameObject.FindWithTag("shadow_cam").GetComponent<Camera>();
        Vector3 test = cam.ScreenToWorldPoint(new Vector3(contourPoints[0].x, contourPoints[0].y, 0.01f));

    }

}
