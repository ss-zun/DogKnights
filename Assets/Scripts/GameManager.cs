using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

     static GameManager instance = null;

     public static GameManager Instance
    {
        get{return instance;}
     }

     [SerializeField]
     CameraMove camera;

     public CameraMove  Camera{
          get{return camera;}
     }

     [SerializeField]
     Player player;
     
     public Player Player{
          get{return player;}
     }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Awake(){
        if(instance != null){
            Debug.LogError("systemManager error");
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
}
