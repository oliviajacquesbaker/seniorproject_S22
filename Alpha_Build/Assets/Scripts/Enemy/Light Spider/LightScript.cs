using UnityEngine;
using System;
using System.Collections;

public class LightScript : MonoBehaviour
{
    public GameObject player;

    [SerializeField]
    private _AIStatsController spiderController;

    private void Start() //Initializes the player game object to the player's game object
    {
        player = GameObject.FindGameObjectWithTag("Player");

        spiderController.SetMultiStage(true);
    }
    private void LateUpdate() //Spider butt looks at player!
    {
        transform.LookAt(player.transform);
    }
}