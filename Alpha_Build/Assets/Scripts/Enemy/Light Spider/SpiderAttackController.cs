using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderAttackController : MonoBehaviour
{

    [SerializeField]
    private Animator _animator;

    [SerializeField]
    private Collider playerDetCol;

    [SerializeField]
    private SpiderController spiderCon;

    [SerializeField]
    _PlayerStatsController player;

    private bool attackBool = true;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<_PlayerStatsController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Melee")) {
            playerDetCol.enabled = false;
            spiderCon.SetDet();
            AnimateController();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("Melee"))
        {
            CancelInvoke();
            playerDetCol.enabled = true;
            _animator.SetBool("AttackRange", false);
            _animator.SetBool("Cooldown", true);
            spiderCon.SetDet();
        }
    }

    private void AnimateController()
    {
        _animator.SetBool("AttackRange", true);
        _animator.SetBool("Cooldown", false);
        InvokeRepeating("Cooldown", 1, 2);
    }

    private void Cooldown()
    {
        if (attackBool)//Place Damage player logic in this if statement.
        {
            _animator.SetBool("Cooldown", true);
            attackBool = false;
            CancelInvoke();
            InvokeRepeating("Cooldown", 3, 1);
            player.DetractHealth(10f, true);
        }
        else if (!attackBool)
        {
            _animator.SetBool("Cooldown", false);
            attackBool = true;
        }
    }
}
