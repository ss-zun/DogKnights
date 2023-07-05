using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeRoomFloorTrap : MonoBehaviour
{
    // ===== public =====

    [Tooltip("바닥 트랩 객체를 담는 변수")]
    public GameObject trap;

    [Tooltip("트랩이 들어가는/올라오는 최소 시간")]
    public float minMoveTimes = 0.3f;

    [Tooltip("트랩이 들어가는/올라오는 최대 시간")]
    public float maxMoveTimes = 0.9f;

    [Tooltip("트랩 최소 잠복 시간")]
    public float minHiddenTimes = 1.5f;

    [Tooltip("트랩 최대 잠복 시간")]
    public float maxHiddenTimes = 4.0f;

    [Tooltip("트랩 최소 돌출 시간")]
    public float minProtrusionTimes = 1.2f;

    [Tooltip("트랩 최대 돌출 시간")]
    public float maxPortrusionTimes = 2.25f;

    // ===== private =====

    // 트랩 잠복 시간 ( 최소 잠복 시간과 최대 잠복 시간 사이에서 랜덤값으로 지정. )
    private float hiddenTimes = 0f;

    // 트랩 돌출 시간 ( 최소 돌출 시간과 최대 돌출 시간 사이에서 랜덤값으로 지정. )
    private float protrusionTimes = 0f;

    // 트랩 이동 시간 ( 최소 이동 시간과 최대 이동 시간 사이엥서 랜덤값으로 지정. )
    private float moveTimes = 0f;

    // 유지 시간
    private float maintainenceTime = 0f;

    // 현재 들어가는/올라오는 중 어느 과정인지 확인
    private bool bIsLiftingOff = false;

    // 현재 상태를 유지하는지 확인하는 변수
    private bool bIsMaintainence = false;

    // 현재 트랩 높이
    private float height = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        // 랜덤값으로 초기화
        hiddenTimes = Random.Range(minHiddenTimes, maxHiddenTimes);
        protrusionTimes = Random.Range(minProtrusionTimes, maxPortrusionTimes);
        moveTimes = Random.Range(minMoveTimes, maxMoveTimes);
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 상태 유지일 때
        if(bIsMaintainence)
        {
            // 유지 시간이 돌출 또는 잠복 시간보다 더 길 때
            if((bIsLiftingOff ? protrusionTimes : hiddenTimes) < maintainenceTime)
            {
                // 유지 상태 해제
                bIsMaintainence = false;

                // 상태 유지 시간 초기화
                maintainenceTime = 0f;

                // 추후 과정을 반대로 설정.
                bIsLiftingOff = !bIsLiftingOff;
            }

            else
            {
                maintainenceTime += Time.deltaTime;
            }

        }

        // 현재 상태 유지가 아닐 때
        else
        {
            // 현재 올라오는 중이라면
            if(bIsLiftingOff)
            {
                // 트랩이 최대 높이까지 올라왔다면
                if(height > 0.0f)
                {
                    // 높이 고정
                    height = 0.0f;

                    // 돌출 상태로 유지
                    bIsMaintainence = true;
                }
                // 트랩이 최대 높이까지 올라오지 않았다면
                else
                {
                    // 높이 올리기
                    height += 0.06f / moveTimes * Time.deltaTime;
                }
                // 위치 조절
                trap.transform.localPosition = new Vector3(trap.transform.localPosition.x, height, trap.transform.localPosition.z);
            }

            // 현재 들어가는 중이라면
            else
            {
                // 트랩이 최대 높이까지 올라왔다면
                if (height < -0.3f)
                {
                    // 높이 고정
                    height = -0.3f;

                    // 들어간 상태로 유지
                    bIsMaintainence = true;
                }
                // 트랩이 최소 높이까지 내려가지 않았다면
                else
                {
                    // 높이 내리기
                    height -= 0.06f / moveTimes * Time.deltaTime;
                }
                // 위치 조절
                trap.transform.localPosition = new Vector3(trap.transform.localPosition.x, height, trap.transform.localPosition.z);
            }
        }
    }
}
