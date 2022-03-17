using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float durabilityDecayRate, opacityDecayRate;
    //public float isActive;
    public Durability durability;
    public _PlayerStatsController playerStatsController;
    public Color color;
    void Start()
    {
        durability = GetComponent<Durability>();
        playerStatsController = GameObject.Find("Player").GetComponent<_PlayerStatsController>();
        color = GetComponent<Renderer>().material.color;
    }

    void Update()
    {
        //Debug.Log(color.a);

        if (playerStatsController.GetPerceivedIntensity() > 0.15)
        {
            color.a -= Time.deltaTime * opacityDecayRate;
            durability.currDurability -= Time.deltaTime * durabilityDecayRate;
        }
    }

    public bool isActive()
    {
        return gameObject.activeInHierarchy;
    }
}
