using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RopeCreator : MonoBehaviour
{
    public CameraController cam;
    public GameObject ropeObj, player;
    public Transform camTransf;
    public Rope rope;
    public KeyCode keycode;
    public float ropeRange, ropeRayRadius;
    public ThirdPersonMovement controller;
    private StateHandler state;
    public GameObject crosshair, aimPoint;
    public LayerMask layerMask;
    public bool isAimingHook = false, isAimingRope = false;
    private float currentHitDistance;

    void Start()
    {
        rope = ropeObj.GetComponent<Rope>();
        player = GameObject.Find("Player");
        controller = player.GetComponent<ThirdPersonMovement>();
        state = GameObject.Find("Main Camera").GetComponent<StateHandler>();
    }

    void Update()
    {
        if (GameObject.Find("Inventory")) { return; }

        CastRopeRaycast();

        if (Input.GetKey(keycode))
        {
            AimRope();
        }

        if (Input.GetKeyUp(keycode))
        {
            StopAimingRope();
        }
    }

    void CastRopeRaycast()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.SphereCast(aimPoint.transform.position, ropeRayRadius, aimPoint.transform.forward, out hit, ropeRange, layerMask))
        {
            //Debug.Log("Hitting" + hit.transform.gameObject.name);
            currentHitDistance = hit.distance;
            var selection = hit.transform;
            if (selection.gameObject.tag == "Hook")
            {
                if (isAimingRope) isAimingHook = true;
                rope.snapFirst = true;
                if (Input.GetKeyUp(keycode))
                {
                    rope.parentObj = selection.transform.gameObject;
                    rope.Spawn();
                }
            }
        }
        else
        {
            currentHitDistance = ropeRange;
            isAimingHook = false;
            isAimingRope = false;
        }
    }

    void AimRope()
    {
        cam.Aim();
        isAimingRope = true;
    }

    void StopAimingRope()
    {
        cam.StopAim();
        isAimingRope = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(aimPoint.transform.position, aimPoint.transform.position + aimPoint.transform.forward * currentHitDistance);
        Gizmos.DrawWireSphere(aimPoint.transform.position + aimPoint.transform.forward * currentHitDistance, ropeRayRadius);
    }
}

