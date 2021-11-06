using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoCall : MonoBehaviour
{
    [SerializeField]
    VideoPlayer otherVideo;
    [SerializeField]
    RawImage otherVideoPanel;
    [SerializeField]
    VideoPlayer myVideo;
    [SerializeField]
    RawImage myVideoPanel;

    [SerializeField]
    float delayToPlay = 3f;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForPlay());
    }

    // Update is called once per frame
    void Update()
    {

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
}
