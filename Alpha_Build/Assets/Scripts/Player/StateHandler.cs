using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    private GameObject bow;
    //private GameObject inv;
    private GameObject sword;
    private Inventory inv;
    
    void Start()
    {
        bow = GameObject.Find("Bow");
        inv = GameObject.Find("Main Camera").GetComponent<Inventory>();
        sword = GameObject.Find("Sword");
    }

    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public bool InvOpen()
    {
        return inv.IsOpen();
    }

    public bool BowIsAiming()
    {
        return GameObject.Find("Bow").GetComponent<Bow>().isAiming;
    }

}
