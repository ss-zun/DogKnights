using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] players;
    // Start is called before the first frame update

    private void Awake()
    {
        for(int i=0; i<11; i++)
        {
            if (players[i]) players[i].SetActive(false);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
