using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject aimCamera;
    public GameObject crosshair;
    private GameObject aimPoint;
    private GameObject player;
    public ThirdPersonMovement playerSpeed;
    private CinemachineFreeLook cam;
    public float targetTurnVelocity, rotationSpeed;
    private float initialTurnVelocity;
    private bool hasRotated;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        player = GameObject.Find("Player");
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        playerSpeed = GameObject.Find("Player").GetComponent<ThirdPersonMovement>();
        initialTurnVelocity = playerSpeed.turnSmoothTime;
        GameObject.Find("Crosshair").SetActive(false);
        aimPoint = GameObject.Find("AimPoint");
        cam = GameObject.Find("Third Person Camera").GetComponent<CinemachineFreeLook>();
        hasRotated = false;
    }

    public void Aim()
    {
        playerSpeed.speed = 2f;
        playerSpeed.turnSmoothTime = targetTurnVelocity;
        if (!hasRotated)
        {
            RotatePlayer();
        }
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
        hasRotated = false;  
    }

    void RotatePlayer()
    {
        player.transform.localEulerAngles = new Vector3(player.transform.rotation.x, cam.m_XAxis.Value, player.transform.rotation.z);
        hasRotated = true;
    }

    IEnumerator CamDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
