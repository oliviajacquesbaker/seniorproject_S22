using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class RopeCreator : MonoBehaviour
{
    private CameraController cam;
    public GameObject ropeObj, player;
    public Transform camTransf;
    private Rope rope;
    public KeyCode keycode;
    public float turnSmoothTime, turnSmoothVelocity, ropeRange;
    public ThirdPersonMovement controller;
    private StateHandler state;
    public GameObject crosshair;
    public Color32 original, highlighted;
    //private float camSens = 1.0f;

    void Start()
    {
        rope = ropeObj.GetComponent<Rope>();
        cam = GameObject.Find("Player").GetComponent<CameraController>();
        player = GameObject.Find("Player");
        controller = player.GetComponent<ThirdPersonMovement>();
        state = GameObject.Find("Main Camera").GetComponent<StateHandler>();
//        original = crosshair.GetComponent<Image>().color;
    }

    void Update()
    {
        if (state.InvOpen()) {return;}

        CastRopeRaycast();

        if (Input.GetKey(keycode))
        {
            cam.Aim();
            player.transform.Rotate(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), 0.0f, Space.Self);
        }

        if (Input.GetKeyUp(keycode))
        {
            cam.StopAim();
            float prevY = player.transform.rotation.y * 180;
            player.transform.rotation = Quaternion.Euler(0f, prevY, 0f);
        }
    }

    void CastRopeRaycast()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, ropeRange))
        {
            var selection = hit.transform;
            if (selection.gameObject.tag == "Hook")
            {  
                rope.snapFirst = true;
                //ShowCrosshairIndicator();
                if (Input.GetKeyUp(keycode))
                {
                    rope.parentObj = selection.transform.gameObject;
                    rope.Spawn();
                }
            }
        }
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
}

