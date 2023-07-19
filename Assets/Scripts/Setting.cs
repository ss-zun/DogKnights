using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Setting : MonoBehaviour
{
    static Setting instance;

    void Awake()
    {
        if (instance != null) {
            Destroy(gameObject);
        }

        instance = this;
    }

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnClickBack()
    {
        SceneManager.LoadScene("Intro");
    } 
}
