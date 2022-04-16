using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BendPipe : MonoBehaviour
{

    [SerializeField]
    private GameObject bentPipe;

    public void Bend()
    {
        gameObject.SetActive(false);
        bentPipe.SetActive(true);
    }
}
