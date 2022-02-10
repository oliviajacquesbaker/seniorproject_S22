using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegment : MonoBehaviour
{

    public GameObject connectedAbove, connectedBelow;
    public Transform anchor;
    private Mesh mesh;
    private Bounds bounds;

    void Start()
    {
        connectedAbove = GetComponent<HingeJoint>().connectedBody.gameObject;
        mesh = connectedAbove.GetComponent<MeshFilter>().mesh;
        bounds = mesh.bounds;
        RopeSegment aboveSegment = connectedAbove.GetComponent<RopeSegment>();

        if (aboveSegment)
        {
            aboveSegment.connectedBelow = gameObject;
            float spawnPos = bounds.size.y;
            GetComponent<HingeJoint>().connectedAnchor = new Vector3(0, (spawnPos-1) * -1, 0);
        }
        else
        {
            GetComponent<HingeJoint>().connectedAnchor = new Vector3(0,0,0);
        }
    }

}
