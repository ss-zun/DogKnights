using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void ToStartScreen()
    {
        SceneManager.LoadScene("Intro");
    }

    public void ToGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ToSettings()
    {
        SceneManager.LoadScene("Setting");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
