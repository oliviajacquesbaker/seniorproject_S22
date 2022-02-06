using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreatheAnimate : MonoBehaviour
{
    private bool useGrav;
    private Cloth JellySlime;

    void Start()
    {
        InvokeRepeating("BreatheFunction", 1.0f, 1.0f);

        JellySlime = GetComponent<Cloth>();

        useGrav = false;
    }

    private void BreatheFunction()
    {
        JellySlime.useGravity = useGrav;

        useGrav = !useGrav;
    }
}
