using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    [SerializeField]
    Button continueBtn, optionsBtn;

    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip click, error;

    private void Start()
    {
        Cursor.visible = true;
        if (!PlayerPrefs.HasKey("savedLevel"))
        {
            continueBtn.interactable = false;
        }
        else
        {
            continueBtn.interactable = true;
        }
    }
    public void NewGame()
    {
        source.clip = click;
        source.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        source.clip = click;
        source.Play();
        SceneManager.LoadScene(PlayerPrefs.GetInt("savedLevel"));
    }

    public void OpenCredits()
    {
        source.clip = click;
        source.Play();
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        source.clip = click;
        source.Play();
        Application.Quit();
    }

}
