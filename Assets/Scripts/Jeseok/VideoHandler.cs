using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoHandler : MonoBehaviour
{

    public TestHome testHome;

    private void OnEnable()
    {
        testHome.onJoinButtonClicked();
    }

    private void OnDisable()
    {
        testHome.onLeaveButtonClicked();
    }
}
