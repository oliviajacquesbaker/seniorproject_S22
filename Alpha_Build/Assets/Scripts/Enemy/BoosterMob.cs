using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterMob : MonoBehaviour
{
    [SerializeField]
    private Light boostedLight;
    [SerializeField]
    private float maxBoostAmount = 1f;
    [SerializeField]
    private Material deadMat;
    private Material[] materials;
    private float minBoostAmount;
    private float initIntensity;
    private float initRange;
    private float increment;
    private float downMod;
    private bool up = false, flare = false;
    private bool active = true;
    public Animator animator;

    void Start()
    {
        if(!animator) animator = gameObject.GetComponentInParent<Animator>();
        materials = gameObject.GetComponent<Renderer>().materials;
        initIntensity = boostedLight.intensity;
        initRange = boostedLight.range;
        minBoostAmount = initIntensity + (maxBoostAmount / 2.5f);
        increment = maxBoostAmount / (initIntensity * 700);
        downMod = -0.6f;
        BoostLight(maxBoostAmount);
    }

    private void Update()
    {
        if (active)
        {
            if (flare) increment *= 1.15f;
            if (up) BoostLight(increment);
            else BoostLight(downMod * increment);

            if (boostedLight.intensity > maxBoostAmount + initIntensity) up = false;
            else if (boostedLight.intensity < minBoostAmount)
            {
                up = true;
                if (flare) active = false;
            }
        }
    }

    private void BoostLight(float amount)
    {
        boostedLight.intensity += amount;
        boostedLight.range += amount / 2;
    }

    private void ResetLight()
    {
        boostedLight.intensity = initIntensity;
        boostedLight.range = initRange;
    }

    private void Flare()
    {
        flare = true;
        up = true;
        downMod = -2f;
        maxBoostAmount = 20f;
    }

    public void OnDeath()
    {
        Flare();
        animator.SetTrigger("Death");
        materials[1] = deadMat;
        gameObject.GetComponent<Renderer>().materials = materials;
    }
}
