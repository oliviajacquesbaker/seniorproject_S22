using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolDurability : MonoBehaviour
{

    private _PlayerStats playerStats;
    private Durability durability;

    float durrFraction = 1f;

    private float lerpTimer;

    private float chipSpeed = 5f;

    [SerializeField]
    Image toolDurabilityFiller, toolDurabilityChaser, toolIconFill;

    [SerializeField]
    Sprite plain, bow, sword, rope;

    private bool activeTool;

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<_PlayerStats>();
        InvokeRepeating("ResetLerp", 0f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (playerStats.GetActiveTool())
        {
            case "bow":
                durability = GameObject.Find("Bow").GetComponent<Durability>();
                toolIconFill.sprite = bow;
                activeTool = true;
                break;
            case "sword":
                durability = GameObject.Find("Sword").GetComponent<Durability>();
                toolIconFill.sprite = sword;
                activeTool = true;
                break;
            case "none":
                activeTool = false;
                toolIconFill.sprite = plain;
                break;
            case "rope":
                durability = GameObject.Find("RopeItem").GetComponent<Durability>();
                activeTool = true;
                toolIconFill.sprite = rope;
                break;
            default:
                activeTool = false;
                toolIconFill.sprite = plain;
                break;
        }
        durrFraction = Mathf.Clamp(durrFraction, 0f, 1f);
        if (activeTool)
        {
            UpdateToolDurabilityUI();
        }
    }

    private void UpdateToolDurabilityUI()
    {
        float fillN = toolDurabilityFiller.fillAmount;
        float fillC = toolDurabilityChaser.fillAmount;
        float durrFraction = durability.GetCurrDurability() / durability.GetMaxDurability();

        if (fillC > durrFraction)
        {
            toolDurabilityFiller.fillAmount = durrFraction;
            toolDurabilityChaser.color = Color.white;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            toolDurabilityChaser.fillAmount = Mathf.Lerp(fillC, durrFraction, percentComplete);
        }
        else if (fillN < durrFraction)
        {
            toolDurabilityChaser.color = Color.black;
            toolDurabilityChaser.fillAmount = durrFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            toolDurabilityFiller.fillAmount = Mathf.Lerp(fillN, toolDurabilityChaser.fillAmount, percentComplete);
        }
    }

    private void ResetLerp()
    {
        lerpTimer = 0f;
    }
}
