using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    public Bow bow;
    public GameObject mainCamera;
    public GameObject aimCamera;
    public GameObject crosshair;
    public GameObject arrow;
    private CinemachineImpulseSource source;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
//        bow = GameObject.Find("Bow").GetComponent<Bow>();
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        crosshair = GameObject.Find("Crosshair");
        arrow = (GameObject)Resources.Load("prefabs/BulletDebug", typeof(GameObject));
        source = arrow.GetComponent<CinemachineImpulseSource>();
        GameObject.Find("Crosshair").SetActive(false);
        //crosshair.SetActive(false);
    }

    void Update()
    {
        // if (bow.ActiveSelf && bow.isAiming)
        // {
        //     mainCamera.SetActive(false);
        //     aimCamera.SetActive(true);
        //     crosshair.SetActive(true);
        // }
        // else
        // {   
        //     mainCamera.SetActive(true);
        //     aimCamera.SetActive(false); 
        //     crosshair.SetActive(false);
        // }
    }

    IEnumerator CamDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
