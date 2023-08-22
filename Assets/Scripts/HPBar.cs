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
        hp.value = GameManager.Instance.Player.heart / GameManager.Instance.Player.maxHeart;
    }
}
