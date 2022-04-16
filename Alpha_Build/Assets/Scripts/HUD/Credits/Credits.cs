using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField]
    GameObject mainCreditsOn;
    [SerializeField]
    GameObject topCreditsOn;
    [SerializeField]
    GameObject mainCreditsOff;

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            loadMainMenu();
        }
    }

    public void TurnOffCreditsLight()
    {
        mainCreditsOff.SetActive(true);
        mainCreditsOn.SetActive(false);
        topCreditsOn.SetActive(false);
    }

    public void loadMainMenu()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartScene");
    }
}
