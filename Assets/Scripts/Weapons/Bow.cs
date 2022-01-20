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

    private float initialTurnVelocity;

    public float targetTurnVelocity;

    public KeyCode fireButton;

    public Transform spawn;

    public Transform bowRotation;

    public Rigidbody arrowObj;

    private GameObject Player;

    private GameObject bullet;

    private ThirdPersonMovement playerSpeed;

    private CinemachineFreeLook cam; 

    //public Cinemachine.CinemachineImpulseSource source;

    public CinemachineVirtualCamera aimCamera;

    public bool isAiming;
    
    void Start()
    {
        Player = GameObject.Find("Player");
        playerSpeed = Player.GetComponent<ThirdPersonMovement>();
        bowRotation = gameObject.transform;
        cam = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
        initialFov = cam.m_Lens.FieldOfView;
        initialTurnVelocity = playerSpeed.turnSmoothTime;
        bullet = (GameObject)Resources.Load("prefabs/BulletDebug", typeof(GameObject));
        //source = bullet.GetComponent<Cinemachine.CinemachineImpulseSource>();
        aimCamera = GameObject.Find("Aim Camera").GetComponent<CinemachineVirtualCamera>();
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
            playerSpeed.speed = 2f;
            bowRotation.Rotate(Input.GetAxis("Mouse Y") * -1, 0.0f, 0.0f, Space.Self);
            playerSpeed.turnSmoothTime = targetTurnVelocity;
        }
        else
        {

            playerSpeed.speed = 10f;
            playerSpeed.turnSmoothTime = initialTurnVelocity;
        }
    }
}
