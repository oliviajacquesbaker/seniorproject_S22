using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public string playerTag;

    public int damage;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == playerTag)
        {
            PlayerStats stats = collider.gameObject.GetComponent<PlayerStats>();
            stats.Hit(damage);
        }
    }

}
