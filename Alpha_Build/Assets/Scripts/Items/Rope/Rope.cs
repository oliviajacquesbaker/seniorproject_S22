using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    private float ropeLength;
    public float partDistance = 0.15f, lengthModifier = 0.80f;
    public GameObject partPrefab, parentObj, floor;
    public bool reset, spawn, snapFirst, snapLast;
    private float hookPos, floorPos;
    private Hook hook;

    void Start()
    {
        DistanceToFloor();
        hook = GetComponentInParent<Hook>();
    }
    void Update()
    {
        if (reset)
        {
            Reset();
        }

        if (spawn)
        {
            Spawn();
        }
    }

    public void Spawn()
    {
        if (hook.hasRope) { return; }

        DistanceToFloor();
        int count = (int) ((ropeLength*lengthModifier) / partDistance);
        //Debug.Log("Rope length: " + ropeLength + ". There are " + count + " segments.");

        for (int i = 0; i < count; i++)
        {
            GameObject temp;
            temp = Instantiate(partPrefab, new Vector3(parentObj.transform.position.x, (parentObj.transform.position.y + partDistance * (i+1)*-1), parentObj.transform.position.z), Quaternion.identity, parentObj.transform);
            temp.transform.eulerAngles = new Vector3(180,0,0);

            temp.name = parentObj.transform.childCount.ToString();

            if (i==0)
            {
                Destroy(temp.GetComponent<CharacterJoint>());
                if (snapFirst)
                {
                    temp.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                }
            }
            else
            {
                temp.GetComponent<CharacterJoint>().connectedBody = parentObj.transform.Find((parentObj.transform.childCount - 1).ToString()).GetComponent<Rigidbody>();
            }

        }

        if (snapLast)
        {
            parentObj.transform.Find((parentObj.transform.childCount).ToString()).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        spawn = false;
    } 

    public void Reset()
    {
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Rope"))
        {
            Destroy(temp);
        }

        reset = false;
    }  
    void DistanceToFloor()
    {
        hookPos = parentObj.transform.position.y;
        floorPos = floor.transform.position.y;
        ropeLength = hookPos - floorPos;
    }
}