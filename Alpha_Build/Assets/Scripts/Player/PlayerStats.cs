using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public int health;

    public bool isHit;

    void Update()
    {
        if (health <= 0 )
        {
            Debug.Log("Dead");
        }
    }

    public void Hit(int damage)
    {
        isHit = true;
        health -= damage;
        isHit = false;
    }

}
