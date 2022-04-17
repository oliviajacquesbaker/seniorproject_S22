using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialToMain : MonoBehaviour
{

    private bool inCollider = false;

    private void OnTriggerEnter(Collider other)
    {
        inCollider = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inCollider = false;
    }
    
    void Update()
    {
        if (inCollider)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }
}
