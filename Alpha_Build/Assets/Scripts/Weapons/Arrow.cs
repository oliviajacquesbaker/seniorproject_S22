using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    public int damage;
    public float destroyTimer;
    private Rigidbody rigidbody;
    private bool hit = false;

    void Start()
    {
        Destroy(gameObject, destroyTimer);
        rigidbody = GetComponent<Rigidbody>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (hit) return;

        if (col.gameObject.GetComponent<_AIStatsController>())
        {
            int additionalDmg = 0;
            if (col.gameObject.name == "BOSSL_hand" || col.gameObject.name == "BOSSR_hand")
            {
                if (GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().isRecovering) { additionalDmg += 50; Debug.Log("critical hit!"); }
            }

            _AIStatsController stats = col.gameObject.GetComponent<_AIStatsController>();
            stats.DetractHealth(damage + additionalDmg, true);
            Debug.Log("Hit enemy!");

            transform.parent = col.transform;
            hit = true;
            //Destroy(gameObject);
        }

        else if (col.gameObject.GetComponent<StatsLinker>())
        {
            StatsLinker stats = col.gameObject.GetComponent<StatsLinker>();
            int additionalDmg = 0;
            if (col.gameObject.name == "BOSSL_hand" || col.gameObject.name == "BOSSR_hand")
            {
                if (GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>().isRecovering) { additionalDmg += 50; Debug.Log("critical hit!"); }
            }
            stats.statsController.DetractHealth(damage + additionalDmg, true);
            hit = true;
        }

        if (!col.isTrigger)
        {
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }

        Invoke("Destroy", 1f);
    }

    void Destroy()
    {
        Destroy(gameObject);
    }

}
