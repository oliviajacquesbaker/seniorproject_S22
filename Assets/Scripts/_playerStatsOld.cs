using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _ffplayerStats : MonoBehaviour
{
    private float playerHealth;


    void Start()
    {
        playerHealth = 100f;
    }

    void Update()
    {
       if(playerHealth > 100)
        {
            playerHealth = 100;
        }
    }

    [SerializeField]
    private float lightIntensity = 1f;

    public void DetractHealthLight()
    {
        playerHealth = playerHealth - 1f * Time.deltaTime * lightIntensity;
    }

    public void DetractHealthLight(float perceivedIntensity)
    {
        playerHealth = playerHealth - 1f * Time.deltaTime * perceivedIntensity;
        Debug.Log(playerHealth);
    }

    public void AddHealthShadow(float perceivedIntensity)
    {
        if(perceivedIntensity > 0)
        {
            playerHealth = playerHealth + 1f * Time.deltaTime * ((1/perceivedIntensity) * .1f);
        }
        else
        {
            playerHealth = playerHealth + 1f * Time.deltaTime;
        }
        

        Debug.Log(playerHealth);
    }
}
