using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    [SerializeField] private GameObject totem;
    [SerializeField] private GameObject slime;
    [SerializeField] private GameObject turtleShell;
    [SerializeField] private GameObject golem;
    [SerializeField] private GameObject boss;

    //몬스터 스폰 위치를 담을 배열
    public Transform[] pointsFloor0;
    public Transform[] pointsFloor1;
    public Transform[] pointsFloor3;
    public Transform[] pointsFloor5;
    public Transform[] pointsFloor6;
    public Transform[] pointsFloor8;
    public Transform[] pointsFloor10;

    // Dictionary를 사용하여 층마다 스폰 위치를 관리
    Dictionary<int, Transform[]> floorSpawnPoints = new Dictionary<int, Transform[]>();

    //몬스터 스폰 주기
    [SerializeField]
    private float spwanTime = 0.1f;

    private int totalMonsterCount; // 남아있는 총 몬스터수

    private int currentFloor; // 현재 몇 층인지
    private bool isChangeFloor = false; // 현재 층이 바꼈는지
    private bool isGameOver = false; // 게임종료 여부

    public FloorManager floorManager;
    public Player player;

    public int TotalMonsterCount
    {
        get { return totalMonsterCount; }
    }

    private void Start()
    {
        currentFloor = 0;
        MonsterSpawner(currentFloor);

        // 각 층에 대한 스폰 위치 배열을 Dictionary에 추가
        floorSpawnPoints[0] = pointsFloor0;
        floorSpawnPoints[1] = pointsFloor1;
        floorSpawnPoints[3] = pointsFloor3;
        floorSpawnPoints[5] = pointsFloor5;
        floorSpawnPoints[6] = pointsFloor6;
        floorSpawnPoints[8] = pointsFloor8;
        floorSpawnPoints[10] = pointsFloor10;
    }
    private void Update()
    {
        isGameOver = player.isDead;
        int newFloor = floorManager.GetCurrentPlayerFloor();
        if (currentFloor != newFloor)
        {
            currentFloor = newFloor;
            isChangeFloor = true;
        }
        if (isChangeFloor)
        {
            MonsterSpawner(currentFloor);
            isChangeFloor = false;
        }
        if (isGameOver)
        {
            StopAllCoroutines();
        }
    }

    void MonsterSpawner(int floor)
    {
        int totalMonsterCount = 0;
        GameObject[] monsterPrefabs = null;

        if (floorSpawnPoints.TryGetValue(floor, out Transform[] spawnPoints))
        {
            switch (floor)
            {
                case 0:
                    totalMonsterCount = 1;
                    monsterPrefabs = new GameObject[] { totem };
                    break;
                case 1:
                    totalMonsterCount = 4;
                    monsterPrefabs = new GameObject[] { slime };
                    break;
                case 3:
                    totalMonsterCount = 9;
                    monsterPrefabs = new GameObject[] { slime, golem, turtleShell };
                    break;
                case 5:
                    totalMonsterCount = 1;
                    monsterPrefabs = new GameObject[] { boss };
                    break;
                case 6:
                    totalMonsterCount = 3;
                    monsterPrefabs = new GameObject[] { golem };
                    break;
                case 8:
                    totalMonsterCount = 8;
                    monsterPrefabs = new GameObject[] { golem, turtleShell };
                    break;
                case 10:
                    totalMonsterCount = 1;
                    monsterPrefabs = new GameObject[] { boss };
                    break;
            }

            if (monsterPrefabs != null && spawnPoints != null)
            {
                StartCoroutine(CreateMonster(monsterPrefabs, totalMonsterCount, spawnPoints));
            }
        }
    }

    IEnumerator CreateMonster(GameObject[] monsterPrefab, int totalMonsterCount, Transform[] points)
    {
        Debug.Log("CreateMonster coroutine started.");
        while (!isGameOver)
        {
            //현재 생성된 몬스터 개수 산출
            int monsterCount = (int)GameObject.FindGameObjectsWithTag("Monster").Length;

            if (monsterCount < totalMonsterCount)
            {
                //몬스터의 생성 주기 시간만큼 대기
                yield return new WaitForSeconds(spwanTime);

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
