using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _AIStatsController : MonoBehaviour
{
    [SerializeField]
    public _AIStats currAi;
    [SerializeField]
    private MobType type = MobType.Patrol;
    [SerializeField]
    float lightIntensity = 0;
    [SerializeField]
    bool debug, log = false;
    [SerializeField]
    Image healthbar;

    private bool multistage = false;
    private bool living = true;

    void Start()
    {
        currAi = GetComponent<_AIStats>();
    }

    void Update()
    {
        if (living && currAi.GetHealth() <= 0 && !multistage)
        {
            Debug.Log(currAi.GetHealth());
            Kill();
        }
        if (healthbar)
        {
            healthbar.transform.parent.transform.LookAt(new Vector3(Camera.main.transform.position.x, healthbar.transform.parent.transform.position.y, Camera.main.transform.position.z));
        }
    }

    public bool GetMultiStage()
    {
        return multistage;
    }

    public void SetMultiStage(bool setMulti)
    {
        multistage = setMulti;
    }

    //This controller function deals with damage or healing in regards to light intensity. NOT normal attack damage/healing
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
            if (lightIntensity < 0.15 && type != MobType.Booster) //Damages AI in Dark
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
        UpdateHealthbar();
    }

    public void DetractHealth(float damage, bool hit) // for hit purposes
    {
        currAi.SetHealth(currAi.GetHealth() - damage);
        UpdateHealthbar();
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

        UpdateHealthbar();
    }

    public void Kill()
    {
        living = false;
        if(healthbar) DestroyHealthbar();
        switch (type)
        {
            case MobType.Booster:
                gameObject.GetComponent<BoosterMob>().OnDeath();
                break;
            case MobType.Patrol:
                gameObject.GetComponent<Patrol_Movement>().OnDeath();
                break;
            case MobType.Boss:
                gameObject.GetComponent<Boss>().OnDeath();
                break;
            default:
                //Debug.Log("set up similar functions in the other mob types :)");
                Destroy(gameObject);
                break;
        }

    }

    public void UpdateHealthbar()
    {
        healthbar.fillAmount = Mathf.Clamp(currAi.GetHealth() / currAi.GetMaxHealth(), 0, 1f);
        if(healthbar.fillAmount <= 0)
        {
            DestroyHealthbar();
        }
    }

    public void DestroyHealthbar()
    {
        Destroy(healthbar.transform.parent.gameObject);
    }
}