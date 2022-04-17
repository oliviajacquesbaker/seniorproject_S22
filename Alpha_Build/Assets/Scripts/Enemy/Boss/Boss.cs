using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Boss : MonoBehaviour
{
    private Collider[] colliders;
    private _AIStats stats;
    private MeshRenderer[] renderer;
    public GameObject player, attackTelegraph;
    private Transform telegraphEffect;
    public bool playerInRange, debug, isRecovering = false, isAttacking = false;
    public float rotationSpeed, telegraphTime, attackRadius, smashAttackDamage, phaseTwoThreshold, attackCooldown, attackRecoveryTime;
    private bool coroutineStarted;
    private int phase = 1;
    private Color original;
    private float timeSinceLastAttack;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Animator swordAnim;
    private bool rightHand;
    private bool dead = false;
    [SerializeField]
    SwapMaterial swapMat;
    [SerializeField]

    Light pointInner;
    [SerializeField]
    Light pointOut;

    [SerializeField]
    private AudioSource beamattack;
    [SerializeField]
    private AudioSource smackattack;

    [SerializeField]
    GameObject winscreen;

    private float projSpeed;
    [SerializeField]
    private Projectile projectilePrefab;
    public GameObject spawnPoint;



    void Start()
    {
        player = GameObject.Find("Player");
        //renderer = GameObject.Find("Body").GetComponent<MeshRenderer>();
        stats = GetComponent<_AIStats>();
        timeSinceLastAttack = attackCooldown / 2f;
        colliders = GetComponentsInChildren<Collider>();
        renderer = GetComponentsInChildren<MeshRenderer>();

        //original = renderer[0].material.color;

        //        original = renderer[0].material.color;

        rightHand = true;
        //spawnPoint = GameObject.Find("ProjectileSpawn");
        projSpeed = 15f;
    }

    void Update()
    {
        if (!playerInRange || dead) return;

        if (!dead && !isAttacking && !isRecovering) LookAtPlayer();

        if (phase == 1 && stats.GetHealth() <= phaseTwoThreshold)
        {
            SetPhase(2);
        }

        switch (phase)
        {
            case 1:
                if (!isAttacking && timeSinceLastAttack > attackCooldown)
                {
                    Attack();
                }
                break;
            case 2:
                {
                    // phase 2 attacks here
                    attackCooldown = 2f;
                    if (timeSinceLastAttack > attackCooldown)
                    {
                        StartCoroutine(ChargeUpBeam());
                    }
                    break;
                }
        }

        if (!isAttacking) timeSinceLastAttack += Time.deltaTime;
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

        GameObject temp = Instantiate(attackTelegraph, new Vector3(player.transform.position.x, 1.06f, player.transform.position.z), Quaternion.identity);
        telegraphEffect = temp.transform;
        Debug.Log("Spawned telegraph");
        debug = true;
    }

    void Attack()
    {
        if (dead) return;
        isAttacking = true;

        Vector3 dist = player.transform.position - transform.position;
        if (dist.magnitude < 6) anim.SetBool("BH_Close", true);
        else if (dist.magnitude < 9)
        {
            if (rightHand) anim.SetBool("RH_Mid", true);
            else anim.SetBool("LH_Mid", true);
        }
        else if (dist.magnitude < 13)
        {
            if (rightHand) anim.SetBool("RH_Far", true);
            else anim.SetBool("LH_Far", true);
        }
        else
        {
            if (rightHand) anim.SetBool("RH_EFar", true);
            else anim.SetBool("LH_EFar", true);
        }
        rightHand = !rightHand;

        TelegraphAttack();
        Invoke("DamageArea", telegraphTime);
        StartCoroutine(AttackRecover());
        debug = true;
        timeSinceLastAttack = 0;
    }

    void DamageArea()
    {
        if (isRecovering) return;

        smackattack.Play();
        Debug.Log("Attack!");
        Collider[] colliders = new Collider[0];
        if(telegraphEffect) colliders = Physics.OverlapSphere(telegraphEffect.position, attackRadius);

        foreach (Collider c in colliders)
        {
            if (c.gameObject.tag == "Player")
            {
                _PlayerStatsController stats = c.gameObject.GetComponent<_PlayerStatsController>();
                stats.DetractHealth(smashAttackDamage, true);
                break;
            }
        }

        isRecovering = true;
    }

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
        isAttacking = false;

        anim.SetBool("BH_Close", false);
        anim.SetBool("RH_Mid", false);
        anim.SetBool("LH_Mid", false);
        anim.SetBool("RH_Far", false);
        anim.SetBool("LH_Far", false);
        anim.SetBool("RH_EFar", false);
        anim.SetBool("LH_EFar", false);
    }

    //this function animates the boss to rear back, charging up
    // then it holds the charge up position for attackRecoveryTime (you can change this time as you please)
    // then it fires the beam. the actual functionality that damages the player / sends a beam of light out should happen in FireBeam
    IEnumerator ChargeUpBeam()
    {
        if (coroutineStarted)
            yield break;

        isAttacking = true;
        coroutineStarted = true;
        anim.SetBool("LongRangeAttack", true);
        yield return new WaitForSeconds(attackRecoveryTime);
        Debug.Log("Charged!");

        coroutineStarted = false;
        FireBeam();
    }

    private void FireBeam()
    {
        if (dead) return;
        anim.SetBool("LongRangeAttack", false);

        // FIRE AT PLAYER HERE !!! 
        // animations already set up!
        FireProj();

        timeSinceLastAttack = 0;
        isAttacking = false;
    }


    private void FireProj() // copied from ranged mob attack
    {
        beamattack.Play();

        var position = spawnPoint.transform.position + spawnPoint.transform.forward;

        var rotation = spawnPoint.transform.rotation;

        var projectile = Instantiate(projectilePrefab, position, rotation);
        var projectile1 = Instantiate(projectilePrefab, position, rotation);
        var projectile2 = Instantiate(projectilePrefab, position, rotation);

        Vector3 lookAtPos = player.transform.position;

        projectile.Fire(projSpeed, Vector3.Normalize(spawnPoint.transform.forward));
        projectile1.Fire(projSpeed, Vector3.Normalize(GameObject.Find("Spawn1").transform.forward));
        projectile2.Fire(projSpeed, Vector3.Normalize(GameObject.Find("Spawn2").transform.forward));
    }

    void OnTriggerEnter(Collider col)
    {
        //Debug.Log("Something hit me!! " + col.gameObject.name + ", " + col.gameObject.tag);
        if (col.gameObject.tag == "Arrow" || (col.gameObject.tag == "Weapon" && swordAnim.GetCurrentAnimatorStateInfo(0).IsName("Attack_sword")))
        {
            // i CAN reimplement this in the boss if you'd like, but i'm going to worry about it later :thumbsup:
            /*foreach (Renderer r in renderer)
            {
                r.material.color = Color.red;
            }
            Invoke("ResetColor", 0.1f);*/

            //MOVED INTO WEAPONS -- this should only be happening when weapons are attacking, and each weapon already has dmg control
            /*foreach (Collider c in colliders)
            {
                
                //Debug.Log(c.gameObject.name);
                if (c.gameObject.name == "L_hand" || c.gameObject.name == "R_hand")
                {                    
                    if (isRecovering) 
                    {
                        _AIStatsController statsController = GetComponent<_AIStatsController>();
                        statsController.DetractHealth(50, true); // magic number needs to be changed
                        Debug.Log("Critical strike!");
                    }
                    
                }
                Debug.Log("Hit " + c.gameObject.name);
            }*/

            //Debug.Log("HP:" + stats.GetHealth());
        }
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
        if (phase == 2)
        {
            anim.SetTrigger("DisableArms");
            anim.SetBool("ArmsDisabled", true);
            swapMat.Swap();
        }
    }

    public void OnDeath()
    {
        dead = true;
        pointInner.gameObject.SetActive(false);
        pointOut.gameObject.SetActive(false);
        anim.SetTrigger("Die");
        StartCoroutine(ShutOffLights());
    }

    IEnumerator ShutOffLights()
    {
        yield return new WaitForSeconds(1);
        RenderSettings.ambientLight = new Color(0.15f, 0.1f, 0.1f);
        yield return new WaitForSeconds(1);
        RenderSettings.ambientLight = Color.black;
        yield return new WaitForSeconds(1);
        winscreen.SetActive(true);
        InvokeRepeating("WinOpaquer", 0f, 0.1f);
        yield return new WaitForSeconds(5);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
    }

    void WinOpaquer()
    {
        Image image = winscreen.GetComponent<Image>();
        var tempColor = image.color;
        if (tempColor.a > 1f)
        {
            CancelInvoke();
        }
        tempColor.a += .01f;
        image.color = tempColor;
    }
}
