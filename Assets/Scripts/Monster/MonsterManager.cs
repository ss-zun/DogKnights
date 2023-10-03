using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
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
    private float createTime; //몬스터를 발생시킬 주기

    private int totalMonsterCount;
    private int countTotem;
    private int countSlime;
    private int countTurtleShell;
    private int countGolem;
    private int countBoss;

    private int currentFloor; // 현재 몇 층인지
    private bool isGameOver = false; // 게임종료 여부

    public int TotalMonsterCount
    {
        get { return totalMonsterCount; }
    }

    private void Awake()
    {
        currentFloor = 0;
        MonsterSpawner(currentFloor);
    }
    private void Update()
    {
        isGameOver = GameManager.Instance.Player.isDead;
        if (currentFloor != getFloorState())
            MonsterSpawner(currentFloor);
    }

    private int getFloorState()
    {
        return 0;
    }

    void MonsterSpawner(int floor)
    {
        switch (floor)
        {
            case 0:
                totalMonsterCount = countTotem;
                break;
            case 1:
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

    IEnumerator CreateMonster()
    {
        while (!isGameOver)
        {
            //현재 생성된 몬스터 개수 산출
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("Monster").Length;

            if (monsterCount < totalMonsterCount)
            {
                //몬스터의 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(createTime);

                //불규칙적인 위치 산출
                //int idx = Random.Range(1, points.Length);
                //몬스터의 동적 생성
                //Instantiate(monsterPrefab, points[idx].position, points[idx].rotation);
            }
            else
            {
                yield return null;
            }
        }
    }
}
