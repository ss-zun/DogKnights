using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField]
    public float damage = 10f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(SystemManager.Instance.Player.anim.GetCurrentAnimatorStateInfo(0).IsName("Attack01") && other.CompareTag("Enemy")){
            Debug.Log("attacking");
            GameObject go = SystemManager.Instance.Player.GenerateEffect(1, other.transform.position);
            Destroy(go, 0.2f);

        }
    }
}
