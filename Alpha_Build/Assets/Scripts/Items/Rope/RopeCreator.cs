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
    public float turnSmoothTime, turnSmoothVelocity, ropeRange;
    public ThirdPersonMovement controller;
    private StateHandler state;
    public GameObject crosshair, aimPoint;
    private Color original;
    public Color highlighted;
    public bool isAimingHook = false;
    //private float camSens = 1.0f;

    void Start()
    {
        rope = ropeObj.GetComponent<Rope>();
        player = GameObject.Find("Player");
        //cam = player.GetComponent<CameraController>();
        controller = player.GetComponent<ThirdPersonMovement>();
        state = GameObject.Find("Main Camera").GetComponent<StateHandler>();
        //        original = crosshair.GetComponent<Image>().color;
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

        if (Physics.Raycast(aimPoint.transform.position, aimPoint.transform.TransformDirection(Vector3.forward) * ropeRange, out hit, ropeRange))
        {
            //Debug.Log("Hitting" + hit.transform.gameObject.name);
            var selection = hit.transform;
            if (selection.gameObject.tag == "Hook")
            {
                isAimingHook = true;
                rope.snapFirst = true;
                //ShowCrosshairIndicator();
                if (Input.GetKeyUp(keycode))
                {
                    rope.parentObj = selection.transform.gameObject;
                    rope.Spawn();
                }
            }
            else
            {
                isAimingHook = false;
            }
        }

        //Debug.DrawLine(GameObject.Find("Aim Camera").transform.position, hit.point);
        //HideCrosshairIndicator();
        Debug.DrawRay(aimPoint.transform.position, aimPoint.transform.TransformDirection(Vector3.forward) * ropeRange, Color.green);
    }

    void ShowCrosshairIndicator()
    {
        crosshair.GetComponentInChildren<Image>().color = highlighted;
        // crosshair.GetComponent<Image>().color = highlighted;
        // left.GetComponent<Image>().color = highlighted;
        // right.GetComponent<Image>().color = highlighted;
    }

    void HideCrosshairIndicator()
    {
        crosshair.GetComponentInChildren<Image>().color = original;
        // crosshair.GetComponent<Image>().color = original;
        // left.GetComponent<Image>().color = original;
        // right.GetComponent<Image>().color = original;
    }

    void AimRope()
    {
        cam.Aim();
        //player.transform.Rotate(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), 0.0f, Space.Self);
    }

    void StopAimingRope()
    {
        cam.StopAim();
        //float prevY = player.transform.rotation.y * 180;
        //player.transform.rotation = Quaternion.Euler(0f, prevY, 0f);
    }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawRay(GameObject.Find("Main Camera").transform.position, GameObject.Find("Main Camera").transform.TransformDirection(Vector3.forward) * ropeRange);
    // }
}

