using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float maxHealth = 100f;

    private float superChargedHealth;
    private float maxSuperChargedHealth = 50f;

    private float lerpTimer;

    private float slerpTimer;

    private float chipSpeed = 2f;

    public Image healthBarNormal;
    public Image healthBarChaser;

    public Image chargeBarNormal;
    public Image chargeBarChaser;

    _PlayerStats player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<_PlayerStats>();
        health = maxHealth;
        superChargedHealth = 0;
    }

    // Update is called once per frame
    void Update()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
        superChargedHealth = Mathf.Clamp(superChargedHealth, 0, maxSuperChargedHealth);
        UpdateHealthUI();
        HealthController();
        UpdateSuperChargeUI();
    }

    public void HealthController()
    {

        if (player.GetHealth() <= 100f) //Effects healing and damge on the health bar
        {
            if (player.GetHealth() > health)
            {
                Heal(player.GetHealth() - health);
            }
            else if (player.GetHealth() < health)
            {
                Damage(health - player.GetHealth());
            }
        }
        else if (player.GetHealth() > 100f)//Effects healing and damage on the super charge bar
        {
            if((player.GetHealth() - 100f) > superChargedHealth)
            {
                SuperCharge((player.GetHealth() - 100f) - superChargedHealth);
            }
            else if ((player.GetHealth() - 100f) < superChargedHealth)
            {
                SuperDamage(superChargedHealth - (player.GetHealth() - 100f));
            }
        }
    }

    public void UpdateHealthUI() //Produces chip away effect
    {
        float fillN = healthBarNormal.fillAmount;
        float fillC = healthBarChaser.fillAmount;
        float hFraction = health / maxHealth;

        if(fillC > hFraction)
        {
            healthBarNormal.fillAmount = hFraction;
            healthBarChaser.color = Color.white;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            healthBarChaser.fillAmount = Mathf.Lerp(fillC, hFraction, percentComplete);
        }
        else if(fillN < hFraction)
        {
            healthBarChaser.color = Color.black;
            healthBarChaser.fillAmount = hFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            healthBarNormal.fillAmount = Mathf.Lerp(fillN,healthBarChaser.fillAmount, percentComplete);
        }
    }

    public void UpdateSuperChargeUI() //Produces chip away effect
    {
        float fillN = chargeBarNormal.fillAmount;
        float fillC = chargeBarChaser.fillAmount;
        float sFraction = superChargedHealth / maxSuperChargedHealth;

        if (fillC > sFraction)
        {
            chargeBarNormal.fillAmount = sFraction;
            chargeBarChaser.color = Color.white;
            slerpTimer += Time.deltaTime;
            float percentComplete = slerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            chargeBarChaser.fillAmount = Mathf.Lerp(fillC, sFraction, percentComplete);
        }
        else if (fillN < sFraction)
        {
            chargeBarChaser.color = Color.black;
            chargeBarChaser.fillAmount = sFraction;
            slerpTimer += Time.deltaTime;
            float percentComplete = slerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            chargeBarNormal.fillAmount = Mathf.Lerp(fillN, chargeBarChaser.fillAmount, percentComplete);
        }
    }

    public void Damage(float damageAmount)
    {
        health -= damageAmount;

        lerpTimer = 0f;
    }

    public void Heal(float healAmount)
    {
        health += healAmount;

        lerpTimer = 0f;
    }

    public void SuperCharge(float chargeAmount)
    {
        superChargedHealth += chargeAmount;

        slerpTimer = 0f;
    }

    public void SuperDamage(float damageAmount)
    {
        superChargedHealth -= damageAmount;

        slerpTimer = 0f;
    }
}
