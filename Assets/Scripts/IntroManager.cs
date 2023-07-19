using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private void Start()
    {
        
    }

    public void OnClickStart()
    {
#if UNITY_EDITOR
        Debug.Log("Start");
        SceneManager.LoadScene("HUD");
#endif
    }

    public void OnClickLoad()
    {
#if UNITY_EDITOR
        Debug.Log("Load");
        SceneManager.LoadScene("SampleUI");
#endif
    }

    public void OnClickConfig()
    {
#if UNITY_EDITOR
        Debug.Log("Setting");
        SceneManager.LoadScene("Setting");
#endif
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        Debug.Log("Quit");
#else
        Application.Quit();
#endif
    }
}
