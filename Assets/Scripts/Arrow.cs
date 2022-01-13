using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    public int damage;

    void OnTriggerEnter(Collider col)
    {
           if (col.GetComponent<EnemyStats>())
           {
               EnemyStats stats = col.GetComponent<EnemyStats>();
               stats.Hit(damage);
           }
    }
}
