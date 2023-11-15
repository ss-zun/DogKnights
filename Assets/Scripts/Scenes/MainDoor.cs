using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainDoor : NextStageDoor
{

    // ===== public =====

    // 문이 열리는 속도
    public float door_rotation_speed = 70f;

    // 문이 닫히는 각도
    public float door_min_rotation_angle = 0f;

    // 문이 최대로 열리는 각도
    public float door_max_rotation_angle = 90f;

    // 문 개폐 방향 설정 여부
    public bool b_is_door_closed = true;

    // 왼쪽 문 정보
    public GameObject left_door;

    // 오른쪽 문 정보
    public GameObject right_door;

    // 현재 스테이지 번호
    public int current_stage;

    // 다음 스테이지 번호
    public int next_stage;

    // ===== private =====

    // 현재 문이 열린 각도
    private float current_angle_door_y = 0f;

    // 현재 문이 열리는 애니메이션 작동 여부
    private bool b_is_door_opening = false;

    // 다음 스테이지로 이동하는 Box Collider
    private BoxCollider portalCollider;


    void Awake()
    {
        // 왼쪽 문과 오른쪽 문이 정상적으로 연결될 때만 실행.
        if(left_door != null && right_door != null)
        {
            // 문이 닫혀 있다면
            if(b_is_door_closed)
            {
                // 문 회전값 초기화
                left_door.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                right_door.transform.localEulerAngles = new Vector3(0f, 0f, 0f);

                // 현재 회전 각도 수정
                current_angle_door_y = 0f;
            }

            // 문이 열려 있다면
            else
            {
                // 문 회전값 초기화
                left_door.transform.localEulerAngles = new Vector3(0f, -door_max_rotation_angle, 0f);
                right_door.transform.localEulerAngles = new Vector3(0f, door_max_rotation_angle, 0f);

                // 현재 회전 각도 수정.
                current_angle_door_y = door_max_rotation_angle;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        portalCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        // 문 개폐 작동 중
        if(b_is_door_opening)
        {
            // 문이 열리는 과정 재생
            if(b_is_door_closed)
            {
                // 문 회전 각도 증가
                current_angle_door_y += door_rotation_speed * Time.deltaTime;

                // 현재 문의 각도가 최대 회전 각도보다 클 때,
                if(current_angle_door_y >= door_max_rotation_angle)
                {
                    // 최대 회전 각도로 고정
                    current_angle_door_y = door_max_rotation_angle;

                    // 문 개폐 종료
                    b_is_door_opening = false;

                    // 문이 완전히 열렸음을 알림
                    b_is_door_closed = false;
                }
            }

            // 문이 닫히는 과정 재생
            else
            {
                // 문 회전 각도 감소
                current_angle_door_y -= door_rotation_speed * Time.deltaTime;

                // 현재 문의 각도가 최소 회전 각도보다 작을 때,
                if (current_angle_door_y < door_min_rotation_angle)
                {
                    // 최대 회전 각도로 고정
                    current_angle_door_y = door_min_rotation_angle;

                    // 문 개폐 종료
                    b_is_door_opening = false;

                    // 문이 완전히 닫혔음을 알림
                    b_is_door_closed = true;
                }
            }

            // 문 회전 설정.
            left_door.transform.localEulerAngles = new Vector3(0f, -current_angle_door_y, 0f);
            right_door.transform.localEulerAngles = new Vector3(0f, current_angle_door_y, 0f);

        }
    }
    /// <summary>
    /// 문 여닫는 작동 설정.
    /// </summary>
    /// <param name="b_is_door_opening">문 작동 설정(Default = true)</param>
    void DoorOperate(bool b_is_door_opening = true)
    {
        this.b_is_door_opening = b_is_door_opening;
    }

    /// <summary>
    /// 다음 스테이지로 이동
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Main Moon Collision!");
        if (collision.gameObject.CompareTag("Player"))
        {
            FindObjectOfType<FloorManager>().NextStage(collision.gameObject, current_stage, next_stage);
        }
    }

    // 문 여는 과정을 실행
    public override void PrepareNextStageDoorOpened()
    {
        base.PrepareNextStageDoorOpened();
        DoorOperate(true);
    }
}
