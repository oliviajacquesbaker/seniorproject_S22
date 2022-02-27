using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowResponse : MonoBehaviour
{
    void OnMouseDown()
    {
        Debug.Log("CLICKED ONMOUSEDOWN");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTERED, ITEM COLLECTED");
    }
    private void OnTriggerExit(Collider other)
    {
        //Debug.Log("TRIGGER EXITED");
    }
}
