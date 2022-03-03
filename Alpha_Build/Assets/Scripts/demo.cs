using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class demo : MonoBehaviour
{
    [SerializeField]
    List<Light> prevLights;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("ENTERED");
            for (int i = 0; i < prevLights.Count; ++i)
            {
                prevLights[i].intensity = 0;
            }
        }
    }
}
