using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerThresholdChange : MonoBehaviour
{
    [SerializeField]
    IdentifyShadows shadowScript;
    [SerializeField]
    float original;
    [SerializeField]
    float adjusted;

    [SerializeField]
    List<GameObject> originalAntiShadow;
    [SerializeField]
    List<GameObject> adjustedAntiShadow;

    bool originalActive;
    void Start()
    {
        originalActive = true;
    }
    void OnEnable()
    {
        Debug.Log("OnEnable called");

        Debug.Log("loaded");
        originalActive = true;
        foreach (GameObject anti in adjustedAntiShadow)
        {
            anti.SetActive(false);
        }

        foreach (GameObject anti in originalAntiShadow)
        {
            anti.SetActive(true);
        }
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

            foreach (GameObject anti in originalAntiShadow)
            {
                if (originalActive) anti.SetActive(false);
                else anti.SetActive(true);
            }

            originalActive = !originalActive;
        }
    }
}
