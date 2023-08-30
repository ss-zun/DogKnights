using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    // ===== public =====

    [Tooltip("���� ���� ������ ���� �迭�Դϴ�.")]
    public GameObject[] floors;

    [Tooltip("�÷��̾ �ش� ������ �̵��� ��ġ�Դϴ�.")]
    public GameObject[] targetPlayers;

    // ===== private =====

    // Start is called before the first frame update
    void Start()
    {
        // Debug.Log(floors[0].transform.childCount);
        // floors[2].transform.GetChild(1).gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// ���� ���������� �̵��� �� ȣ���մϴ�. ���� ���������� ���� Invisible�� �����մϴ�.
    /// </summary>
    public void NextStage(GameObject player, int currentStageNum, int nextStageNum)
    {
        // ���� �������� ���������� ���� (Ȱ��ȭ)
        for(int i = 0; i < floors[nextStageNum].transform.childCount; i++)
        {
            floors[nextStageNum].transform.GetChild(i).gameObject.SetActive(true);
        }

        // ���� �������� �������� ���� (��Ȱ��ȭ)
        for(int i = 0; i < floors[currentStageNum].transform.childCount; i++)
        {
            floors[currentStageNum].transform.GetChild(i).gameObject.SetActive(false);
        }

        // �÷��̾� ���� ��ġ�� �̵�
        player.transform.position = targetPlayers[nextStageNum].transform.position;
        player.transform.rotation = targetPlayers[nextStageNum].transform.rotation;
    }

}