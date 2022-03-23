using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public GameObject player, attackTelegraph;
    private Transform telegraphEffect;
    public bool playerInRange, debug = false;
    public float rotationSpeed, telegraphTime, attackRadius, smashAttackDamage;
    public bool coroutineStarted = false;

    void Start()
    {
        player = GameObject.Find("Player");
    }

    void Update()
    {
        if (!playerInRange) { return; }
        
        LookAtPlayer();

        if (!debug)
        {   
            TelegraphAttack();
            StartCoroutine(TelegraphDelay());
            //Attack();
            debug = true;
            //timePassed += Time.deltaTime;
        }

    }

    void LookAtPlayer()
    {
        Vector3 direction = player.transform.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

    void TelegraphAttack()
    {
        GameObject temp = Instantiate(attackTelegraph, player.transform.position, Quaternion.identity);
        telegraphEffect = temp.transform;
        Debug.Log("Spawned telegraph");
        debug = true;
    }
    void Attack()
    {
        //TelegraphAttack();
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
    }

    IEnumerator TelegraphDelay()
    {
        if (coroutineStarted)
            yield break;

        coroutineStarted = true;

        yield return new WaitForSeconds(telegraphTime);
        Attack();
        coroutineStarted = false;
    }

}
