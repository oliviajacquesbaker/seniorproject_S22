using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PausedController : MonoBehaviour
{
    [SerializeField]
    GameObject menu,controls;
    [SerializeField]
    Button saveBtn,controlBtn,returnBtn;

    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip click, error;


    private bool paused = false;

    private void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            paused = !paused;

            if (paused)
            {
                ShowMenu();
                PauseGame();

                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
            else if (!paused)
            {
                HideMenu();
                UnpauseGame();

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

    }


    void ShowMenu()
    {
        menu.SetActive(true);
    }

    void HideMenu()
    {
        menu.SetActive(false);
    }

    void PauseGame()
    {
        Time.timeScale = 0;
    }

    void UnpauseGame()
    {
        Time.timeScale = 1;
    }

    public void SaveExitBtn()
    {
        PlayerPrefs.SetInt("savedLevel", SceneManager.GetActiveScene().buildIndex);
        source.clip = click;
        source.Play();
        UnpauseGame();
        SceneManager.LoadScene("StartScene");
    }

    public void ControlsBtn()
    {
        source.clip = click;
        source.Play();

        saveBtn.gameObject.SetActive(false);
        controlBtn.gameObject.SetActive(false);

        controls.SetActive(true);
        returnBtn.gameObject.SetActive(true);
    }

    public void ReturnBtn()
    {
        source.clip = click;
        source.Play();

        saveBtn.gameObject.SetActive(true);
        controlBtn.gameObject.SetActive(true);

        controls.SetActive(false);
        returnBtn.gameObject.SetActive(false);
    }

}
