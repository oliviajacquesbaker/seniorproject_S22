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


    void Start()
    {
        player = GameObject.Find("Player");
        controller = player.GetComponent<ThirdPersonMovement>();
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
                return true;
            }
        }

        return false;
    }

    void ToggleClimb()
    {
        isClimbing = !isClimbing;
        controller.enabled = !controller.enabled;
        player.GetComponent<Rigidbody>().useGravity = !player.GetComponent<Rigidbody>().useGravity;
    }

    void ClimbRope()
    {
        if (isClimbing)
        {
            if (Input.GetKey(KeyCode.W))
            {
                player.transform.Translate(Vector3.up * Time.deltaTime * climbRate, Space.World); // move up
            }

        if (Input.GetKey(KeyCode.S))
            {
                player.transform.Translate(Vector3.down * Time.deltaTime * climbRate, Space.World); // move down
            }
        }
    }

}
