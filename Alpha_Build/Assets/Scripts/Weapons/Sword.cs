using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    private float startingPos;
    public float targetPos;
    public float attackRange = 0.5f;
    public KeyCode attackButton;
    public Transform attackPoint;
    public LayerMask enemyLayers;
    [SerializeField]
    public Animator anim;

    public int damage;

    void Start()
    {
        startingPos = transform.rotation.z;
        anim = GameObject.Find("Player").GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKey(attackButton))
        {
            Attack();
            //anim.ResetTrigger("SwordAttack");
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (Input.GetKeyDown(attackButton))
        {
            if (collider.GetComponent<_AIStatsController>())
            {
                _AIStatsController stats = collider.GetComponent<_AIStatsController>();
                stats.DetractHealth(damage);
            }
        }
    }

    void Attack()
    {
        //add animation here
        anim.SetTrigger("SwordAttack");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider enemy in hitEnemies)
        {
            //Debug.Log("Hit enemy " + enemy.name);
            if (enemy.GetComponent<_AIStatsController>())
            {
                _AIStatsController stats = enemy.GetComponent<_AIStatsController>();
                stats.DetractHealth(damage);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (!attackPoint) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
