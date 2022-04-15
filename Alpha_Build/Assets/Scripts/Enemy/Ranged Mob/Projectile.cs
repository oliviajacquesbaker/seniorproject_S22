using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{


    [SerializeField]
    _PlayerStatsController player;


    private Rigidbody projRB;
    private void Awake()
    {
        projRB = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<_PlayerStatsController>();
    }

    public void Fire(float speed, Vector3 direction)
    {
        projRB.velocity = direction * speed;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
            player.DetractHealth(20f, true);
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }


}
