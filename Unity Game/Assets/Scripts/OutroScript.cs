using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class OutroScript : MonoBehaviour
{
    [SerializeField] VideoPlayer myVideoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        myVideoPlayer.loopPointReached += OnOutroEnd;
    }

    void OnOutroEnd(VideoPlayer vp)
    {
        SceneManager.LoadSceneAsync(0);
    }
}