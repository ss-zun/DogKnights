using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void OnClickStart()
    {
#if UNITY_EDITOR
        Debug.Log("Clicked Start Button");
#endif
    }

    public void OnClickLoad()
    {
#if UNITY_EDITOR
        Debug.Log("Clicked Load Button");
#endif
    }

    public void OnClickConfig()
    {
#if UNITY_EDITOR
        Debug.Log("Clicked Config Button");
#endif
    }

    public void OnClickQuit()
    {
#if UNITY_EDITOR
        Debug.Log("Clicked Quit Button");
#else
        Application.Quit();
#endif
    }
}
