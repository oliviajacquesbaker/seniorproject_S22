using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerThresholdChange : MonoBehaviour
{
    [SerializeField]
    IdentifyShadows shadowScript;
    [SerializeField]
    float original;
    [SerializeField]
    float adjusted;

    [SerializeField]
    List<GameObject> adjustedAntiShadow;

    bool originalActive;
    void Start()
    {
        originalActive = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            /*if (originalActive) shadowScript.lenient = adjusted;
            else shadowScript.lenient = original;*/

            foreach (GameObject anti in adjustedAntiShadow)
            {
                if (originalActive) anti.SetActive(true);
                else anti.SetActive(false);
            }
            originalActive = !originalActive;
        }
    }
}
