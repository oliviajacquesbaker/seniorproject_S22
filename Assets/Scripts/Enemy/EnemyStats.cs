using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{

    public int health;

    public void Hit(int damage)
    {
        health -= damage;
        Debug.Log("Enemy health: " + health.ToString());
    }

    void Update()
    {
        if (health <= 0)
        {
            health = 0;
            Destroy(gameObject);
        }
    }

}
