using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{   
    public GameObject Target;
    float offsetX = 0.0f;
    float offsetY = 0.0f;
    public float offsetZ = -16.0f;

    public float CameraSpeed = 10.0f;
    Vector3 TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 camPos = Target.transform.position;
        //camPos.z = camPos.z-16;
        //transform.position = camPos;
    }

    void FixedUpdate(){
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offsetZ
        );

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }


}
