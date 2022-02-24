using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private CameraController cam;
    public GameObject ropeObj, player;
    public Transform camTransf;
    private Rope rope;
    public KeyCode keycode;
    public float climbRate, checkRadius;
    public float turnSmoothTime, turnSmoothVelocity;

    public LayerMask ropeLayers;
    //private float camSens = 1.0f;

    void Start()
    {
        rope = ropeObj.GetComponent<Rope>();
        cam = GameObject.Find("Player").GetComponent<CameraController>();
        player = GameObject.Find("Player");
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("Raycast hit " + hit.transform.name);
            var selection = hit.transform;
            if (selection.gameObject.tag == "Hook")
            {  
                Debug.Log("Hit the hook!");
                rope.snapFirst = true;
                if (Input.GetKeyUp(keycode))
                {
                    rope.parentObj = selection.transform.gameObject;
                    rope.Spawn();
                    Debug.Log("Spawned rope at object: " + rope.parentObj.name);
                }
            }
        }

        if (IsNextToRope())
        {
            Debug.Log("Next to rope!");

            if (Input.GetKey(KeyCode.E))
            {
                player.GetComponent<Rigidbody>().isKinematic = true;
                player.transform.Translate(Vector3.up * Time.deltaTime * climbRate, Space.Self);
            }
        }
        else if (Input.GetKeyUp(KeyCode.E) || !IsNextToRope())
        {
            player.GetComponent<Rigidbody>().isKinematic = false;
        }

        if (Input.GetKey(keycode))
        {
            cam.Aim();
            player.transform.Rotate(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), 0.0f, Space.Self); // change this from player to another object which still allows player to move camera inthe y axis
            //GameObject.Find("AimPoint").transform.Rotate(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), 0.0f, Space.Self);
        }

        if (Input.GetKeyUp(keycode))
        {
            cam.StopAim();
            float prevY = player.transform.rotation.y * 180;
            player.transform.rotation = Quaternion.Euler(0f, prevY, 0f);
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

}
