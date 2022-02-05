using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    //public Bow bow;
    public GameObject mainCamera;
    public GameObject aimCamera;
    public GameObject crosshair;
    public GameObject arrow;
    private CinemachineImpulseSource source;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        GameObject.Find("Crosshair").SetActive(false);
    }

    IEnumerator CamDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
