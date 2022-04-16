using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateShadow : MonoBehaviour
{

    [SerializeField]
    IdentifyShadows shadowScripter;
    float timeSinceLastUpdate = 0f;
    // [SerializeField]
    // Collider col;
    void Update()
    {
        timeSinceLastUpdate += Time.deltaTime;
    }
    void OnTriggerEnter(Collider col)
    {
        //if (timeSinceLastUpdate < 3f) return;

        if (col.gameObject.tag == "Player")
        {
            shadowScripter.DetectShadows();
            timeSinceLastUpdate = 0f;
        }
    }
}
