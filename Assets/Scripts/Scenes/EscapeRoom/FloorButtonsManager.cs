using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorButtonsManager : MonoBehaviour
{
    // ===== public =====
    [Tooltip("버튼 객체들")]
    public FloorButton[] buttons;


    // ===== private =====

    // 현재 상호작용 버튼 개수
    private int counts = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach(FloorButton tempButton in buttons)
        {
            tempButton.SetFloorButtonsManager(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Tooltip("버튼과 플레이어의 상호작용이 발생했을 때, 상호작용한 버튼의 총 개수를 증가시킨다.")]
    public void ButtonCollisionCountIncrease()
    {
        counts++;
        Debug.Log(counts);
    }
}
