using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButton : MonoBehaviour
{
    // ===== public =====

    [Tooltip("버튼의 renderer")]
    public Renderer buttonRender;

    // ===== private =====

    // 버튼 관리자
    private FloorButtonsManager floorButtonsManager;

    // 상호작용을 한 번만 하도록 설정하기 위한 변수.
    private bool bIsOnce = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 버튼 관리자 설정
    /// </summary>
    /// <param name="floorButtonsManager_">해당 버튼에 넣을 버튼 관리자 객체</param>
    public void SetFloorButtonsManager(FloorButtonsManager floorButtonsManager_)
    {
        this.floorButtonsManager = floorButtonsManager_;
    }

    // 플레이어와의 충돌 감지
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌체의 태그가 "Player"이고 한 번만 작동하도록 설정.
        if (collision.gameObject.CompareTag("Player") && bIsOnce)
        {
            // 더 이상 상호작용하지 못하도록 방지
            bIsOnce = false;

            // 청록색으로 변경
            buttonRender.material.color = Color.cyan;
            buttonRender.material.SetColor("_EmissionColor", Color.cyan);

            // 상호작용 버튼 개수 증가
            floorButtonsManager.ButtonCollisionCountIncrease();
        }
    }
}
