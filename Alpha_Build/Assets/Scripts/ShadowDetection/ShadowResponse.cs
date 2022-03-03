using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowResponse : MonoBehaviour
{
    [SerializeField]
    private ShadowType itemType;
    private bool readyToCollect = true;
    private Label connectedLabel;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER ENTERED, ITEM COLLECTED");
        if (readyToCollect)
        {
            Camera.main.GetComponent<Inventory>().EnableItem(itemType);
            readyToCollect = false;
        }
    }

    private void SetLabel(Label label)
    {
        connectedLabel = label;
    }
}
