using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MiniVideoHandler : MonoBehaviour
{
    [SerializeField]
    RawImage miniVideoScreen;

    // Start is called before the first frame update
    void OnEnable()
    {
        VideoPlayer otehrVideo = VideoManager.instance.otherVideo;

        VideoManager.instance.PalyVideo(otehrVideo, miniVideoScreen);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDisable()
    {
        VideoManager.instance.otherVideo.Stop();
        VideoManager.instance.myVideo.Stop();
    }
}
