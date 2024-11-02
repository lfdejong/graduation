using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
 
    public void PlayGame()
    {
        SceneManager.LoadScene("toolkit");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
