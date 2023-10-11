using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] players;
    private Player player;

    
    // Start is called before the first frame update

    private void Awake()
    {
        //for (int i=0; i<11; i++)
        //{
        //    if (players[i]) players[i].SetActive(false);
        //}
    }
    void Start()
    {
    
        player = GameManager.Instance.Player;

        //FloorManager.Instance.NextStage(player.gameObject, 0, 0); 
        player.transform.position = FloorManager.Instance.targetPlayers[0].transform.position;
        player.transform.rotation = FloorManager.Instance.targetPlayers[0].transform.rotation;

    }

    // Update is called once per frame
    void Update()
    {
        checkMonster();
        //complete();
    }

    void complete()
    { 
        //Debug.Log("PlayerManager Test...");
        if (player.isComplete)
        {
            FloorManager.Instance.NextStage(player.gameObject, player.curFloorNum, player.curFloorNum + 1);
            player.curFloorNum += 1;
            player.isComplete = false;
        }
    }

    void checkMonster()
    {
        if(player.curFloorNum!=0 && MonsterManager.Instance.TotalMonsterCount == 0)
        {
            player.isComplete = true;
            complete();
        }
    }
}
