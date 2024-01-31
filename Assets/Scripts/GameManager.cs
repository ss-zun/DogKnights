 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// 게임 진행 규칙 관리 매니저
// 게임 오버 관리
// 점수 관리 등등..
public class GameManager : MonoBehaviour
{
    static GameManager instance = null;
    static public bool isGamePause = false;
    static public bool isEnding = false;
    public static GameManager Instance
    {
        get { return instance; }
    }

    void Awake(){
        if(instance != null){
            Debug.LogError("systemManager error");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    [SerializeField]
    Player player;

    [SerializeField]
    Player[] players;


    public Player Player
    {
        get { return player; }
    }

    public Player[] Players
    {
        get { return players; }
    }

    [SerializeField]
    CameraMove camera;

    public CameraMove Camera
    {
        get { return camera; }
    }

    // 게임 시작버튼 눌렀을 때
    public void OnClickStartButton()
    {
        // 0층 맵 활성화
        // 플레이어가 0층에 스폰됨
        // 토템이 0층에 스폰됨
        // 게임내 UI 활성화
        // 스토리 설명시작
    }

    public void EndGame()
    {
        isEnding = true;
        Debug.Log("****** Game Cleer! ******");
        SceneManager.LoadScene("Ending");
    }
}
