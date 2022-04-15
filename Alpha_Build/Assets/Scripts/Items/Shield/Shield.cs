using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float durabilityDecayRate, opacityDecayRate, damageReduction;
    //public float isActive;
    public Durability durability;
    public _PlayerStatsController playerStatsController;
    public Color color;
    public Animator anim;
    public KeyCode shieldButton;
    bool currentlyRaised;

    void Start()
    {
        durability = GetComponent<Durability>();
        playerStatsController = GameObject.Find("Player").GetComponent<_PlayerStatsController>();
        color = GetComponent<Renderer>().material.color;
        anim = GameObject.Find("Player").GetComponent<Animator>();
        currentlyRaised = false;
    }

    void Update()
    {
        if (Input.GetKey(shieldButton))
        {
            RaiseShield();
            playerStatsController.dmgModifier = damageReduction;
        }

        if (Input.GetKeyUp(shieldButton))
        {
            LowerShield();
            playerStatsController.dmgModifier = 1.0f;
        }
        if (currentlyRaised && playerStatsController.GetPerceivedIntensity() > 0.15)
        {
            color.a -= Time.deltaTime * opacityDecayRate;
            durability.currDurability -= Time.deltaTime * durabilityDecayRate;
        }
    }

    public bool isActive()
    {
        return gameObject.activeInHierarchy;
    }

    private void RaiseShield()
    {
        currentlyRaised = true;
        anim.SetBool("ShieldUp", true);
    }

    private void LowerShield()
    {
        currentlyRaised = false;
        anim.SetBool("ShieldUp", false);
    }
}
