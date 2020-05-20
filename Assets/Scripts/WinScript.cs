using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScript : MonoBehaviour
{
    public GameObject winScreen;
    public Text timer;
    public int time = 300;

    private float gameTime = 0;

    private void Update()
    {
        int min = time / 60;
        timer.text = min + ":" + (time - min * 60);
        if (Pause.isPaused == false)
        {
            gameTime += Time.deltaTime;
            if (gameTime >= 1)
            {
                time--;
                gameTime = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.GetComponent<PlayerPlane>().fuel >= other.GetComponent<PlayerPlane>().fuelMax)
            {
                Pause.isPaused = true;
                Time.timeScale = 0;
                winScreen.SetActive(true);
            }
        }
    }
}
