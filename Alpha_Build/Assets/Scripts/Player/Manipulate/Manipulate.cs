using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manipulate : MonoBehaviour
{
    public float moveForce = 400;
    public Transform holdParent;

    private GameObject heldObj;
    private GameObject tempObj;
    [SerializeField]
    IdentifyShadows shadowScript;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Rigidbody>() && other.gameObject.layer != 5) //dont pick up the shadow labels on the UI layer lol
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
        //float yRot = heldObj.transform.eulerAngles.y + 1;
        //heldObj.transform.eulerAngles = new Vector3(heldObj.transform.eulerAngles.x, yRot, heldObj.transform.eulerAngles.z);
        heldObj.transform.Rotate(0, 1, 0);
    }
    
    void RotateX()
    {
        //float zRot = heldObj.transform.eulerAngles.z + 1;
        //heldObj.transform.eulerAngles = new Vector3(heldObj.transform.eulerAngles.x, heldObj.transform.eulerAngles.y, zRot);
        heldObj.transform.Rotate(0, 0, 1);
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
        UpdateShadows();
        shadowScript.holdingItem = true;
    }

    void DropObject()
    {
        Rigidbody heldRig = heldObj.GetComponent<Rigidbody>();
        heldRig.useGravity = true;
        heldRig.drag = 0;

        heldObj.GetComponent<Collider>().enabled = true;

        heldObj.transform.parent = null;
        heldObj = null;
        UpdateShadowsOnDelay();
        shadowScript.holdingItem = false;
    }

    void UpdateShadows()
    {
        shadowScript.RemoveLabels();
        shadowScript.DetectShadows();
    }

    IEnumerator UpdateShadowsOnDelay()
    {
        yield return new WaitForSeconds(1.25f);
        shadowScript.RemoveLabels();
        shadowScript.DetectShadows();
    }
}