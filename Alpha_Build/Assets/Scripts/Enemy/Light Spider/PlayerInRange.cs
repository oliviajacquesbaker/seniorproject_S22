using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInRange : MonoBehaviour
{
    public bool playerInRange;

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            Debug.Log("player in range");
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }
    }
}
