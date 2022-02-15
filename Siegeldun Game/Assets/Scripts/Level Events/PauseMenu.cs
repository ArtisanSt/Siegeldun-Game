using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] public bool isPaused = false;
    [SerializeField] public GameObject PauseMenuUI;

    void Awake()
    {
        PauseMenuUI = GameObject.Find("/GUI/PauseMenu");
        if (isPaused) Pause();
        else Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if(PauseMenuUI != null && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        if (isPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void Resume()
    {
        Debug.Log(true);
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }

    void OnClick()
    {
        if (isPaused == true)
        {
            return;  // ignore click
        }

        // otherwise do normal click logic
    }

}
