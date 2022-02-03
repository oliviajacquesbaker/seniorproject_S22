using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    public int damage;
    public float destroyTimer;

    void Start()
    {
        Destroy(gameObject, destroyTimer);
    }

    void OnTriggerEnter(Collider col)
    {
           if (col.GetComponent<EnemyStats>())
           {
               EnemyStats stats = col.GetComponent<EnemyStats>();
               stats.Hit(damage);
           }
    }

}
