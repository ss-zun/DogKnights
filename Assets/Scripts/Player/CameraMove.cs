using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{   
    public GameObject Target;
    public float offsetX = 0.0f;
    public float offsetY = 4.0f;
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
        //Vector3 camPos = Target.transform.position;
        if (Target.GetComponent<Player>().isMapPuzzle)
        {
            cameraToVertical(16f);
        }
        else
        {

            cameraToHorizontal(-25f);
        }
        //camPos.z = camPos.z-16;
        //transform.position = camPos;

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }

    void FixedUpdate(){
        //TargetPos = new Vector3(
        //    Target.transform.position.x + offsetX,
        //    Target.transform.position.y + offsetY,
        //    Target.transform.position.z + offsetZ
        //);

        transform.position = Vector3.Lerp(transform.position, TargetPos, Time.deltaTime * CameraSpeed);
    }

    void cameraToHorizontal(float offset)
    {

        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offsetY,
            Target.transform.position.z + offset
        );
        transform.rotation = Quaternion.Euler(0,0, 0);
    }

    void cameraToVertical(float offset)
    {
        TargetPos = new Vector3(
            Target.transform.position.x + offsetX,
            Target.transform.position.y + offset,
            Target.transform.position.z + 0.0f
        ) ;
        transform.rotation = Quaternion.Euler(90f, 0, 0);
    }


}
