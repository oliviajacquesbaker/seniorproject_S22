using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulate : MonoBehaviour
{
    public float moveForce = 400;
    public Transform holdParent;

    private GameObject heldObj;
    private GameObject tempObj;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>())
        {
            tempObj = other.transform.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        tempObj = null;
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(heldObj == null && tempObj != null) {
                PickupObject();
            }
            else if(heldObj != null)
            {
                DropObject();
            }
        }
        if(heldObj != null)
        {
            if (Input.GetKey(KeyCode.R))
            {
                RotateX();
            }
            else if (Input.GetKey(KeyCode.Q))
            {
                RotateY();
            }
            MoveObject();
        }
    }

    void RotateY()
    {
        float yRot = heldObj.transform.eulerAngles.y + 1;
        heldObj.transform.eulerAngles = new Vector3(heldObj.transform.eulerAngles.x, yRot, heldObj.transform.eulerAngles.z);
    }
    
    void RotateX()
    {
        float zRot = heldObj.transform.eulerAngles.z + 1;
        heldObj.transform.eulerAngles = new Vector3(heldObj.transform.eulerAngles.x, heldObj.transform.eulerAngles.y, zRot);
    }

    void MoveObject()
    {
        if (Vector3.Distance(heldObj.transform.position, holdParent.position) > .03f)
        {
            Vector3 moveDirection = (holdParent.position - heldObj.transform.position);
            heldObj.GetComponent<Rigidbody>().AddForce(moveDirection * moveForce);
        }
    }

    void PickupObject()
    {
        heldObj = tempObj;
        Rigidbody heldRig = heldObj.GetComponent<Rigidbody>();
        heldRig.useGravity = false;
        heldRig.drag = 10;

        heldRig.transform.parent = holdParent;

        heldObj.GetComponent<Collider>().enabled = false;
    }

    void DropObject()
    {
        Rigidbody heldRig = heldObj.GetComponent<Rigidbody>();
        heldRig.useGravity = true;
        heldRig.drag = 0;

        heldObj.GetComponent<Collider>().enabled = true;

        heldObj.transform.parent = null;
        heldObj = null;
    }
}