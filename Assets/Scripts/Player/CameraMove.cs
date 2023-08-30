using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = GameManager.Instance.Player.transform.position;
        camPos.z = camPos.z-16;
        transform.position = camPos;
    }


}
