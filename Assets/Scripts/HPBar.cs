using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [SerializeField]
    Slider hp;

    void Update()
    {
        hp.value = SystemManager.Instance.Player.heart / 5.0f;
    }
}
