using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class Wait : MonoBehaviour
{
    public VideoPlayer videoplayer;
    public int sceneIndex;

    void Start()
    {
        
        StartCoroutine(WaitforVideoEnd());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Game play");
        }
    }

    IEnumerator WaitforVideoEnd()
    {   
        yield return new WaitForSeconds(41);
        SceneManager.LoadScene("Game play");
    }
}