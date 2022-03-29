using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    [SerializeField]
    Button continueBtn, optionsBtn;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("savedLevel"))
        {
            continueBtn.interactable = false;
        }
        else
        {
            continueBtn.interactable = true;
        }
        optionsBtn.interactable = false;
    }
    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("savedLevel"));
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

}
