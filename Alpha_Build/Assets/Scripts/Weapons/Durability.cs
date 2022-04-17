using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Durability : MonoBehaviour
{

    public float currDurability;
    public float maxDurability;
    public float decayRate;
    private Inventory inventory;

    void Start()
    {
        maxDurability = 100.0f;
        currDurability = maxDurability;
        inventory = GameObject.Find("Main Camera").GetComponent<Inventory>();
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
            DestroyItem();
        }

    }

    void DestroyItem()
    {
        if (gameObject.name == "Bow")
        {
            Debug.Log("Destroying bow");
            inventory.DisableItem(ShadowType.bow);
        }
        else if (gameObject.name == "Sword")
        {
            Debug.Log("Destroying sword");
            inventory.DisableItem(ShadowType.sword);
        }
        else if (gameObject.name == "Shield")
        {
            Debug.Log("Destroying shield");
            inventory.DisableItem(ShadowType.shield);
        }
        else if (gameObject.name == "RopeItem")
        {
            Debug.Log("Destroying rope");
            inventory.DisableItem(ShadowType.rope);
        }
    }
}
