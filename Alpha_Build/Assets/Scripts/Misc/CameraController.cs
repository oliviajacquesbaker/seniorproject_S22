using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject aimCamera;
    public GameObject crosshair;
    public GameObject arrow;
    public GameObject aimPoint;
    public ThirdPersonMovement playerSpeed;
    private CinemachineImpulseSource source;
    public float initialTurnVelocity, targetTurnVelocity;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        playerSpeed = GameObject.Find("Player").GetComponent<ThirdPersonMovement>();
        initialTurnVelocity = playerSpeed.turnSmoothTime;
        GameObject.Find("Crosshair").SetActive(false);
        aimPoint = GameObject.Find("AimPoint");
    }

    public void Aim()
    {
        playerSpeed.speed = 2f;
        playerSpeed.turnSmoothTime = 1.5f;
        //GameObject.Find("AimPoint").transform.Rotate(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), 0.0f, Space.Self);
        mainCamera.SetActive(false);
        aimCamera.SetActive(true);
        crosshair.SetActive(true);
    }

    public void StopAim()
    {  
        playerSpeed.speed = 10f;
        playerSpeed.turnSmoothTime = initialTurnVelocity;
        mainCamera.SetActive(true);
        aimCamera.SetActive(false);
        crosshair.SetActive(false);    
    }

    IEnumerator CamDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
