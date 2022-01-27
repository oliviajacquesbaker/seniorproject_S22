using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectScript : MonoBehaviour
{
    //Todo list: 
    //1) Increment the playerstats Slimes Found Variable CHECK MARK
    //2) Heal the Player OR overcharge the player CHECK MARK
    //3) Shrink the slime and disappear it (particle effects?) CHECK MARK


    //Player Objects
    private GameObject playerObj;
    private _PlayerStatsController playerController;

    //Slime Objects
    private GameObject mainSlime;
    private GameObject innerSlime;

    //Collider Field Bool
    private bool collected;

    //Function Bool
    private int repeatSendCounter;
    private int repeatShrinkCounter;
    

    // Start is called before the first frame update
    void Start()
    {
        playerObj = GameObject.FindWithTag("Player");
        playerController = playerObj.GetComponent<_PlayerStatsController>();
        collected = false;
        repeatSendCounter = 0;
        repeatShrinkCounter = 0;

        mainSlime = gameObject.transform.parent.gameObject;
        innerSlime = mainSlime.transform.Find("Inner Cloth Binder").gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!collected && other.gameObject.tag == "Player")
        {
            playerController.AddSlime();
            InvokeRepeating("SendSlimeHealth", 0f, .05f);
            InvokeRepeating("ShrinkSlime", 0f, .05f);
            collected = true;
        }
    }
    //Takes two seconds to fully do 50 health.
    private void SendSlimeHealth()
    {
        if (repeatSendCounter <= 39)
        {
            playerController.AddSlimeHealth();
            repeatSendCounter++;
        }
        else
        {
            CancelInvoke("SendSlimeHealth");
        }
    }
    //Takes two seconds to shrink and disappear the slime
    private void ShrinkSlime()
    {
        if (repeatShrinkCounter <= 39)
        {
            mainSlime.transform.localScale += new Vector3(-0.025f, -0.025f, -0.025f);
            repeatShrinkCounter++;
        }
        else
        {
            CancelInvoke("ShrinkSlime");
            Destroy(mainSlime);
        }
    }
}
