using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeFloor : MonoBehaviour
{

    [Tooltip("원래 위치로 이동할 곳")]
    public GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 땅바닥에 떨어졌을 경우 원래 위치로 이동
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = target.transform.position;
            collision.gameObject.transform.rotation = target.transform.rotation;
        }
    }
}
