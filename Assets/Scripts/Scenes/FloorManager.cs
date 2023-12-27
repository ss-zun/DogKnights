using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{

    static FloorManager instance = null;
    // ===== public =====

    [Tooltip("층에 대한 정보를 모은 배열입니다.")]
    public GameObject[] floors;

    [Tooltip("플레이어가 해당 층으로 이동할 위치입니다.")]
    public GameObject[] targetPlayers;

    [Tooltip("다음 스테이지를 가기 전, 실행해야할 객체입니다.")]
    [SerializeField]
    private NextStageDoor[] nextDoorOperaters;
     

    public static FloorManager Instance
    {
        get { return instance; }
    }
    // ===== private =====

    // 현재 플레이어 위치
    private int currentPlayerFloor = 0;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("systemManager error");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(floors[0].transform.childCount);
        // floors[2].transform.GetChild(1).gameObject.SetActive(false);
        
        // 1F ~ 10F까지 비활성화
        for(int i = 1; i < floors.Length; i++)
        {
            // 층마다 자식 객체들 모두 비활성화
            for(int j = 0; j < floors[i].transform.childCount; j++)
            {
                floors[i].transform.GetChild(j).gameObject.SetActive(false);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 다음 스테이지로 이동할 때 호출합니다. 현재 스테이지의 맵을 Invisible로 설정합니다.
    /// </summary>
    public void NextStage(GameObject player, int currentStageNum, int nextStageNum)
    {
        ExploreTrap tempTrap;

        // 다음 스테이지 불투명으로 변경 (활성화)
        for(int i = 0; i < floors[nextStageNum].transform.childCount; i++)
        {
            floors[nextStageNum].transform.GetChild(i).gameObject.SetActive(true);
            // 탈출맵의 애셋인 경우 활성화
            tempTrap = floors[nextStageNum].transform.GetChild(i).gameObject.GetComponent<ExploreTrap>();
            if (tempTrap != null)
            {
                tempTrap.SetActivate(true);
            }
        }

        // 이전 스테이지 투명으로 변경 (비활성화)
        for(int i = 0; i < floors[currentStageNum].transform.childCount; i++)
        {
            floors[currentStageNum].transform.GetChild(i).gameObject.SetActive(false);
            // 탈출맵의 애셋인 경우 비활성화
            tempTrap = floors[currentStageNum].transform.GetChild(i).gameObject.GetComponent<ExploreTrap>();
            if (tempTrap != null)
            {
                tempTrap.SetActivate(false);
            }
        }

        // 탈출맵인 경우 플레이어 시점을 상단으로 변경
        if (nextStageNum == 2 || nextStageNum == 7)
        {
            player.GetComponent<Player>().isMapPuzzle = true;
        }
        // 탈출맵이 아닌 경우 플레이어 시점을 옆으로 변경
        else
        {
            player.GetComponent<Player>().isMapPuzzle = false;
        }

        // 플레이어 현재 속도 0으로 설정
        player.GetComponent<Rigidbody>().velocity = new Vector3(0.0f, 0.0f, 0.0f);

        SetCurrentPlayerFloor(nextStageNum);
        // 플레이어 다음 위치로 이동
        player.transform.position = targetPlayers[nextStageNum].transform.position;
        player.transform.rotation = targetPlayers[nextStageNum].transform.rotation;
    }

    // 현재 플레이어의 층 위치를 설정합니다.
    void SetCurrentPlayerFloor(int _floor)
    {
        currentPlayerFloor = _floor;
    }

    // 현재 플레이어의 층 위치를 가져옵니다.
    public int GetCurrentPlayerFloor()
    {
        return currentPlayerFloor;
    }

    // 다음 스테이지로 이동할 수 있는 준비 과정을 실행합니다.
    public void NextStageDoorOpened()
    {
        nextDoorOperaters[currentPlayerFloor].PrepareNextStageDoorOpened();
    }
}
