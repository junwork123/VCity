using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : Singleton<VideoManager>
{
    [Header("Other")]
    [SerializeField]
    VideoPlayer _otherVideo;
    public VideoPlayer otherVideo { get => _otherVideo; }
    [Header("Mini")]
    [SerializeField]
    VideoPlayer _miniVideo;
    public VideoPlayer miniVideo { get => _miniVideo; }

    [Header("My")]
    [SerializeField]
    VideoPlayer _myVideo;
    public VideoPlayer myVideo { get => _myVideo; }

    [Space(10f)]
    [SerializeField]
    float delayToPlay = 3f;

    public void PlayVideo(VideoPlayer videoPlayer, RawImage rawImage)
    {
        StartCoroutine(WaitForPlay(videoPlayer, rawImage));
    }

    IEnumerator WaitForPlay(VideoPlayer videoPlayer, RawImage rawImage)
    {
        yield return new WaitForSeconds(delayToPlay);

        videoPlayer.Prepare();
        yield return new WaitUntil(() => videoPlayer.isPrepared);
        rawImage.texture = videoPlayer.texture;
        videoPlayer.Play();
    }

    IEnumerator WaitForPause(VideoPlayer videoPlayer)
    {
        yield return null;

        videoPlayer.Pause();
    }
}
