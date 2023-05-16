using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { Health }
    public InfoType type;

    Text text;
    Slider slider;

    private void Awake()
    {
        text = GetComponent<Text>();
        slider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Health:
                break;
        }
    }
}
