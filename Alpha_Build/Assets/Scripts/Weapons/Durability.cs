using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durability : MonoBehaviour
{

    public float currDurability;
    public float maxDurability;
    public float decayRate;

    void Start()
    {
        maxDurability = 100.0f;
        currDurability = maxDurability;
    }

    void Update()
    {
        currDurability = Mathf.Clamp(currDurability, 0f, 100f);
        DecayDurability();
        //Debug.Log(currDurability);
    }

    public float GetCurrDurability()
    {
        return currDurability;
    }

    public float GetMaxDurability()
    {
        return maxDurability;
    }

    void DecayDurability()
    {
        currDurability -= Time.deltaTime * decayRate;
        
        if (currDurability <= 0)
        {
            gameObject.SetActive(false);
            currDurability = maxDurability;
        }

    }
}
