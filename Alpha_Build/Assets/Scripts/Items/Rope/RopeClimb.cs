using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeClimb : MonoBehaviour
{

    public GameObject player;
    public float climbRate, checkRadius;
    public bool isClimbing;
    private ThirdPersonMovement controller;
    public LayerMask ropeLayers;
    [SerializeField]
    StateHandler playerStateHandler;
    [SerializeField]
    GameObject playerBlob;
    public Animator blobAnim;


    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<ThirdPersonMovement>();
        blobAnim = playerBlob.GetComponent<Animator>();
        isClimbing = false;
    }

    void Update()
    {

        if (IsNextToRope())
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                ToggleClimb(); // grabs rope
            }

            ClimbRope();
        }
    }

    bool IsNextToRope()
    {
        Collider[] hit = Physics.OverlapSphere(player.transform.position, checkRadius, ropeLayers);

        foreach(Collider rope in hit)
        {
            if (rope.GetComponent<CharacterJoint>())
            {
                //Debug.Log("NEXT TO ROPE");
                return true;
            }
        }

        return false;
    }

    void ToggleClimb()
    {
        if (!isClimbing) playerStateHandler.TransitionToBlob();
        else playerStateHandler.InitialHumanoidTransition();

        isClimbing = !isClimbing;
        controller.enabled = !controller.enabled;
        player.GetComponent<Rigidbody>().useGravity = !player.GetComponent<Rigidbody>().useGravity;
    }

    void ClimbRope()
    {
        if (isClimbing)
        {
            bool triggered = false;
            if (Input.GetKey(KeyCode.W))
            {
                player.transform.Translate(Vector3.up * Time.deltaTime * climbRate, Space.World); // move up
                blobAnim.SetBool("Climbing", true);
                triggered = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                player.transform.Translate(Vector3.down * Time.deltaTime * climbRate, Space.World); // move down
                blobAnim.SetBool("Climbing", true);
                triggered = true;
            }

            if(!triggered) blobAnim.SetBool("Climbing", false);
        }
    }

}
