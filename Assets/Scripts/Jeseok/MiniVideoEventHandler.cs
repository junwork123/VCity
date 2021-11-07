using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniVideoEventHandler : MonoBehaviour
{
    [SerializeField]
    RawImage miniVideoScreen;
    [SerializeField]
    VideoManager videoManager;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PlayMiniVideo());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator PlayMiniVideo()
    {
        yield return null;

        videoManager.PuaseVideoCall();

        miniVideoScreen.texture = videoManager.otherVideo.texture;
        // videoManager.otherVideo.Play();
    }

    IEnumerator CloseMiniVideo()
    {
        yield return null;

        videoManager.ResumeVideoCall();

        miniVideoScreen.texture = null;

    }
}
