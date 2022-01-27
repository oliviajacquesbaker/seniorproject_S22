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
    public float maxRotation;
    public float minRotation;
    private float currentBowRotation;
    public bool isAiming;
    private GameObject mainCamera;
    private GameObject aimCamera;
    private GameObject crosshair;
    private GameObject inv;
    private StateHandler state;
    
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
    }

    void Update()
    {
        if (!state.InvOpen() && gameObject.activeInHierarchy) // check if inventory is not open AND bow is active weapon
        {         
            if (Input.GetKey(fireButton))
            {
                Aim();
                
                if (_charge < chargeMax )
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
        Rigidbody arrow = Instantiate(arrowObj, spawn.position, Quaternion.identity) as Rigidbody;
        arrow.AddForce(spawn.forward * _charge, ForceMode.Impulse);
        _charge = 0;
        isAiming = false;
    }

    void Aim()
    {
        currentBowRotation = gameObject.transform.eulerAngles.x;
        playerSpeed.speed = 2f;
        playerSpeed.turnSmoothTime = targetTurnVelocity;
        Player.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
        bowRotation.Rotate(Input.GetAxis("Mouse Y") * -1, 0.0f, 0.0f, Space.Self);
        mainCamera.SetActive(false);
        aimCamera.SetActive(true);
        //StartCoroutine(CrosshairDelay(0.5f));
        crosshair.SetActive(true);
    }

    void StopAiming()
    {
        playerSpeed.speed = 10f;
        playerSpeed.turnSmoothTime = initialTurnVelocity;
        mainCamera.SetActive(true);
        aimCamera.SetActive(false); 
        crosshair.SetActive(false);
    }

    IEnumerator CrosshairDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        crosshair.SetActive(true);
    }

}
