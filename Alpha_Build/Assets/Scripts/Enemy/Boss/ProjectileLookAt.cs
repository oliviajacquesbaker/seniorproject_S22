using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLookAt : MonoBehaviour
{

    [SerializeField]
    private GameObject player;

    void Update()
    {
        transform.LookAt(player.transform);
    }

}
