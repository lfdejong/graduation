using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    public Button[] buttons;
 
    public void PlayGame(int testId)
    {
        string test = "Test " + testId;
        SceneManager.LoadScene(test);
    }
}
