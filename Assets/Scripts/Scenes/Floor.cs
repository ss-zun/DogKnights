using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floor : MonoBehaviour
{
    [Tooltip("해당 층에서 작동해야할 객체들")]
    public FloorController[] objects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 작동해야할 객체 실행
    /// </summary>
    void Operate()
    {
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].OperateObjects();
        }
    }
}
