using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MiniVideoHandler : MonoBehaviour
{
    [Header("Mini")]
    [SerializeField]
    RawImage miniVideoScreen;

    // Start is called before the first frame update
    void OnEnable()
    {
        VideoPlayer miniVideo = VideoManager.instance.miniVideo;

        VideoManager.instance.PlayVideo(miniVideo, miniVideoScreen);
    }

    private void OnDisable()
    {
        VideoManager.instance.miniVideo.Stop();
    }
}
