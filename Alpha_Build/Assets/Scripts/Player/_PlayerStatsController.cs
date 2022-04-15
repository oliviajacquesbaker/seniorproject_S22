using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class _PlayerStatsController : MonoBehaviour
{
    [SerializeField]
    public _PlayerStats player;
    [SerializeField]
    float lightIntensity = 0;
    [SerializeField]
    bool debug, log, healDark = false;
    [SerializeField]
    float currentIntensity = 0;

    private float timeSinceDeath = 0f;

    [SerializeField]
    GameObject DeathPanel, LivingGroup;
    bool hudSwitch = false;

    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip death, lightBurn;

    bool inLight = false;

    public float dmgModifier = 1.0f;

    void Start()
    {
        player = GetComponent<_PlayerStats>();
    }

    void Update()
    {
        if (!player.GetAlive())
        {
            if (!hudSwitch)
            {
                Dead();
                hudSwitch = true;
                InvokeRepeating("HudOpaquer", 0f, 0.1f);
            }
            timeSinceDeath += UnityEngine.Time.deltaTime;
            if (timeSinceDeath > 7)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void PhaseBurnSound()
    {
        if (source.volume > 0)
        {
            source.volume -= 0.01f;
        }
        else
        {
            source.Stop();
            source.volume = 1f;
            CancelInvoke("PhaseBurnSound");
        }
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
                if (inLight)
                {
                    inLight = false;
                    if (player.GetAlive())
                    {
                        InvokeRepeating("PhaseBurnSound", 0f, 0.05f);
                    }
                }
            }
            else //Damages Player in Light
            {
                if (!inLight)
                {
                    source.clip = lightBurn;
                    source.volume = 0.1f;
                    source.Play();
                }
                inLight = true;
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
        Debug.Log("LOSING " + damage + " HEALTH");
        player.SetHealth(player.GetHealth() - (damage * dmgModifier));
        //if (player.GetHealth() <= 0) player.Die();
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

    void Dead()
    {
        LivingGroup.SetActive(false);
        source.clip = death;
        source.Play();
        DeathPanel.SetActive(true);
    }
    void HudOpaquer()
    {
        Image image = DeathPanel.GetComponent<Image>();
        var tempColor = image.color;
        if (tempColor.a > 1f)
        {
            CancelInvoke();
        }
        tempColor.a += .01f;
        image.color = tempColor;
    }
}
