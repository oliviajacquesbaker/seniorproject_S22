using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShadowIndicator : MonoBehaviour
{
    [SerializeField]
    private Image SlimeIndicator;

    [SerializeField]
    private Sprite SlimeIndicatorOne, SlimeIndicatorTwo, SlimeIndicatorThree;

    private int faceSwitch = 1;

    private bool forwards;


    private _PlayerStatsController player;
    
    private float perceivedIntensity;


    private void Start()
    {
        InvokeRepeating("FaceSwitcher", 0f, .8f);

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<_PlayerStatsController>();
    }

    private void Update()
    {
        FaceOpacity();
    }

    private void FaceOpacity()
    {
        perceivedIntensity = Mathf.Clamp(perceivedIntensity, 0f, 1f);

        perceivedIntensity = 1 - player.GetPerceivedIntensity();

        SlimeIndicator.color = new Color(SlimeIndicator.color.r, SlimeIndicator.color.g, SlimeIndicator.color.b, perceivedIntensity);
    }

    private void FaceSwitcher()
    {
        switch (faceSwitch)
        {
            case 1:
                forwards = true;
                SlimeIndicator.sprite = SlimeIndicatorOne;
                faceSwitch++;
                break;
            case 2:
                SlimeIndicator.sprite = SlimeIndicatorTwo;
                if (forwards)
                    faceSwitch++;
                else
                    faceSwitch--;
                break;
            case 3:
                forwards = false;
                SlimeIndicator.sprite = SlimeIndicatorThree;
                faceSwitch--;
                break;
        }
    }
}
