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

    public KeyCode fireButton;

    public Transform spawn;

    private Transform bowRotation;

    public Rigidbody arrowObj;

    private GameObject Player;

    private GameObject bullet;

    private ThirdPersonMovement playerSpeed;

    private CinemachineFreeLook cam; 

    //public Cinemachine.CinemachineImpulseSource source;

    //private CinemachineVirtualCamera aimCamera;

    public float maxRotation;

    public float minRotation;

    private float currentBowRotation;
    public bool isAiming;
    private GameObject mainCamera;
    private GameObject aimCamera;
    private GameObject crosshair;
    private GameObject inv;
    
    
    void Start()
    {
        Player = GameObject.Find("Player");
        playerSpeed = Player.GetComponent<ThirdPersonMovement>();
        bowRotation = gameObject.transform;
        cam = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
        initialTurnVelocity = playerSpeed.turnSmoothTime;
        bullet = (GameObject)Resources.Load("prefabs/BulletDebug", typeof(GameObject));
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        crosshair = GameObject.Find("Crosshair");
        inv = GameObject.Find("Inventory");
    }

    void Update()
    {
        if (!inv.activeInHierarchy && gameObject.activeInHierarchy) // check if inventory is not open AND bow is active weapon
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
                currentBowRotation = gameObject.transform.eulerAngles.x;
                playerSpeed.speed = 2f;
                playerSpeed.turnSmoothTime = targetTurnVelocity;
                //Debug.Log(currentBowRotation);
                Player.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
                bowRotation.Rotate(Input.GetAxis("Mouse Y") * -1, 0.0f, 0.0f, Space.Self);
                mainCamera.SetActive(false);
                aimCamera.SetActive(true);
                crosshair.SetActive(true);
            }
            else
            {
                playerSpeed.speed = 10f;
                playerSpeed.turnSmoothTime = initialTurnVelocity;
                mainCamera.SetActive(true);
                aimCamera.SetActive(false); 
                crosshair.SetActive(false);
            }
        }
    }
}
