using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClickStart()
    {
#if UNITY_EDITOR
        Debug.Log("Start");
#endif
    }

    public void OnClickLoad()
    {
#if UNITY_EDITOR
        Debug.Log("Load");
#endif
    }

    public void OnClickConfig()
    {
#if UNITY_EDITOR
        Debug.Log("Config");
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
