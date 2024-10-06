using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject PauseMenuUi;
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (gameIsPaused )
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }
    public void QuitGame()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void Pause()
    {
        PauseMenuUi.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
        Input.gyro.enabled = false;
    }

    public void Resume()
    {
        PauseMenuUi.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
        Input.gyro.enabled = true;
    }
}
