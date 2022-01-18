using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{

    MeshRenderer renderers;

    Rigidbody gravity;

    // Start is called before the first frame update
    void Start()
    {
        renderers = GetComponent<MeshRenderer>();
        gravity = GetComponent<Rigidbody>();

        gravity.useGravity = false;
        renderers.enabled = false;
    }

    [SerializeField]
    int timeToWait = 1;

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeToWait) {
            EnableFalling();
        }
    }


    void EnableFalling()
    {
        renderers.enabled = true;
        gravity.useGravity = true;
    }

}
