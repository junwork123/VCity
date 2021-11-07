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
        StartCoroutine(WaitForPlay());
    }

    public void PuaseVideoCall()
    {
        StartCoroutine(WaitForPause(myVideo));
    }

    IEnumerator WaitForPlay()
    {
        yield return new WaitForSeconds(delayToPlay);

        otherVideo.Prepare();
        myVideo.Prepare();

        yield return new WaitUntil(() => otherVideo.isPrepared);
        yield return new WaitUntil(() => myVideo.isPrepared);

        otherVideoPanel.texture = otherVideo.texture;
        myVideoPanel.texture = myVideo.texture;

        otherVideo.Play();
        myVideo.Play();
    }

    IEnumerator WaitForPause(VideoPlayer videoPlayer)
    {
        yield return null;

        videoPlayer.Pause();
    }
}
