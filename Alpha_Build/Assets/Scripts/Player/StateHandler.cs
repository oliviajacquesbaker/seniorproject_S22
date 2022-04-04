using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateHandler : MonoBehaviour
{
    private GameObject bow;
    //private GameObject inv;
    private GameObject sword;
    [SerializeField]
    GameObject playerMain;
    private GameObject playerBlob;
    [SerializeField]
    GameObject middleman;
    [SerializeField]
    GameObject rope;
    private Inventory inv;
    public Animator anim;
    public Animator blobAnim;
    public Animator middleAnim;

    void Start()
    {
        bow = GameObject.Find("Bow");
        playerBlob = GameObject.Find("player_BLOB");
        //playerMain = GameObject.Find("shadowplayer");
        //middleman = GameObject.Find("BLOBTRANSITION");
        playerBlob.SetActive(false);
        middleman.SetActive(false);
        blobAnim = playerBlob.GetComponent<Animator>();
        middleAnim = middleman.GetComponent<Animator>();
        anim = GameObject.Find("Player").GetComponent<Animator>();
        inv = GameObject.Find("Main Camera").GetComponent<Inventory>();
        sword = GameObject.Find("Sword");
    }

    public bool InvOpen()
    {
        return GameObject.Find("Inventory");
    }

    public bool BowIsAiming()
    {
        return bow.GetComponent<Bow>().isAiming;
    }

    //called by rope climb script when player tries to climb
    public void TransitionToBlob()
    {
        Debug.Log("TRANSITION TO BLOB");
        anim.SetBool("IsBlob", true);
        playerMain.SetActive(false);
        middleman.SetActive(true);
        middleAnim.SetBool("Blobify", true);
        rope.SetActive(false);
    }

    //called by middleman blobify
    public void TurnToBlob()
    {
        Debug.Log("TURN TO BLOB");
        middleAnim.SetBool("Blobify", false);
        middleman.SetActive(false);
        playerBlob.SetActive(true);
        blobAnim.SetBool("Forming", true);
    }

    public void InitialHumanoidTransition()
    {
        Debug.Log("INITIALIZE HUMAN");
        blobAnim.SetBool("Forming", false);
        blobAnim.SetBool("Climbing", false);
        blobAnim.SetBool("Unforming", true);
    }

    //called by blob's unblobify after disengaging from rope and playing appropriate unblobify anim
    public void TransitionToHumanoid()
    {
        Debug.Log("TRANSITION TO HUMAN");
        blobAnim.SetBool("Unforming", false);
        playerBlob.SetActive(false);
        middleman.SetActive(true);
        middleAnim.SetBool("Unblobify", true);
        middleAnim.SetBool("Blobify", false);
    }

    //called by middleman unblobify
    public void TurnHumanoid()
    {
        Debug.Log("TURN TO HUMAN");
        middleAnim.SetBool("Unblobify", false);
        middleman.SetActive(false);
        playerMain.SetActive(true);
        anim.SetBool("IsBlob", false);
        rope.SetActive(true);

    }

}
