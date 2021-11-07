using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{
    [Header("Other")]
    [SerializeField]
    RawImage otherVideoScreen;
    [Header("My")]
    [SerializeField]
    RawImage myVideoScreen;

    private void OnEnable()
    {
        VideoPlayer otehrVideo = VideoManager.instance.otherVideo;
        VideoPlayer myVideo = VideoManager.instance.myVideo;

        VideoManager.instance.PalyVideo(otehrVideo, otherVideoScreen);
        VideoManager.instance.PalyVideo(myVideo, myVideoScreen);
    }

    private void OnDisable()
    {
        VideoManager.instance.otherVideo.Stop();
        VideoManager.instance.myVideo.Stop();
    }
}
