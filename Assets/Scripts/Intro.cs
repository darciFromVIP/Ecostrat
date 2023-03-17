using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class Intro : MonoBehaviour
{
    private PlayableDirector director;
    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            director.time = director.duration - 0.1f;
        }
    }
}
