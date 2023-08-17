using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Setting : MonoBehaviour
{
    [SerializeField]
    Scrollbar seBar;
    [SerializeField]
    Scrollbar bgmBar;
    
    public static float soundEffect = 0.5f;
    public static float bgm = 0.5f;

    void Start()
    {
        Debug.Log(soundEffect);
    }

    public void OnSeChanged(float value)
    {
        soundEffect = value;
    }

    public void OnBgmChanged(float value)
    {
        bgm = value;
    }
}
