using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject aimCamera;
    public GameObject crosshair, targetCrosshair;
    private GameObject aimPoint;
    public ThirdPersonMovement playerSpeed;
    public float targetTurnVelocity;
    private float initialTurnVelocity;
    [SerializeField]
    private RopeCreator rope;
    private GameObject activeCrosshair;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        playerSpeed = GameObject.Find("Player").GetComponent<ThirdPersonMovement>();
        initialTurnVelocity = playerSpeed.turnSmoothTime;
        GameObject.Find("Crosshair").SetActive(false);
        GameObject.Find("Target Crosshair").SetActive(false);
        aimPoint = GameObject.Find("AimPoint");
    }

    void Update()
    {
        if (rope.isAimingHook)
        {
            activeCrosshair = targetCrosshair;
        }
        else
        {
            activeCrosshair = crosshair;
        }
    }

    public void Aim()
    {
        playerSpeed.speed = 2f;
        playerSpeed.turnSmoothTime = targetTurnVelocity;
        //transform.LookAt(aimPoint.transform);
        aimPoint.transform.Rotate(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), 0.0f, Space.Self);
        mainCamera.SetActive(false);
        aimCamera.SetActive(true);
        crosshair.SetActive(false);
        targetCrosshair.SetActive(false);
        activeCrosshair.SetActive(true);
    }

    public void StopAim()
    {  
        playerSpeed.speed = 10f;
        playerSpeed.turnSmoothTime = initialTurnVelocity;
        mainCamera.SetActive(true);
        aimCamera.SetActive(false);
        //crosshair.SetActive(false);
        crosshair.SetActive(false);
        targetCrosshair.SetActive(false);
        activeCrosshair.SetActive(false);
    }

    IEnumerator CamDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
