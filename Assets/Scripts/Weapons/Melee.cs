using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{

    public KeyCode attackButton;

    public int damage;

    void OnTriggerStay(Collider collider)
    {
        if (Input.GetKeyDown(attackButton))
        {
            if (collider.GetComponent<EnemyStats>())
            {
                EnemyStats stats = collider.GetComponent<EnemyStats>();
                stats.Hit(damage);
            }
        }
    }

}
