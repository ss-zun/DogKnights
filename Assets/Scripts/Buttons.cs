using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ToGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void Restart()
    {
        Debug.Log("Restart");
        SceneManager.LoadScene("MainScene");
        GameManager.Instance.Player.Restart();
    }

    public void Resume()
    {
        Debug.Log("resume");
        GameManager.Instance.Player.OnButtonResume();
    }

    public void Exit()
    {
        Debug.Log("exit");
        GameManager.Instance.Player.OnButtonExit();

    }
}
