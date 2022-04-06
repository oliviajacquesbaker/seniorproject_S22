using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShatter : MonoBehaviour
{
    public Material original, shattered;
    public GameObject light;
    public bool debug;
    private Renderer renderer;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        original = renderer.material;
    }
    void Update()
    {
        if (debug)
        {
            TurnOffLight();
        }
        else
        {
            TurnOnLight();
        }
    }
    void OnTriggerEnter()
    {
        TurnOffLight();
    }

    public void TurnOffLight()
    {
        // add light shatter SFX
        renderer.material = shattered;    
        light.SetActive(false);
    }

    public void TurnOnLight()
    {
        renderer.material = original;    
        light.SetActive(true);
    }
}
