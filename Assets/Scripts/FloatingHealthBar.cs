using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingHealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private readonly Camera cam;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 offset;

    public void UpdateHealthBar(float current, float max)
    {
        slider.value = current / max;
    }

    void Update()
    {
        transform.rotation = cam.transform.rotation;
        transform.position = target.position + offset;
    }
}
