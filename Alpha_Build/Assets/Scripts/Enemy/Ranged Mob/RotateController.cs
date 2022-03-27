using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateController : MonoBehaviour
{
    [SerializeField]
    GameObject player, arm, body;

    Transform armDefault, bodyDefault;

    private bool inside = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    private void OnTriggerEnter(Collider other)
    {
        inside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inside = false;

            arm.transform.rotation = armDefault.rotation;
            body.transform.rotation = bodyDefault.rotation;
        }
        
    }

    private void LookAtIgnoreHeight()
    {
        Vector3 lookAtPos = player.transform.position;

        lookAtPos.y = arm.transform.position.y;
        arm.transform.LookAt(lookAtPos);
        arm.transform.rotation *= Quaternion.FromToRotation(Vector3.left, Vector3.forward);
    }

    private void LookAtHeight()
    {
        Vector3 lookAtPos = player.transform.position;

        body.transform.LookAt(lookAtPos);
        body.transform.rotation *= Quaternion.FromToRotation(Vector3.left, Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        if (inside)
        {
            LookAtIgnoreHeight();
            LookAtHeight();
        }
    }
}
