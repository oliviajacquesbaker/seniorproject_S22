using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float health;

    void Start()
    {
        health = 100f;
    }

    public void SetHealth(float diffHealth)
    {
        health = diffHealth;
    }

    public float GetHealth()
    {
        return health;
    }
}
