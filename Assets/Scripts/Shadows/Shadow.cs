using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShadowType
{
    circle,
    oval,
    rect,
    square,
    line,
    unknown,
    none
}

public class Shadow
{
    public List<int> pixels;
    public List<int> cornerVerts;
    public int vertices;
    public ShadowType label;
    public int imgWidth;
    Label labelObj;

    public Shadow(ShadowType label_, int width)
    {
        label = label_;
        imgWidth = width;
        vertices = -1;
        pixels = new List<int>();
        cornerVerts = new List<int>();
    }

    public void Relabel(GameObject[] labelPrefabs)
    {
        if (cornerVerts.Count == 2) label = ShadowType.line;
        else if (cornerVerts.Count == 4)
        {
            int yPos_0 = Mathf.FloorToInt(cornerVerts[0] / imgWidth);
            int xPos_0 = cornerVerts[0] % imgWidth;
            int yPos_1 = Mathf.FloorToInt(cornerVerts[1] / imgWidth);
            int xPos_1 = cornerVerts[1] % imgWidth;
            int yPos_2 = Mathf.FloorToInt(cornerVerts[2] / imgWidth);
            int xPos_2 = cornerVerts[2] % imgWidth;
            int yPos_3 = Mathf.FloorToInt(cornerVerts[3] / imgWidth);
            int xPos_3 = cornerVerts[3] % imgWidth;

            float side0 = Mathf.Sqrt(Mathf.Pow(xPos_1 - xPos_0, 2.0f) + Mathf.Pow(yPos_1 - yPos_0, 2.0f));
            float side1 = Mathf.Sqrt(Mathf.Pow(xPos_2 - xPos_1, 2.0f) + Mathf.Pow(yPos_2 - yPos_1, 2.0f));
            float side2 = Mathf.Sqrt(Mathf.Pow(xPos_3 - xPos_2, 2.0f) + Mathf.Pow(yPos_3 - yPos_2, 2.0f));
            float side3 = Mathf.Sqrt(Mathf.Pow(xPos_0 - xPos_3, 2.0f) + Mathf.Pow(yPos_0 - yPos_3, 2.0f));
            float cross1 = (side0 + side2) / 2;
            float cross2 = (side1 + side3) / 2;

            if (side0 / side2 > Mathf.Max(1.5f, 1.5f * 10 / cross1) || side2 / side0 > Mathf.Max(1.5f, 1.5f * 10 / cross1) || side1 / side3 > Mathf.Max(1.5f, 1.5f * 10 / cross2) || side3 / side1 > Mathf.Max(1.5f, 1.5f * 10 / cross2)) label = ShadowType.unknown;
            else
            {
                float ratio = cross1 / cross2;
                if (ratio > 0.8 && ratio < 1.2) label = ShadowType.square;
                else label = ShadowType.rect;
            }

        }
        else if (cornerVerts.Count == 6)
        {
            //check for rects that are a bit concave/convex. adjust the thresholds allowed here if it turns out to be too allowing later
            int yPos_0 = Mathf.FloorToInt(cornerVerts[0] / imgWidth);
            int xPos_0 = cornerVerts[0] % imgWidth;
            int yPos_1 = Mathf.FloorToInt(cornerVerts[1] / imgWidth);
            int xPos_1 = cornerVerts[1] % imgWidth;
            int yPos_2 = Mathf.FloorToInt(cornerVerts[2] / imgWidth);
            int xPos_2 = cornerVerts[2] % imgWidth;
            int yPos_3 = Mathf.FloorToInt(cornerVerts[3] / imgWidth);
            int xPos_3 = cornerVerts[3] % imgWidth;
            int yPos_4 = Mathf.FloorToInt(cornerVerts[4] / imgWidth);
            int xPos_4 = cornerVerts[4] % imgWidth;
            int yPos_5 = Mathf.FloorToInt(cornerVerts[5] / imgWidth);
            int xPos_5 = cornerVerts[5] % imgWidth;

            float side0 = Mathf.Sqrt(Mathf.Pow(xPos_1 - xPos_0, 2.0f) + Mathf.Pow(yPos_1 - yPos_0, 2.0f));
            float side1 = Mathf.Sqrt(Mathf.Pow(xPos_2 - xPos_5, 2.0f) + Mathf.Pow(yPos_2 - yPos_5, 2.0f));
            float side2 = Mathf.Sqrt(Mathf.Pow(xPos_3 - xPos_4, 2.0f) + Mathf.Pow(yPos_3 - yPos_4, 2.0f));
            float ratio1 = side0 / side1;
            float ratio2 = side1 / side2;
            float ratio3 = side2 / side0;
            Vector2 cross1 = new Vector2(xPos_1 - xPos_0, yPos_1 - yPos_0);
            Vector2 cross2 = new Vector2(xPos_2 - xPos_5, yPos_2 - yPos_5);
            Vector2 cross3 = new Vector2(xPos_3 - xPos_4, yPos_3 - yPos_4);
            float dir1, dir2, dir3;
            dir1 = Vector2.Dot(cross1, cross2);
            dir2 = Vector2.Dot(cross2, cross3);
            dir3 = Vector2.Dot(cross3, cross1);
            if (dir1 > 0.8 && dir2 > 0.8 && dir3 > 0.8)
            {
                float compareLow = Mathf.Min(0.8f, 0.8f / (10 / ((side0 + side1 + side2) / 3)));
                float compareHigh = Mathf.Max(1.2f, 1.2f * 10 / ((side0 + side1 + side2) / 3));
                if (ratio1 > compareLow && ratio1 < compareHigh && ratio2 > compareLow && ratio2 < compareHigh && ratio3 > compareLow && ratio3 < compareHigh) label = ShadowType.rect;
                else label = ShadowType.unknown;
            }
            else label = ShadowType.unknown;

        }
        else if (cornerVerts.Count >= 8)
        {
            //first check if its actually round
            int dirChange = 0; float slope = 0, prevSlope = 0;
            Vector2 dot1, dot2; float dot;
            int next = 0;
            int curr = cornerVerts.Count - 1;
            int y0 = Mathf.FloorToInt(cornerVerts[curr] / imgWidth);
            int x0 = cornerVerts[curr] % imgWidth;
            int y1 = Mathf.FloorToInt(cornerVerts[next] / imgWidth);
            int x1 = cornerVerts[next] % imgWidth;
            dot2 = new Vector2(x1 - x0, y1 - y0);
            List<Vector2> rectCheck = new List<Vector2>();
            rectCheck.Add(dot2.normalized);
            for (int i = 0; i < cornerVerts.Count; i++)
            {
                next = (i + 1) % cornerVerts.Count;
                curr = i;
                y0 = Mathf.FloorToInt(cornerVerts[curr] / imgWidth);
                x0 = cornerVerts[curr] % imgWidth;
                y1 = Mathf.FloorToInt(cornerVerts[next] / imgWidth);
                x1 = cornerVerts[next] % imgWidth;
                slope = (y1 - y0) / (x1 - x0 + 0.000000001f);
                dot1 = new Vector2(x1 - x0, y1 - y0);
                dot = Vector2.Dot(dot1, dot2);
                if (cornerVerts.Count == 8) rectCheck.Add(dot1.normalized);
                else if (dot < 0.5) { label = ShadowType.unknown; return; }
                if (Mathf.Sign(slope) != Mathf.Sign(prevSlope)) dirChange++;
                prevSlope = slope;
                dot2 = dot1;
            }
            if (!(dirChange == 3 || dirChange == 4)) label = ShadowType.unknown;
            else
            {
                bool rect1 = false, rect2 = false;
                if (cornerVerts.Count == 8) Debug.Log(Vector2.Dot(rectCheck[0], rectCheck[4]) + " , " + Vector2.Dot(rectCheck[2], rectCheck[6]));
                if (cornerVerts.Count == 8 && Mathf.Abs(Vector2.Dot(rectCheck[0], rectCheck[4])) > 0.8 && Mathf.Abs(Vector2.Dot(rectCheck[2], rectCheck[6])) > 0.8) rect1 = true;
                else if (cornerVerts.Count == 8 && Mathf.Abs(Vector2.Dot(rectCheck[1], rectCheck[5])) > 0.8 && Mathf.Abs(Vector2.Dot(rectCheck[3], rectCheck[7])) > 0.8) rect2 = true;

                if (rect1 || rect2)
                {
                    int adjust = rect1 ? 0 : 1;
                    float side0 = Mathf.Sqrt(Mathf.Pow(rectCheck[0 + adjust].x - rectCheck[2 + adjust].x, 2.0f) + Mathf.Pow(rectCheck[0 + adjust].y - rectCheck[2 + adjust].y, 2.0f));
                    float side1 = Mathf.Sqrt(Mathf.Pow(rectCheck[4 + adjust].x - rectCheck[2 + adjust].x, 2.0f) + Mathf.Pow(rectCheck[4 + adjust].y - rectCheck[2 + adjust].y, 2.0f));
                    float side2 = Mathf.Sqrt(Mathf.Pow(rectCheck[6 + adjust].x - rectCheck[4 + adjust].x, 2.0f) + Mathf.Pow(rectCheck[6 + adjust].y - rectCheck[4 + adjust].y, 2.0f));
                    float side3 = Mathf.Sqrt(Mathf.Pow(rectCheck[0 + adjust].x - rectCheck[6 + adjust].x, 2.0f) + Mathf.Pow(rectCheck[0 + adjust].y - rectCheck[6 + adjust].y, 2.0f));
                    float cross1 = (side0 + side2) / 2;
                    float cross2 = (side1 + side3) / 2;

                    if (side0 / side2 > Mathf.Max(1.5f, 1.5f * 10 / cross1) || side2 / side0 > Mathf.Max(1.5f, 1.5f * 10 / cross1) || side1 / side3 > Mathf.Max(1.5f, 1.5f * 10 / cross2) || side3 / side1 > Mathf.Max(1.5f, 1.5f * 10 / cross2)) label = ShadowType.unknown;
                    else
                    {
                        float ratio = cross1 / cross2;
                        Debug.Log("ratio: " + ratio);
                        if (ratio > 0.75 && ratio < 1.25) label = ShadowType.square;
                        else label = ShadowType.rect;
                    }
                }
                else
                {
                    //now circle vs oval
                    int yPos_0 = Mathf.FloorToInt(cornerVerts[0] / imgWidth);
                    int xPos_0 = cornerVerts[0] % imgWidth;
                    int yPos_1 = Mathf.FloorToInt(cornerVerts[cornerVerts.Count / 4] / imgWidth);
                    int xPos_1 = cornerVerts[cornerVerts.Count / 4] % imgWidth;
                    int yPos_2 = Mathf.FloorToInt(cornerVerts[cornerVerts.Count / 2] / imgWidth);
                    int xPos_2 = cornerVerts[cornerVerts.Count / 2] % imgWidth;
                    int yPos_3 = Mathf.FloorToInt(cornerVerts[3 * cornerVerts.Count / 4] / imgWidth);
                    int xPos_3 = cornerVerts[3 * cornerVerts.Count / 4] % imgWidth;

                    float cross1 = Mathf.Sqrt(Mathf.Pow(xPos_2 - xPos_0, 2.0f) + Mathf.Pow(yPos_2 - yPos_0, 2.0f));
                    float cross2 = Mathf.Sqrt(Mathf.Pow(xPos_3 - xPos_1, 2.0f) + Mathf.Pow(yPos_3 - yPos_1, 2.0f));
                    //Debug.Log("cross1: " + cross1 + " cross2: " + cross2 );
                    float ratio = cross1 / cross2;
                    if (ratio > 0.8 && ratio < 1.2) label = ShadowType.circle;
                    else label = ShadowType.oval;
                }
            }
        }
        Debug.Log("-----------------------------------------------label for this shadow: " + label);

        GameObject labelPrefab;
        if (label == ShadowType.circle) labelPrefab = labelPrefabs[0];
        else if (label == ShadowType.oval) labelPrefab = labelPrefabs[1];
        else if (label == ShadowType.rect) labelPrefab = labelPrefabs[2];
        else if (label == ShadowType.square) labelPrefab = labelPrefabs[3];
        else return; // labelPrefab = labelPrefabs[4];
        int yPos, xPos;
        yPos = Mathf.FloorToInt(cornerVerts[0] / imgWidth);
        xPos = cornerVerts[0] % imgWidth;

        GameObject labelList = GameObject.FindWithTag("labeler");
        labelObj = labelList.AddComponent<Label>();
        labelObj.SetLabelPrefab(labelPrefab);
        labelObj.SetTransforms(xPos, yPos);
        labelObj.SetInScene();
    }

    public void ReduceVerts(int width, int runNum)
    {
        int incr = 2;
        Vector2 back; Vector2 front; float angle = 0.0f;
        int xPos_1 = -1; int yPos_1 = -1; int xPos_2 = -1; int yPos_2 = -1; int xPos_3 = -1; int yPos_3 = -1;
        for (int iters = 0; iters < runNum; iters++)
        {
            for (int i = iters % 4 + 1; i < cornerVerts.Count - 1; i += incr)
            {
                yPos_1 = Mathf.FloorToInt(cornerVerts[i - 1] / width);
                xPos_1 = cornerVerts[i - 1] % width;
                yPos_2 = Mathf.FloorToInt(cornerVerts[i] / width);
                xPos_2 = cornerVerts[i] % width;
                yPos_3 = Mathf.FloorToInt(cornerVerts[i + 1] / width);
                xPos_3 = cornerVerts[i + 1] % width;
                back = new Vector2(xPos_2 - xPos_1, yPos_2 - yPos_1);
                front = new Vector2(xPos_3 - xPos_2, yPos_3 - yPos_2);
                angle = Mathf.Abs(Vector2.SignedAngle(back, front));
                if (angle > 175 || angle < 15)
                {
                    cornerVerts.RemoveAt(i);
                    incr = 0;
                }
                else incr = 2;
            }
        }
        if (cornerVerts.Count >= 3)
        {
            yPos_1 = Mathf.FloorToInt(cornerVerts[cornerVerts.Count - 1] / width);
            xPos_1 = cornerVerts[cornerVerts.Count - 1] % width;
            yPos_2 = Mathf.FloorToInt(cornerVerts[0] / width);
            xPos_2 = cornerVerts[0] % width;
            yPos_3 = Mathf.FloorToInt(cornerVerts[1] / width);
            xPos_3 = cornerVerts[1] % width;
            back = new Vector2(xPos_2 - xPos_1, yPos_2 - yPos_1);
            front = new Vector2(xPos_3 - xPos_2, yPos_3 - yPos_2);
            angle = Mathf.Abs(Vector2.SignedAngle(back, front));
            if (angle > 175 || angle < 15)
            {
                cornerVerts.RemoveAt(0);
            }
        }

        if (cornerVerts.Count >= 4)
        {
            yPos_1 = Mathf.FloorToInt(cornerVerts[cornerVerts.Count - 2] / width);
            xPos_1 = cornerVerts[cornerVerts.Count - 2] % width;
            yPos_2 = Mathf.FloorToInt(cornerVerts[cornerVerts.Count - 1] / width);
            xPos_2 = cornerVerts[cornerVerts.Count - 1] % width;
            yPos_3 = Mathf.FloorToInt(cornerVerts[0] / width);
            xPos_3 = cornerVerts[0] % width;
            back = new Vector2(xPos_2 - xPos_1, yPos_2 - yPos_1);
            front = new Vector2(xPos_3 - xPos_2, yPos_3 - yPos_2);
            angle = Mathf.Abs(Vector2.SignedAngle(back, front));
            if (angle > 175 || angle < 15)
            {
                cornerVerts.RemoveAt(cornerVerts.Count - 1);
            }
        }
    }

    public void ReduceVerts_2(int width, int threshold, int movement)
    {
        int incr = 1; float yChange = 0; float xChange = 0;
        int xPos_1 = -1; int yPos_1 = -1; int xPos_3 = -1; int yPos_3 = -1;
        for (int i = 1; i < cornerVerts.Count - 1; i += incr)
        {
            yPos_1 = Mathf.FloorToInt(cornerVerts[i - 1] / width);
            xPos_1 = cornerVerts[i - 1] % width;
            yPos_3 = Mathf.FloorToInt(cornerVerts[i + 1] / width);
            xPos_3 = cornerVerts[i + 1] % width;
            xChange = Mathf.Abs(xPos_3 - xPos_1);
            yChange = Mathf.Abs(yPos_3 - yPos_1);
            if ((xChange < threshold && yChange >= movement) || (xChange >= movement && yChange < threshold)) { cornerVerts.RemoveAt(i); incr = 0; }
            else incr = 1;
        }

        if (cornerVerts.Count >= 3)
        {
            yPos_1 = Mathf.FloorToInt(cornerVerts[1] / width);
            xPos_1 = cornerVerts[1] % width;
            yPos_3 = Mathf.FloorToInt(cornerVerts[cornerVerts.Count - 1] / width);
            xPos_3 = cornerVerts[cornerVerts.Count - 1] % width;
            xChange = Mathf.Abs(xPos_3 - xPos_1);
            yChange = Mathf.Abs(yPos_3 - yPos_1);
            if ((xChange < threshold && yChange >= movement) || (xChange >= movement && yChange < threshold)) { cornerVerts.RemoveAt(0); }
        }

    }

    public void condenseVerts(int width)
    {
        int incr = 2; float distance = 0.0f;
        int xPos_1 = -1; int yPos_1 = -1; int xPos_2 = -1; int yPos_2 = -1;
        for (int i = 1; i < cornerVerts.Count - 1; i += incr)
        {
            yPos_1 = Mathf.FloorToInt(cornerVerts[i - 1] / width);
            xPos_1 = cornerVerts[i - 1] % width;
            yPos_2 = Mathf.FloorToInt(cornerVerts[i] / width);
            xPos_2 = cornerVerts[i] % width;
            distance = Mathf.Sqrt(Mathf.Pow(xPos_2 - xPos_1, 2.0f) + Mathf.Pow(yPos_2 - yPos_1, 2.0f));
            if (distance < 2)
            {
                /*int ind = xPos_2 + yPos_1 * width;
                cornerVerts[i-1] = ind;*/
                cornerVerts.RemoveAt(i);
                incr = 0;
            }
            else incr = 1;
        }
    }

    public void lookAtAngles(int width)
    {
        Vector2 back; Vector2 front; float angle = 0.0f;
        int xPos_1 = -1; int yPos_1 = -1; int xPos_2 = -1; int yPos_2 = -1; int xPos_3 = -1; int yPos_3 = -1;
        for (int i = 1; i < cornerVerts.Count - 1; i += 2)
        {
            yPos_1 = Mathf.FloorToInt(cornerVerts[i - 1] / width);
            xPos_1 = cornerVerts[i - 1] % width;
            yPos_2 = Mathf.FloorToInt(cornerVerts[i] / width);
            xPos_2 = cornerVerts[i] % width;
            yPos_3 = Mathf.FloorToInt(cornerVerts[i + 1] / width);
            xPos_3 = cornerVerts[i + 1] % width;
            back = new Vector2(xPos_2 - xPos_1, yPos_2 - yPos_1);
            front = new Vector2(xPos_3 - xPos_2, yPos_3 - yPos_2);
            angle = Mathf.Abs(Vector2.SignedAngle(back, front));
            Debug.Log(angle);
        }
        for (int i = 2; i < cornerVerts.Count - 1; i += 2)
        {
            yPos_1 = Mathf.FloorToInt(cornerVerts[i - 1] / width);
            xPos_1 = cornerVerts[i - 1] % width;
            yPos_2 = Mathf.FloorToInt(cornerVerts[i] / width);
            xPos_2 = cornerVerts[i] % width;
            yPos_3 = Mathf.FloorToInt(cornerVerts[i + 1] / width);
            xPos_3 = cornerVerts[i + 1] % width;
            back = new Vector2(xPos_2 - xPos_1, yPos_2 - yPos_1);
            front = new Vector2(xPos_3 - xPos_2, yPos_3 - yPos_2);
            angle = Mathf.Abs(Vector2.SignedAngle(back, front));
            Debug.Log(angle);
        }
    }
}
