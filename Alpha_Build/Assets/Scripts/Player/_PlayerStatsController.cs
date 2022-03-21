using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerStatsController : MonoBehaviour
{
    [SerializeField]
    public _PlayerStats player;
    [SerializeField]
    public Shield shield;
    [SerializeField]
    float lightIntensity = 0;
    [SerializeField]
    bool debug, log, healDark, isImmune = false;
    [SerializeField]
    float currentIntensity = 0;

    void Start()
    {
        player = GetComponent<_PlayerStats>();
        //shield = shield.GetComponent<Shield>();
    }
    //Updates Health in regard to light intensity
    public void UpdateHealth(float perceivedIntensity)
    {
        currentIntensity = perceivedIntensity;
        if (!debug)
        {
            if (perceivedIntensity < 0.15) //Heals Player in Dark
            {
                AddHealth(perceivedIntensity);
            }
            else //Damages Player in Light
            {
                if (!isImmune)
                {
                    DetractHealth(perceivedIntensity);
                }
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
            if (player.GetHealth() - 1f * Time.deltaTime * lightIntensity < 0)
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
        if (healDark)
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

    //If the player is physically hit?
    public void DetractHealth(float damage, bool hit)
    {
        player.SetHealth(player.GetHealth() - damage);
    }

    public void AddSlime()
    {
        player.SetSlimes(player.GetSlimes() + 1);
    }

    public void AddSlimeHealth()
    {
        player.SetHealth(player.GetHealth() + 1.25f);
    }

    public float GetPerceivedIntensity()
    {
        if (debug)
            return lightIntensity;
        else
            return currentIntensity;
    }

    public void SetPlayerImmune(bool immune)
    {
        isImmune = immune;
    }

}
