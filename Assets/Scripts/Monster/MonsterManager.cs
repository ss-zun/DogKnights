using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    static MonsterManager instance = null;

    [SerializeField]
    GameObject totem;
    [SerializeField]
    GameObject slime;
    [SerializeField]
    GameObject turtleShell;
    [SerializeField]
    GameObject golem;
    [SerializeField]
    GameObject boss;

    [SerializeField]
    public Transform[] points; //몬스터 스폰 위치를 담을 배열
    [SerializeField]
    private float createTime; //몬스터 스폰 주기

    private int totalMonsterCount;

    private int currentFloor; // 현재 몇 층인지
    private bool isGameOver = false; // 게임종료 여부

    public int TotalMonsterCount
    {
        get { return totalMonsterCount; }
    }
    public static MonsterManager Instance
    {
        get { return instance; }
    }

    private void Awake()
    {
        currentFloor = 0;
        MonsterSpawner(currentFloor);

        if (instance != null)
        {
            Debug.LogError("systemManager error");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Update()
    {
        //isGameOver = GameManager.Instance.Player.isDead;
        if (currentFloor != getFloorState())
            MonsterSpawner(currentFloor);
    }

    private int getFloorState()
    {
        return 0;
    }

    void MonsterSpawner(int floor)
    {
        GameObject[] monsterPrefab;
        switch (floor)
        {
            case 0:
                totalMonsterCount = 1;
                monsterPrefab = new GameObject[1];
                monsterPrefab[0] = totem;
                CreateMonster(monsterPrefab, totalMonsterCount);
                break;
            case 1:
                totalMonsterCount = 6;
                monsterPrefab = new GameObject[2];
                monsterPrefab[0] = slime;
                monsterPrefab[1] = turtleShell;
                CreateMonster(monsterPrefab, totalMonsterCount);
                break;
            case 3:
                break;
            case 5:
                break;
            case 6:
                break;
            case 8:
                break;
            case 10:
                break;
        }
    }

    IEnumerator CreateMonster(GameObject[] monsterPrefab, int totalMonsterCount)
    {
        while (!isGameOver)
        {
            //현재 생성된 몬스터 개수 산출
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("Monster").Length;

            if (monsterCount < totalMonsterCount)
            {
                //몬스터의 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(createTime);

                int idx = Random.Range(1, points.Length);
                int monster = Random.Range(0, monsterPrefab.Length);
                Instantiate(monsterPrefab[monster], points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }
}
