using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.right * 30 * Time.deltaTime); //°øÀü
    }
}
