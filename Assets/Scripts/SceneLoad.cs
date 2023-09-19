using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoad : MonoBehaviour
{
    [SerializeField]
    String targetSceneName;

    [SerializeField]
    Slider progressBar;

    void Start()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(targetSceneName);

        op.allowSceneActivation = false;
        while (!op.isDone)
        {
            yield return null;

            if (progressBar.value < 1.0f)
            {
                progressBar.value = Mathf.MoveTowards(progressBar.value, 1.0f, Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space) && progressBar.value >= 1.0f && op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
            }
        }
    }
}
