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
    private float initialTurnVelocity;
    public float targetTurnVelocity;
    public float fireDecay;
    public KeyCode fireButton;
    public Transform spawn;
    private Transform bowRotation;
    public Rigidbody arrowObj;
    private Rigidbody playerRB;
    private GameObject Player;
    private GameObject bullet;
    private ThirdPersonMovement playerSpeed;
    private CinemachineFreeLook cam; 
    //public float maxRotation;
    //public float minRotation;
    private float currentBowRotation;
    public bool isAiming;
    private GameObject mainCamera;
    private GameObject aimCamera;
    private GameObject crosshair;
    private GameObject inv;
    private StateHandler state;
    private Durability durability;
    private CameraController camController;
    
    void Start()
    {
        Player = GameObject.Find("Player");
        playerSpeed = Player.GetComponent<ThirdPersonMovement>();
        bowRotation = gameObject.transform;
        cam = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
        initialTurnVelocity = playerSpeed.turnSmoothTime;
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        crosshair = GameObject.Find("Crosshair");
        state = GameObject.Find("Main Camera").GetComponent<StateHandler>();
        durability = gameObject.GetComponent<Durability>();
        camController = Player.GetComponent<CameraController>();
        playerRB = Player.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (!state.InvOpen()) // check if inventory is not open
        {         
            if (Input.GetKey(fireButton))
            {
                Aim();
                
                if (_charge < chargeMax)
                {
                    ChargeBow();
                }

            }

            if (Input.GetKeyUp(fireButton))
            {
                Fire();
                StopAiming();
            }
        }
    }

    void ChargeBow()
    {
        _charge += Time.deltaTime * chargeRate;
        isAiming = true;
    }

    void Fire()
    {
        Rigidbody arrow = Instantiate(arrowObj, spawn.position, transform.rotation * Quaternion.Euler(270f,0f,0f)) as Rigidbody;
        arrow.AddForce(spawn.forward * _charge, ForceMode.Impulse);
        _charge = 0;
        durability.currDurability -= fireDecay;
        isAiming = false;
    }

    void Aim()
    {

        camController.Aim();
        Player.transform.Rotate(0.0f, Input.GetAxis("Mouse X"), 0.0f);
        bowRotation.Rotate(Input.GetAxis("Mouse Y") * -1, 0.0f, 0.0f, Space.Self);
    }

    void StopAiming()
    {
        camController.StopAim();
        //transform.rotation = Quaternion.identity;
    }

    // IEnumerator CrosshairDelay(float seconds)
    // {
    //     yield return new WaitForSeconds(seconds);
    //     crosshair.SetActive(true);
    // }

}
