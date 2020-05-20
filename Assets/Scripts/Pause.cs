using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public GameObject pause;
    public static bool isPaused;

    private void Awake()
    {
        isPaused = true;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (pause != null)
            {
                if (!isPaused)
                {
                    Time.timeScale = 0;
                    isPaused = true;
                    pause.SetActive(true);
                }
                else if (isPaused && PlayerMovement.dead == false)
                {
                    Time.timeScale = 1;
                    isPaused = false;
                    pause.SetActive(false);
                }
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        isPaused = false;
        pause.SetActive(false);
    }
    
    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    
    public void Restart()
    {
        Time.timeScale = 1;
        isPaused = false;
        SceneManager.LoadScene("Test");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
