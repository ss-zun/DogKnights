using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickNew()
    {
        Debug.Log("New Game");
    }

    public void OnClickLoad()
    {
        Debug.Log("Load");
    }

    public void OnClickConfig()
    {
        Debug.Log("Config");
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
