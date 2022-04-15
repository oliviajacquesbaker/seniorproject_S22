using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Patrol : MonoBehaviour
{
    public int damage_p;
    private Patrol_Movement patrol;

    void Start()
    {
        patrol = GetComponentInParent<Patrol_Movement>();
    }
    void OnTriggerEnter(Collider collider)
    {
        if (patrol.IsDead()) return;

        if (collider.GetComponent<_PlayerStatsController>())
        {
            _PlayerStatsController stats = collider.GetComponent<_PlayerStatsController>();
            stats.DetractHealth(damage_p, true);
        }
    }


}
