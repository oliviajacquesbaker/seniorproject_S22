using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bow : MonoBehaviour
{

    float _charge;

    public float chargeMax;

    public float chargeRate;

    public float rotationSpeed;

    public float lerpValue;

    public float fovTarget;

    private float initialFov;

    public KeyCode fireButton;

    public Transform spawn;

    public Transform bowRotation;

    public Rigidbody arrowObj;

    private GameObject Player;

    private ThirdPersonMovement playerSpeed;

    private CinemachineFreeLook cam; 

    public bool isAiming;
    
    void Start()
    {
        Player = GameObject.Find("Player");
        playerSpeed = Player.GetComponent<ThirdPersonMovement>();
        bowRotation = gameObject.transform;
        cam = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
        initialFov = cam.m_Lens.FieldOfView;
    }

    void Update()
    {
        if (Input.GetKey(fireButton) && _charge < chargeMax)
        {
            _charge += Time.deltaTime * chargeRate;
            isAiming = true;
            //Debug.Log(_charge.ToString());
        }

        if (Input.GetKeyUp(fireButton))
        {
            Rigidbody arrow = Instantiate(arrowObj, spawn.position, Quaternion.identity) as Rigidbody;
            arrow.AddForce(spawn.forward * _charge, ForceMode.Impulse);
            _charge = 0;
            isAiming = false;
        }

        if (isAiming) 
        {
            playerSpeed.speed = 5f;
            bowRotation.Rotate(Input.GetAxis("Mouse Y") * rotationSpeed * -1, 0.0f, 0.0f, Space.Self);
            var currentFov = cam.m_Lens.FieldOfView;
            var fov = Mathf.Lerp(currentFov, fovTarget, Time.deltaTime * lerpValue);
            cam.m_Lens.FieldOfView = fov;
            Debug.Log(fov);
        }
        else
        {
            playerSpeed.speed = 10f;
            bowRotation.Rotate(0.0f, 0.0f, 0.0f, Space.Self); // reset x to 0
            var currentFov = cam.m_Lens.FieldOfView;
            cam.m_Lens.FieldOfView = Mathf.Lerp(currentFov, initialFov, Time.deltaTime * lerpValue);
        }
    }
}
