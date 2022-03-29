using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    public int damage;
    public float destroyTimer;
    private Rigidbody rigidbody;

    void Start()
    {
        Destroy(gameObject, destroyTimer);
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.GetComponent<_AIStatsController>())
        {
            _AIStatsController stats = col.gameObject.GetComponent<_AIStatsController>();
            stats.DetractHealth(damage, true);
            Debug.Log("Hit enemy!");

            transform.parent = col.transform;
            //Destroy(gameObject);
        }

        if (!col.isTrigger)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

    }

}
