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
        if (col.GetComponent<_AIStatsController>() && !col.isTrigger)
        {
            _AIStatsController stats = col.GetComponent<_AIStatsController>();
            stats.DetractHealth(damage, true);
            Debug.Log("Hit enemy!");
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

}
