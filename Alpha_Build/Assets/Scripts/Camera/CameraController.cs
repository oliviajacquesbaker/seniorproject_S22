using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject aimCamera;
    public GameObject crosshair, targetCrosshair;
    [SerializeField]
    private GameObject aimPoint;
    public ThirdPersonMovement playerSpeed;
    public float targetTurnVelocity;
    private float initialTurnVelocity;
    [SerializeField]
    private RopeCreator rope;
    private GameObject activeCrosshair;
    //private Quaternion originalRotation;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        playerSpeed = GameObject.Find("Player").GetComponent<ThirdPersonMovement>();
        initialTurnVelocity = playerSpeed.turnSmoothTime;
        GameObject.Find("Crosshair").SetActive(false);
        if (GameObject.Find("Target Crosshair")) GameObject.Find("Target Crosshair").SetActive(false);
        aimPoint = GameObject.Find("AimPoint");
        //originalRotation = aimPoint.gameObject.transform.rotation;
    }

    void Update()
    {
        if (rope && rope.isAimingHook)
        {
            ToggleTargetCrosshair();
        }
        else
        {
            ToggleNormalCrosshair();
        }
    }

    public void ToggleTargetCrosshair()
    {
        activeCrosshair = targetCrosshair;
    }

    public void ToggleNormalCrosshair()
    {
        activeCrosshair = crosshair;
    }

    public void Aim()
    {
        playerSpeed.enabled = false;
        playerSpeed.turnSmoothTime = targetTurnVelocity;
        //transform.LookAt(GameObject.Find("ArrowSpawnPoint").transform);
        aimPoint.transform.Rotate(Input.GetAxis("Mouse Y") * -1, Input.GetAxis("Mouse X"), 0.0f, Space.Self);
        mainCamera.SetActive(false);
        aimCamera.SetActive(true);
        crosshair.SetActive(false);
        targetCrosshair.SetActive(false);
        activeCrosshair.SetActive(true);
    }

    public void StopAim()
    {
        playerSpeed.enabled = true;
        playerSpeed.turnSmoothTime = initialTurnVelocity;
        mainCamera.SetActive(true);
        aimCamera.SetActive(false);
        //crosshair.SetActive(false);
        crosshair.SetActive(false);
        targetCrosshair.SetActive(false);
        activeCrosshair.SetActive(false);
        aimPoint.transform.rotation = GameObject.Find("Follow Target").transform.rotation;
    }

    IEnumerator CamDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
