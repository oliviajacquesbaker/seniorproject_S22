using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerStats : MonoBehaviour
{
    [SerializeField]
    private float health = 100;
    [SerializeField]
    private int slimesCollected = 0;
    [SerializeField]
    private string activeTool = "none";

    void Start()
    {
        health = 100f;
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0f, 150f);
    }

    public void SetHealth(float diffHealth)
    {
        if (diffHealth <= 150)
        {
            health = diffHealth;
        }
        else
        {
            health = 150;
        }
    }

    public float GetHealth()
    {
        return health;
    }

    public int GetSlimes()
    {
        return slimesCollected;
    }

    public void SetSlimes(int slimes)
    {
        slimesCollected = slimes;
    }

    public void SetActiveTool(string toolName)
    {
        activeTool = toolName;
    }

    public string GetActiveTool()
    {
        return activeTool;
    }
}
