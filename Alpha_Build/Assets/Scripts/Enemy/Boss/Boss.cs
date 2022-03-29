using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Collider[] colliders;
    private _AIStats stats;
    private MeshRenderer[] renderer;
    public GameObject player, attackTelegraph;
    private Transform telegraphEffect;
    public bool playerInRange, debug, isRecovering = false;
    public float rotationSpeed, telegraphTime, attackRadius, smashAttackDamage, phaseTwoThreshold, attackCooldown, attackRecoveryTime;
    private bool coroutineStarted;
    private int phase = 1;
    private Color original;
    private float timeSinceLastAttack;

    void Start()
    {
        player = GameObject.Find("Player");
        //renderer = GameObject.Find("Body").GetComponent<MeshRenderer>();
        stats = GetComponent<_AIStats>();
        timeSinceLastAttack = attackCooldown / 2f;
        colliders = GetComponentsInChildren<Collider>();
        renderer = GetComponentsInChildren<MeshRenderer>();
        original = renderer[0].material.color;
    }

    void Update()
    {
        if (!playerInRange) return; 

        LookAtPlayer();

        if (stats.GetHealth() <= phaseTwoThreshold)
        {
            SetPhase(2);
        }

        switch (phase)
        {
            case 1:
                if (timeSinceLastAttack > attackCooldown)
                {   
                    Attack();
                }
                break;
            case 2:
            {
                // phase 2 attacks here
                Debug.Log("Phase 2!");
                break;
            }
        }

        timeSinceLastAttack += Time.deltaTime;
    }

    void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    void TelegraphAttack()
    {
        if (isRecovering) return;

        GameObject temp = Instantiate(attackTelegraph, player.transform.position, Quaternion.identity);
        telegraphEffect = temp.transform;
        Debug.Log("Spawned telegraph");
        debug = true;
    }

    void Attack()
    {
        TelegraphAttack();
        Invoke("DamageArea", telegraphTime);
        StartCoroutine(AttackRecover());
        debug = true;
        timeSinceLastAttack = 0;
    }
    void DamageArea()
    {
        if (isRecovering) return;

        Debug.Log("Attack!");
        Collider[] colliders = Physics.OverlapSphere(telegraphEffect.position, attackRadius);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                _PlayerStatsController stats = c.gameObject.GetComponent<_PlayerStatsController>();
                stats.DetractHealth(smashAttackDamage, true);
            }
        }

        isRecovering = true;
    }

    // void AttackRecover()
    // {
    //     float timePassed = 0f;

    //     while (timePassed < attackRecoveryTime)
    //     {
    //         yield return null;
    //     }

    //     timePassed += Time.deltaTime;
    // }

    IEnumerator AttackRecover()
    {
        if (coroutineStarted)
            yield break;

        coroutineStarted = true;
        //isRecovering = true;
        yield return new WaitForSeconds(attackRecoveryTime);
        Debug.Log("Recovered!");

        isRecovering = false;
        coroutineStarted = false;
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Something hit me!!");
        //if (col.gameObject.tag == "Arrow")
        //{
        foreach (Renderer r in renderer)
        {
            r.material.color = Color.red;
        }

        foreach (Collider c in colliders)
        {
            if (c.gameObject.name == "Left Hand" || c.gameObject.name == "Right Hand")
            {
                _AIStatsController statsController = GetComponent<_AIStatsController>();
                if (isRecovering)
                {
                    statsController.DetractHealth(50, true); // magic number needs to be changed
                    Debug.Log("Critical strike!");
                }
                Debug.Log("Hit " + c.gameObject.name);
            }
        }
        Invoke("ResetColor", 0.1f);
        Debug.Log("HP:" + stats.GetHealth());
        //}
    }

    void ResetColor()
    {
        foreach (Renderer r in renderer)
        {
            r.material.color = original;
        }
    }

    void SetPhase(int phase)
    {
        this.phase = phase;
    }
}
