using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Patrol_Movement : MonoBehaviour
{
    [SerializeField]
    Transform[] points;
    [SerializeField]
    GameObject raycastSource;
    private int currentDestination;
    private NavMeshAgent patrolMob;
    private Animator anim;
    private bool idling;
    private bool spottedPlayer;
    private bool lockedOn;
    private bool dead;
    private GameObject player;
    private IEnumerator coroutine;

    [SerializeField]
    AudioSource source;
    [SerializeField]
    AudioClip step1, step2, step3, step4, step5, attack;

    private int choice = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        patrolMob = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentDestination = 0;
        patrolMob.autoBraking = false;
        idling = false;
        spottedPlayer = false;
        lockedOn = false;
        dead = false;

        coroutine = VisitNextPoint(0);
        StartCoroutine(coroutine);
    }

    private IEnumerator VisitNextPoint(float delayTime)
    {
        idling = true;
        yield return new WaitForSeconds(delayTime);
        anim.SetBool("Patrol", true);
        patrolMob.destination = points[currentDestination].position;
        currentDestination = (++currentDestination) % points.Length;
        idling = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) return;

        Vector3 towardsPlayer = player.transform.position - raycastSource.transform.position;
        if (!spottedPlayer && towardsPlayer.magnitude < 20)
        {
            //float angle = Mathf.Atan2(towardsPlayer.y, towardsPlayer.x) * Mathf.Rad2Deg;
            float dot = Vector3.Dot(towardsPlayer.normalized, raycastSource.transform.forward);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

            //if (angle < 0) angle += 180;

            //Debug.Log(angle);

            bool allowedEntry = false;
            if (towardsPlayer.magnitude > 15 && angle < 5) allowedEntry = true;
            else if (towardsPlayer.magnitude < 15 && angle < 20) allowedEntry = true;
            else if (towardsPlayer.magnitude < 7 && angle < 40) allowedEntry = true;
            else if (towardsPlayer.magnitude < 3 && angle < 170) allowedEntry = true;
            if (towardsPlayer.magnitude < 1.5) allowedEntry = true;

            if (allowedEntry)
            {
                if (Physics.Raycast(transform.position, towardsPlayer, 20))
                {
                    SpotPlayer();
                }
            }
        }
        else if (spottedPlayer && (transform.position - player.transform.position).magnitude > 3)
        {
            if (!Physics.Raycast(raycastSource.transform.position, towardsPlayer, 20))
            {
                //Debug.Log("lose sight of player");
                LoseSightOfPlayer();
            }
        }

        if (lockedOn)
        {
            if ((transform.position - player.transform.position).magnitude <= 2)
            {
                anim.SetBool("WithinRange", true);
            }
            else
            {
                anim.SetBool("WithinRange", false);
                patrolMob.destination = player.transform.position;
            }
        }

        if (!lockedOn && !idling && !patrolMob.pathPending && patrolMob.remainingDistance < 0.2)
        {
            SearchIdle();
        }
    }

    private void SearchIdle()
    {
        anim.SetBool("Patrol", false);
        float randomDelay = Random.Range(2, 6);
        coroutine = VisitNextPoint(randomDelay);
        StartCoroutine(coroutine);
    }

    private void SpotPlayer()
    {
        transform.LookAt(player.transform);
        patrolMob.ResetPath();
        anim.SetBool("SeesPlayer", true);
        spottedPlayer = true;
        StopCoroutine(coroutine);
        coroutine = LockOn(0.5f);
        StartCoroutine(coroutine);
    }

    private void LoseSightOfPlayer()
    {
        anim.SetBool("SeesPlayer", false);
        anim.SetBool("LockedOn", false);
        spottedPlayer = false;
        lockedOn = false;
        patrolMob.ResetPath();
        patrolMob.speed = 3.5f;
        patrolMob.angularSpeed = 200;
        StopCoroutine(coroutine);
        SearchIdle();
    }

    private IEnumerator LockOn(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        if (spottedPlayer)
        {
            anim.SetBool("LockedOn", true);
            lockedOn = true;
            patrolMob.destination = player.transform.position;
            patrolMob.speed = 7.5f;
            patrolMob.angularSpeed = 300;
        }
    }

    public void OnDeath()
    {
        anim.SetBool("Dead", true);
        anim.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        dead = true;
        patrolMob.ResetPath();
        patrolMob.isStopped = true;
    }

    public void FootStep()
    {
        int randChoice = Random.Range(1, 4);

        choice++;

        if (choice > 5)
            choice = 1;

        switch (choice)
        {
            case 1:
                source.clip = step1;
                break;
            case 2:
                source.clip = step2;
                break;
            case 3:
                source.clip = step3;
                break;
            case 4:
                source.clip = step4;
                break;
            case 5:
                source.clip = step5;
                break;
        }

        source.Play();
    }

    public void AttackSound()
    {
        source.clip = attack;
        source.Play();
    }

}


