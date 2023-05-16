using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubMenu : MonoBehaviour
{
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
#endif
    }
}
