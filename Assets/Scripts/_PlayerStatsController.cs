using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerStatsController : MonoBehaviour
{
    [SerializeField]
    public _PlayerStats player;
    [SerializeField]
    float lightIntensity = 0;
    [SerializeField]
    bool debug, log = false;

    void Start()
    {
        player = GetComponent<_PlayerStats>();
    }

    public void UpdateHealth(float perceivedIntensity)
    {
        if (!debug)
        {
            if (perceivedIntensity < 0.15) //Heals Player in Dark
            {
                AddHealth(perceivedIntensity);
            }
            else //Damages Player in Light
            {
                DetractHealth(perceivedIntensity);
            }
        }
        else
        {
            if (lightIntensity < 0.15) //Heals Player in Dark
            {
                AddHealth(lightIntensity);
            }
            else //Damages Player in Light
            {
                DetractHealth(lightIntensity);
            }
        }
        if (log)
        {
            Debug.Log(player.GetHealth());
        }
    }

    //NOTE** DetractHealth may be unintended behaviour that may eventually be turned off.
    public void DetractHealth(float perceivedIntensity) //Damages in shadow
    {
        lightIntensity = perceivedIntensity;

        if (lightIntensity > 0)
        {
            if(player.GetHealth() - 1f * Time.deltaTime * lightIntensity < 0)
            {
                player.SetHealth(0);
            }
            else
            {
                player.SetHealth((player.GetHealth() - 1f * Time.deltaTime * lightIntensity));
            }
            
        }
        else
        {
            if (player.GetHealth() - 1f * Time.deltaTime * 2f < 0)
            {
                player.SetHealth(0);
            }
            else
            {
                player.SetHealth((player.GetHealth() - 1f * Time.deltaTime * 2f));
            }
        }
    }

    public void AddHealth(float perceivedIntensity) //Heals in light
    {
        lightIntensity = perceivedIntensity;

        if (lightIntensity > 0)
        {
            if ((player.GetHealth() + 1f * Time.deltaTime * ((1 / lightIntensity) * .1f)) < 100f)
            {
                player.SetHealth(player.GetHealth() + 1f * Time.deltaTime * ((1 / perceivedIntensity) * .0001f));
            }
        }
        else
        {
            if ((player.GetHealth() < 100f))
            {
                player.SetHealth(player.GetHealth() + 1f * Time.deltaTime);
            }
        }
        }
}
