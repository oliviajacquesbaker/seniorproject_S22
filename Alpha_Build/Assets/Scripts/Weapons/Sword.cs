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
    [SerializeField]
    Boss boss;
    bool attacking = false;
    int attacked = 0;

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
        /*if (!attacking && Input.GetKeyDown(attackButton))
        {
            if (collider.GetComponent<_AIStatsController>())
            {
                _AIStatsController stats = collider.GetComponent<_AIStatsController>();
                stats.DetractHealth(damage);
            }
        }*/
    }

    void Attack()
    {
        if (attacking) return;
        attacked++;
        attacking = true;
        //add animation here
        anim.SetTrigger("SwordAttack");

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        foreach(Collider enemy in hitEnemies)
        {
            
            if (enemy.GetComponent<_AIStatsController>())
            {
                Debug.Log(attacked + ": Hit enemy " + enemy.name);
                _AIStatsController stats = enemy.GetComponent<_AIStatsController>();
                int additionalDmg = 0;
                if (enemy.gameObject.name == "BOSSL_hand" || enemy.gameObject.name == "BOSSR_hand")
                {
                    if (boss.isRecovering) { additionalDmg += 50; Debug.Log("critical hit!"); }
                }
                stats.DetractHealth(damage + additionalDmg, true);
            }
            else if (enemy.GetComponent<StatsLinker>())
            {
                Debug.Log(attacked + ": Hit enemy " + enemy.name);
                StatsLinker stats = enemy.GetComponent<StatsLinker>();
                int additionalDmg = 0;
                if (enemy.gameObject.name == "BOSSL_hand" || enemy.gameObject.name == "BOSSR_hand")
                {
                    if (boss.isRecovering) { additionalDmg += 50; Debug.Log("critical hit!"); }
                }
                stats.statsController.DetractHealth(damage + additionalDmg, true);
            }
        }
        StartCoroutine(AllowAttack());
    }

    IEnumerator AllowAttack()
    {
        
        yield return new WaitForSeconds(0.5f);
        attacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!attackPoint) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
