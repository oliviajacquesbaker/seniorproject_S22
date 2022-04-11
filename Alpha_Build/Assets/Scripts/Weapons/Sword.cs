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
    bool coroutineStarted = false;
    private int frameStarted;
    Durability durability;

    public int damage;
    [SerializeField]
    private AudioSource source;

    [SerializeField]
    private AudioClip sword1, sword2, sword3;
    private int clipCounter = 1;
    void Start()
    {
        startingPos = transform.rotation.z;
        anim = GameObject.Find("Player").GetComponent<Animator>();
        durability = gameObject.GetComponent<Durability>();
    }

    void Update()
    {
        if (Input.GetKey(attackButton))
        {
            Attack();
            //anim.ResetTrigger("SwordAttack");

            PlaySoundClip();
        }
        if(!coroutineStarted && attacking) StartCoroutine(AllowAttack());
        else if (coroutineStarted && Time.frameCount - frameStarted > 60) StartCoroutine(AllowAttack()); //the above section gets disturbed when players use menus
    }

    void PlaySoundClip()
    {
        switch (clipCounter){
            case 1:
                source.clip = sword1;
                break;
            case 2:
                source.clip = sword2;
                break;
            case 3:
                source.clip = sword3;
                break;
        }
        clipCounter++;
        if(clipCounter == 4)
        {
            clipCounter = 1;
        }
        source.Play();
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
        durability.currDurability -= 8;
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
        coroutineStarted = true;
        frameStarted = Time.frameCount;
        yield return new WaitForSeconds(0.5f);
        coroutineStarted = false;
        attacking = false;
    }

    void OnDrawGizmosSelected()
    {
        if (!attackPoint) return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

}
