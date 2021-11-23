using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// VideoTexure is used to render local and remote video images
/// </summary>
public class VideoTexure : MonoBehaviour
{
    Renderer render;
    RawImage rawImage;
    public bool IsSelf;
    public int peerID;
    void Start()
    {
#if UNITY_EDITOR
        peerID = 2001;
#else
        peerID = 1001;
#endif
        render = GetComponent<Renderer>();
        rawImage = GetComponent<RawImage>();
        SetBlack();
        lastVideoTime = DateTime.Now;
    }

    float updateTime;
    DateTime lastVideoTime;
    void Update()
    {
        if (Time.time - updateTime < 0.03f)
        {
            return;
        }
        updateTime = Time.time;

        VideoInfo video;
        if (IsSelf)
        {
            video = UnityChatSDK.Instance.GetSelfTexture();
        }
        else
        {
            video = UnityChatSDK.Instance.GetPeerTexture(peerID);
        }

        if (video == null)
        {
            if ((DateTime.Now - lastVideoTime).TotalSeconds > 2)
            {
                SetBlack();
            }
            return;
        }
        else
        {
            lastVideoTime = video.LastTime;
        }

        if ((DateTime.Now - video.LastTime).TotalSeconds > 2)
        {
            SetBlack();
        }
        else
        {
            if (render != null)
            {
                render.material.mainTexture = video.Texture;
            }
            if (rawImage != null)
            {
                rawImage.texture = video.Texture; ;
            }
        }
    }

    private void OnDisable()
    {
        SetBlack();
    }
    void SetBlack()
    {
        if (render != null)
        {
            render.material.mainTexture = UnityChatSDK.Instance.TextureBlack;
        }
        if (rawImage != null)
        {
            rawImage.texture = UnityChatSDK.Instance.TextureBlack;
        }
    }
}
