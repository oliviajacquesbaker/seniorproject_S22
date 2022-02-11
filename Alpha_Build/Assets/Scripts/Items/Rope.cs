using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public int ropeLength = 1;
    public float partDistance = 0.12f;
    public GameObject partPrefab, parentObj;
    public bool reset, spawn, snapFirst, snapLast;

    void Update()
    {
        if (reset)
        {
            foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Rope"))
            {
                Destroy(temp);
            }

            reset = false;
        }

        if (spawn)
        {
            Spawn();

            spawn = false;
        }
    }

    void Spawn()
    {
        int count = (int) (ropeLength / partDistance);

        for (int i = 0; i < count; i++)
        {
            GameObject temp;
            temp = Instantiate(partPrefab, new Vector3(transform.position.x, (transform.position.y + partDistance * (i+1)*-1), transform.position.z), Quaternion.identity, parentObj.transform);
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
    }
}