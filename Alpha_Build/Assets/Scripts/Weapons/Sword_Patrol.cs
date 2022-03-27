using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Patrol : MonoBehaviour
{
    public int damage_p;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<_PlayerStatsController>())
        {
            _PlayerStatsController stats = collider.GetComponent<_PlayerStatsController>();
            stats.DetractHealth(damage_p, true);
        }
    }


}
