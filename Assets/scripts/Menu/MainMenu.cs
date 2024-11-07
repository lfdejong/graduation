using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject tutorial;
    [SerializeField] GameObject menu;

    private void Start()
    {
        menu.SetActive(true);
        tutorial.SetActive(false);
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("demo");
    }

    public void Tutorial()
    {
        menu.SetActive(false);
        tutorial.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
