using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeDoor : MonoBehaviour
{
    // ===== public =====

    // 문 회전 속도
    public float door_rotation_speed = 70f;

    // 문 최소 회전 각도
    public float door_min_rotation_angle = 0f;

    // 문 최대 회전 각도
    public float door_max_rotation_angle = 130f;

    // 문 회전 방향
    public bool b_is_clockwise = true;

    // ===== private =====

    // 문 피벗 정보.
    private GameObject door_mesh_infor;

    // 문을 여는 애니메이션 재생 여부
    private bool b_is_door_opened = false;

    // 문 Game Object 객체 정보
    private GameObject door;
    
    // 현재 문 각도.
    private float door_rotation_y = 0f;

    //  public 쓰지 말고, private으로 선언한 뒤, Serialize를 활용.

    // Start is called before the first frame update
    void Start()
    {
        // GameObject.FindGameObjectWithTag()
        // 씬 내에서 해당 컴포넌트의 태그를 가진 첫 번째 객체를 반환함.

        // GetComponents는 MonoBehavior나 Component 클래스를 상속받을 때 사용할 수 있음. 그러나, GameObject는 두 클래스를 상속받지 않으므로 사용할 수 없음.

        // GameObject.Find() 함수는 Scene에 배치되어 있는 모든 객체에 대해서 검사를 진행함. ( 전역 )

        // transform.Find() 함수는 현재 스크립트가 있는 컴포넌트의 자식 Game Object에 대해서만 검사를 진행함. ( 지역 )

        // transform.Find() 함수에서 파라미터의 입력값으로 별다른 경로없이 문자열을 입력했으면, 해당 Game Object의 자식에 대해서만 검사를 진행함. "~/~"를 파라미터로 입력한 경우, 자식의 자식에 대해서도 검사함.

        // 'door_pivot' Game Object 객체 정보를 가져옴.
        door_mesh_infor = transform.Find("door_pivot").gameObject;

        // 'door' Game Object 정보를 가져옴.
        door = transform.Find("door_pivot/door").gameObject;

        // 'door' Game Object의 Y축 회전값을 설정.
        // transform.eulerAngles.y 값은 읽기만 가능하고 쓰기는 불가능함.
        door_rotation_y = door.transform.localEulerAngles.y;

        DoorOperate();
    }

    // Update is called once per frame
    void Update()
    {

        // 문 여는 애니메이션 재생
        if(b_is_door_opened)
        {
            // 문을 여는 과정.
            if(b_is_clockwise)
            {
                door_rotation_y += door_rotation_speed * Time.deltaTime;
            }

            // 문을 닫는 과정.
            else
            {
                door_rotation_y -= door_rotation_speed * Time.deltaTime;
            }

            // 문이 완전히 닫혔을 때
            if(door_rotation_y <= door_min_rotation_angle)
            {
                // 문의 최소 각도로 설정.
                door_rotation_y = door_min_rotation_angle;

                // 문이 더 이상 움직이지 않도록 설정.
                // b_is_door_opened = false;

                // 문이 열리는 방향으로 변경.
                b_is_clockwise = true;
            }

            // 문이 완전히 열렸을 때
            else if(door_rotation_y >= door_max_rotation_angle)
            {
                // 문의 최대 각도로 설정.
                door_rotation_y = door_max_rotation_angle;

                // 문이 더 이상 움직이지 않도록 설정.
                // b_is_door_opened = false;

                // 문이 닫히는 방향으로 변경.
                b_is_clockwise = false;
            }

            // 문이 아직 열리거나 닫히고 있는 도중에
            else
            {
                // Ignore
            }

            // 문 회전각도 설정.
            door.transform.localRotation = Quaternion.Euler(0f, door_rotation_y, 0f);
        }
    }

    // 문을 작동시키는 함수.
    public void DoorOperate()
    {
        b_is_door_opened = true;
    }
}
