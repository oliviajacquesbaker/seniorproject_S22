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
    Vector3 verticalVel;


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
            if (Input.GetKeyDown(KeyCode.F))
            {
                ToggleClimb(); // grabs rope
            }

            ClimbRope();
        }

        // if (isClimbing)
        // {
        //     if (Input.Get)
        // }
    }

    bool IsNextToRope()
    {
        Collider[] hit = Physics.OverlapSphere(player.transform.position, checkRadius, ropeLayers);

        foreach (Collider rope in hit)
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
        controller.gravityEnabled = !controller.gravityEnabled;
        //        player.GetComponent<Rigidbody>().useGravity = !player.GetComponent<Rigidbody>().useGravity;
    }

    void ClimbRope()
    {
        //Vector3 moveDir = new Vector3(0f, 0f, 0f);
        CharacterController playerController = controller.GetComponent<CharacterController>();
        if (isClimbing)
        {
            bool triggered = false;
            if (Input.GetKey(KeyCode.W))
            {
                verticalVel.y += 1.0f;
                playerController.Move(verticalVel * Time.deltaTime * 0.05f);
                //player.transform.Translate(Vector3.up * Time.deltaTime * climbRate, Space.World); // move up
                blobAnim.SetBool("Climbing", true);
                triggered = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                verticalVel.y -= 1.0f;
                playerController.Move(verticalVel * -1 * Time.deltaTime * 0.05f);
                //player.transform.Translate(Vector3.down * Time.deltaTime * climbRate, Space.World); // move down
                blobAnim.SetBool("Climbing", true);
                triggered = true;
            }

            if (!triggered) blobAnim.SetBool("Climbing", false);
        }
    }

}
