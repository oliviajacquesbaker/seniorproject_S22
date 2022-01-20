using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class IdentifyShadows : MonoBehaviour
{
    [SerializeField]
    Camera cam;
    [SerializeField]
    int resizeAmount = 3;
    [SerializeField]
    GameObject[] labels = new GameObject[5];

    List<Shadow> shadows;
    Pixel[] BW;
    ShadowType currAvailShadow;
    Vector3 pastPos;

    struct Pixel
    {
        public bool discovered;
        public float color;

        public Pixel(float col, bool disc)
        {
            discovered = disc;
            color = col;
        }
    }

    void Start()
    {
        pastPos = this.gameObject.transform.position;
        DetectShadows();
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

    public Color[] DetectShadows()
    {
        shadows = new List<Shadow>();
        RenderTexture inbetween = new RenderTexture(3 * Screen.width / 4, Screen.height, 24);
        Texture2D image = new Texture2D(3 * Screen.width / 4, Screen.height, TextureFormat.RGB24, false);
        Texture2D imageResized;

        //grab what the ortho cam sees and put it on a texture we can read
        if (cam.targetTexture != null) cam.targetTexture.Release();
        cam.targetTexture = inbetween;
        cam.Render();
        RenderTexture.active = inbetween;
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = null;

        imageResized = ResizeTexture(image, 3 * Screen.width / 4 / resizeAmount, Screen.height / resizeAmount);

        //all this is just to check if it's proper B&W
        Color[] pixels = ApplyGreyscaleFilter(imageResized);
        CreateImage(pixels, "BW_ShadowBlobs");
        //end of checking code

        for (int i = 0; i < pixels.Length; i++)
        {
            if (BW[i].color == 0 && !BW[i].discovered)
            {
                Shadow toAdd = BFS_ShadowIdentification(i, imageResized.width, imageResized.height);
                if (toAdd.pixels.Count > 10) shadows.Add(toAdd);
            }
        }

        //all this is just to check if everything was properlly identified
        Color[] testpixels = ApplyShapeColorFilter(pixels);
        CreateImage(pixels, "Separated_ShadowBlobs");
        //end of checking code

        return testpixels;
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

    Color[] ApplyGreyscaleFilter(Texture2D image)
    {
        Color[] pixels = image.GetPixels();
        BW = new Pixel[pixels.Length];
        int blackCount = 0;
        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i].r + pixels[i].g + pixels[i].b > 1)
            {
                pixels[i].r = 255;
                pixels[i].g = 255;
                pixels[i].b = 255;
            }
            else
            {
                pixels[i].r = 0;
                pixels[i].g = 0;
                pixels[i].b = 0;
                blackCount++;
            }
            BW[i] = new Pixel(pixels[i].r, false);
        }
        return pixels;
    }

    Color[] ApplyShapeColorFilter(Color[] pixels)
    {
        //the actual coloring
        for (int i = 0; i < shadows.Count; i++)
        {
            for (int j = 0; j < shadows[i].pixels.Count; j++)
            {
                if (shadows[i].label == ShadowType.square) pixels[shadows[i].pixels[j]].g = 10;
                else if (shadows[i].label == ShadowType.rect) pixels[shadows[i].pixels[j]].b = 10;
                else if (shadows[i].label == ShadowType.circle) pixels[shadows[i].pixels[j]].r = 100;
                else if (shadows[i].label == ShadowType.oval) { pixels[shadows[i].pixels[j]].g = 10; pixels[shadows[i].pixels[j]].r = 10; }
                else if (shadows[i].label == ShadowType.line) { pixels[shadows[i].pixels[j]].g = 10; pixels[shadows[i].pixels[j]].b = 10; }
                else if (shadows[i].label == ShadowType.unknown || shadows[i].label == ShadowType.none) { pixels[shadows[i].pixels[j]].r = 10; pixels[shadows[i].pixels[j]].b = 10; }
            }
        }

        //color the verts in separate so you can see how the label was calculated
        for (int i = 0; i < shadows.Count; i++)
        {
            for (int j = 0; j < shadows[i].cornerVerts.Count; j++)
            {
                pixels[shadows[i].cornerVerts[j]].r = 0;
                pixels[shadows[i].cornerVerts[j]].g = 0;
                pixels[shadows[i].cornerVerts[j]].b = 0;
            }
        }

        return pixels;
    }

    public List<int> SortVerts(bool[] edges, int startingVert, int width)
    {
        int start = startingVert; int current = start;
        List<int> sortedVerts = new List<int>();
        bool[] added = new bool[edges.Length];
        for (int i = 0; i < edges.Length; i++) { added[i] = false; }
        sortedVerts.Add(start); added[start] = true;
        int iterated = 0;
        int x, y, t, b, l, r, tr, tl, br, bl;
        while (iterated < edges.Length)
        {
            y = Mathf.FloorToInt(current / width);
            x = current % width;
            l = (x - 1) + y * width;
            t = (y + 1) * width + x;
            r = (x + 1) + y * width;
            b = (y - 1) * width + x;
            br = (y - 1) * width + x + 1;
            bl = (y - 1) * width + x - 1;
            tr = (y + 1) * width + x + 1;
            tl = (y + 1) * width + x - 1;

            if (r >= 0 && r < edges.Length && edges[r] && !added[r]) { sortedVerts.Add(r); added[current] = true; current = r; }
            else if (tr >= 0 && tr < edges.Length && edges[tr] && !added[tr]) { sortedVerts.Add(tr); added[current] = true; current = tr; }
            else if (t >= 0 && t < edges.Length && edges[t] && !added[t]) { sortedVerts.Add(t); added[current] = true; current = t; }
            else if (tl >= 0 && tl < edges.Length && edges[tl] && !added[tl]) { sortedVerts.Add(tl); added[current] = true; current = tl; }
            else if (l >= 0 && l < edges.Length && edges[l] && !added[l]) { sortedVerts.Add(l); added[current] = true; current = l; }
            else if (bl >= 0 && bl < edges.Length && edges[bl] && !added[bl]) { sortedVerts.Add(bl); added[current] = true; current = bl; }
            else if (b >= 0 && b < edges.Length && edges[b] && !added[b]) { sortedVerts.Add(b); added[current] = true; current = b; }
            else if (br >= 0 && br < edges.Length && edges[br] && !added[br]) { sortedVerts.Add(br); added[current] = true; current = br; }
            else break;

            iterated++;
        }
        return sortedVerts;
    }

    public List<int> FilterVerts(List<int> edges, bool[] keyedges, int width)
    {
        List<int> sortedKeyVerts = new List<int>();
        for (int i = 0; i < edges.Count; i++)
        {
            if (keyedges[edges[i]]) sortedKeyVerts.Add(edges[i]);
        }
        return sortedKeyVerts;
    }

    Shadow BFS_ShadowIdentification(int startInd, int width, int height)
    {
        Shadow thisShadow = new Shadow(ShadowType.unknown, width);
        List<int> queue = new List<int>();
        BW[startInd].discovered = true;
        bool[] edges = new bool[BW.Length];
        bool[] keyedges = new bool[BW.Length];
        queue.Add(startInd);

        int currInd = 0; int xPos = 0; int yPos = 0;
        int left = 0; int top = 0; int right = 0; int bottom = 0;

        while (queue.Count > 0)
        {
            currInd = queue[0];
            thisShadow.pixels.Add(currInd);
            queue.RemoveAt(0);

            yPos = Mathf.FloorToInt(currInd / width);
            xPos = currInd % width;
            left = (xPos - 1) + yPos * width;
            top = (yPos + 1) * width + xPos;
            right = (xPos + 1) + yPos * width;
            bottom = (yPos - 1) * width + xPos;

            //BFS stuff
            if (left >= 0 && left < BW.Length && !BW[left].discovered)
            {
                if (BW[left].color == 0) { queue.Add(left); }
                BW[left].discovered = true;
            }
            if (top >= 0 && top < BW.Length && !BW[top].discovered)
            {
                if (BW[top].color == 0) { queue.Add(top); }
                BW[top].discovered = true;
            }
            if (right >= 0 && right < BW.Length && !BW[right].discovered)
            {
                if (BW[right].color == 0) { queue.Add(right); }
                BW[right].discovered = true;
            }
            if (bottom >= 0 && bottom < BW.Length && !BW[bottom].discovered)
            {
                if (BW[bottom].color == 0) { queue.Add(bottom); }
                BW[bottom].discovered = true;
            }

            int blackNeighbors = 0;
            if (left >= 0 && left < BW.Length && BW[left].color == 0) blackNeighbors++;
            if (right >= 0 && right < BW.Length && BW[right].color == 0) blackNeighbors++;
            if (top >= 0 && top < BW.Length && BW[top].color == 0) blackNeighbors++;
            if (bottom >= 0 && bottom < BW.Length && BW[bottom].color == 0) blackNeighbors++;
            if (blackNeighbors <= 3) edges[currInd] = true;
            else edges[currInd] = false;
            if (blackNeighbors <= 2) keyedges[currInd] = true;
            else keyedges[currInd] = false;
        }

        thisShadow.cornerVerts = FilterVerts(SortVerts(edges, thisShadow.pixels[0], width), keyedges, width);
        thisShadow.condenseVerts(width);
        thisShadow.ReduceVerts_2(width, 2, 2);
        thisShadow.ReduceVerts(width, 4);
        //thisShadow.lookAtAngles(width);
        thisShadow.Relabel(labels);

        return thisShadow;
    }

    void CreateImage(Color[] pixels, string fileName)
    {
        Texture2D image = new Texture2D(3 * Screen.width / 4 / resizeAmount, Screen.height / resizeAmount, TextureFormat.RGB24, false);
        image.SetPixels(pixels);
        image.Apply();
        byte[] bytes = image.EncodeToPNG();
        string dirPath = Application.dataPath + "/./SaveImages/";
        File.WriteAllBytes(dirPath + fileName + ".png", bytes);
    }
}
