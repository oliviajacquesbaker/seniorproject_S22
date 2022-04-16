using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOffOnCollect : MonoBehaviour
{
    public GameObject light;

    public void TurnOffLight()
    {
        light.SetActive(false);
    }

}
