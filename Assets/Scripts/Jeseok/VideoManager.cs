using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    [Header("Other")]
    [SerializeField]
    VideoPlayer _otherVideo;
    public VideoPlayer otherVideo { get => _otherVideo; }
    [SerializeField]
    RawImage otherVideoPanel;

    [Header("My")]
    [SerializeField]
    VideoPlayer myVideo;
    [SerializeField]
    RawImage myVideoPanel;

    [Space(10f)]
    [SerializeField]
    float delayToPlay = 3f;


    // Update is called once per frame
    void Update()
    {

    }

    public void StartVideoCall()
    {
        
    }

    public void PuaseVideoCall()
    {
        StartCoroutine(WaitForPause(myVideo));
    }

    public void ResumeVideoCall()
    {
        StartCoroutine(WaitForPlay(otherVideo, otherVideoPanel));
        StartCoroutine(WaitForPlay(myVideo, myVideoPanel));
    }

    IEnumerator WaitForPlay(VideoPlayer videoPlayer, RawImage panel)
    {
        yield return new WaitForSeconds(delayToPlay);

        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        panel.texture = otherVideo.texture;
        videoPlayer.Play();
    }

    IEnumerator WaitForPause(VideoPlayer videoPlayer)
    {
        yield return null;

        videoPlayer.Pause();
    }
}
