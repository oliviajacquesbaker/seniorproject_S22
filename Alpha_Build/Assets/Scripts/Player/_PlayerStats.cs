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
    public Animator anim;
    bool alive;

    void Start()
    {
        alive = true;
        health = 100f;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        health = Mathf.Clamp(health, 0f, 150f);
        if (health <= 0 && alive) Die();
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
        health = diffHealth;
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

    public void Die()
    {
        //anim.ApplyBuiltinRootMotion();
        anim.SetBool("Dead", true);
        anim.SetTrigger("Die");
        GetComponent<ThirdPersonMovement>().enabled = false;
        alive = false;
    }
}
