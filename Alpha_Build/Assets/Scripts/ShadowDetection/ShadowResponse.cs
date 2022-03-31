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
        if(other.gameObject.tag == "Player")
        {
            Debug.Log("TRIGGER ENTERED, ITEM COLLECTED, TYPE: " + itemType + " at " + gameObject.transform.position);
            if (readyToCollect)
            {
                Camera.main.GetComponent<Inventory>().EnableItem(itemType);
                connectedLabel.CollectShadow();
                readyToCollect = false;
            }
        }
    }

    public void SetLabel(Label label)
    {
        connectedLabel = label;
    }
}
