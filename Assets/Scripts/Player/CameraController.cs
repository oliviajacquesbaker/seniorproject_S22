using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{

    public Bow aiming;
    public GameObject mainCamera;
    public GameObject aimCamera;
    public GameObject crosshair;
    public GameObject arrow;
    private CinemachineImpulseSource source;

    void Start()
    {
        //aiming = GameObject.Find("Bow");
        Cursor.lockState = CursorLockMode.Locked;
        aiming = GameObject.Find("Bow").GetComponent<Bow>();
        mainCamera = GameObject.Find("Third Person Camera");
        aimCamera = GameObject.Find("Aim Camera");
        crosshair = GameObject.Find("Crosshair");
        arrow = (GameObject)Resources.Load("prefabs/BulletDebug", typeof(GameObject));
        source = arrow.GetComponent<CinemachineImpulseSource>();
        crosshair.SetActive(false);
    }

    void Update()
    {
        // if (aiming.isAiming)
        // {
        //     mainCamera.SetActive(false);
        //     aimCamera.SetActive(true);
        //     crosshair.SetActive(true);
        // }
        // else
        // {
        //     //StartCoroutine(CamDelay(3f));
        //     // //source.GenerateImpulse(Camera.main.transform.forward);
        //     // mainCamera.SetActive(true);
        //     // aimCamera.SetActive(false); 
        //     // crosshair.SetActive(false);
        // }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mainCamera.SetActive(false);
            aimCamera.SetActive(true);
            StartCoroutine(CamDelay(3f));
            crosshair.SetActive(true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {   
            mainCamera.SetActive(true);
            aimCamera.SetActive(false); 
            crosshair.SetActive(false);
            Debug.Log("isnide");
            //source.GenerateImpulse(Camera.main.transform.forward * 5f);
        }
    }

    IEnumerator CamDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
    }

}
