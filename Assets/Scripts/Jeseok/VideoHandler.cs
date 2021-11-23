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

    public TestHome testHome;

    private void OnEnable()
    {
        // VideoPlayer otehrVideo = VideoManager.instance.otherVideo;
        // VideoPlayer myVideo = VideoManager.instance.myVideo;

        // VideoManager.instance.PlayVideo(otehrVideo, otherVideoScreen);
        // VideoManager.instance.PlayVideo(myVideo, myVideoScreen);
        testHome.onJoinButtonClicked();
    }

    private void OnDisable()
    {
        // VideoManager.instance.otherVideo.Stop();
        // VideoManager.instance.myVideo.Stop();
        testHome.OnApplicationQuit();
    }
}
