using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForPlayer : MonoBehaviour
{
    public Boss boss;
    void Start()
    {
        boss = GameObject.Find("Boss").GetComponent<Boss>();
    }

    void OnTriggerEnter(Collider c) 
    {
        if (c.gameObject.tag == "Player")
        {
            boss.playerInRange = true;
        }
    }

    void OnTriggerExit(Collider c) 
    {
        if (c.gameObject.tag == "Player")
        {
            boss.playerInRange = false;
        }
    }
}
