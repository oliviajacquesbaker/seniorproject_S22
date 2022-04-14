using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            loadMainMenu();
        }
    }

    public void loadMainMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
}
