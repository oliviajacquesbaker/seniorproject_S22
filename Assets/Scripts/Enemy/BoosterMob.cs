using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterMob : MonoBehaviour
{
    [SerializeField]
    float boostAmount = 2f;
    [SerializeField]
    Light boostedLight;

    private void Start()
    {
        BoostLight();
    }

    private void BoostLight()
    {
        boostedLight.intensity += boostAmount;
    }

    private void RemoveBoost()
    {
        boostedLight.intensity -= boostAmount;
    }

    public void OnDeath()
    {
        RemoveBoost();
    }
}
