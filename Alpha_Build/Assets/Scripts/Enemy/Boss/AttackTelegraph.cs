using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTelegraph : MonoBehaviour
{
    private Boss boss;

    void Start()
    {
        Destroy(gameObject, 3);
        boss = GameObject.Find("Boss").GetComponent<Boss>();
    }

    void OnDrawGizmosSelected() 
    {
        //if (telegraphEffect == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GameObject.Find("Inner").transform.position, boss.attackRadius);
    }

}
