using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _AIStatsController : MonoBehaviour
{
    [SerializeField]
    public _AIStats currAi;
    [SerializeField]
    float lightIntensity = 0;
    [SerializeField]
    bool debug, log = false;

    void Start()
    {
        currAi = GetComponent<_AIStats>();
    }

    public void UpdateHealth(float perceivedIntensity)
    {
        if (!debug)
        {
            if (perceivedIntensity < 0.15) //Damages AI in Dark
            {
                DetractHealth(perceivedIntensity);
            }
            else //Heals AI in Light
            {
                AddHealth(perceivedIntensity);
            }
        }
        else
        {
            if (lightIntensity < 0.15) //Damages AI in Dark
            {
                DetractHealth(lightIntensity);
            }
            else //Heals AI in Light
            {
                AddHealth(lightIntensity);
            }
        }
        if (log)
        {
            Debug.Log(currAi.GetHealth());
        }
    }

    //NOTE** DetractHealth may be unintended behaviour that may eventually be turned off.
    public void DetractHealth(float perceivedIntensity) //Damages in shadow
    {
        lightIntensity = perceivedIntensity;

        if (lightIntensity > 0)
        {
            currAi.SetHealth((currAi.GetHealth() - 1f * Time.deltaTime * lightIntensity));
        }
        else
        {
            currAi.SetHealth((currAi.GetHealth() - 1f * Time.deltaTime * 2f));
        }
    }

    public void AddHealth(float perceivedIntensity) //Heals in light
    {
        lightIntensity = perceivedIntensity;

        if ((currAi.GetHealth() + 1f * Time.deltaTime * ((1 / lightIntensity) * .1f)) < 100f)
        {
            if (lightIntensity > 0)
            {
                currAi.SetHealth(currAi.GetHealth() + 1f * Time.deltaTime * lightIntensity * .1f);
            }
            else
            {
                currAi.SetHealth(currAi.GetHealth() + 1f * Time.deltaTime * 2f);
            }
        }
        else
        {
            currAi.SetHealth(100f);
        }
    }

}