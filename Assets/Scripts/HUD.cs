using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { 
        Health,
        Money,
        Kill,
        Stage
    }
    public InfoType type;

    private void Awake()
    {
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Health:
                break;
            case InfoType.Money:
                break;
            case InfoType.Kill:
                break;
            case InfoType.Stage:
                break;
        }
    }
}
