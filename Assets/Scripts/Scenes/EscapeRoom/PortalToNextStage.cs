using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalToNextStage : NextStageDoor
{

    // ===== public =====
    // 현재 스테이지
    public int current_stage = 0;

    // 다음 스테이지
    public int next_stage = 0;

    // ===== private =====
    // 포탈이 활성화됐는지의 여부
    bool bIsPortalOn = false;

    // Start is called before the first frame update
    void Start()
    {
        if (current_stage != 2 || current_stage != 7)
        {
            PortalActivate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 포탈 이펙트 및 충돌 활성화
    /// </summary>
    public void PortalActivate()
    {
        ParticleSystem particleSystem = transform.Find("FX").gameObject.GetComponent<ParticleSystem>();
        bIsPortalOn = true;
        particleSystem.Play();
    }

    // 플레이어와 충돌했을 때
    private void OnCollisionEnter(Collision collision)
    {
        // 플레이어 태그가 부딪혔으며 포탈이 활성화되었을 때만 작동.
        if(collision.gameObject.CompareTag("Player") && bIsPortalOn)
        {
            // 다음 스테이지로 이동.
            FindObjectOfType<FloorManager>().NextStage(collision.gameObject, current_stage, next_stage);
            // Debug.Log("True");
            if(current_stage==4)
            {
                current_stage = 9;
                next_stage = 10;
            }
        }
    }

    // 포탈 활성화
    public override void PrepareNextStageDoorOpened()
    {
        base.PrepareNextStageDoorOpened();
        gameObject.SetActive(true);
    }
}
