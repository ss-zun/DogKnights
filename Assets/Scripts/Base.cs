using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    protected int currentHp = 100;
    [SerializeField]
    protected int maxHp = 100;
    public int HPMax{
        get{
            return maxHp;
        }
    }
    protected int currentMp = 100;
    
    [SerializeField]
    protected int MaxMp = 100;
    public int MPMax{
        get{
            return MaxMp;
        }
    }
    
    [SerializeField]
    protected float moveSpeed = 5.0f;
    
    [SerializeField]
    protected float attackSpeed = 5.0f;
    
    [SerializeField]
    protected int attackPower = 5;
    protected Vector2 moveDirection ;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
