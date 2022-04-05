using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _AIStats : MonoBehaviour
{
    [SerializeField]
    private float health = 100f;
    private float maxHealth;

    void Start()
    {
        maxHealth = health;
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void SetHealth(float diffHealth)
    {
        health = diffHealth;
    }    

    public void Heal(float addHealth)
    {
        health += addHealth;
    }

    public void Damage(float removeHealth)
    {
        health -= removeHealth;
    }
}