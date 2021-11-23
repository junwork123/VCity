using UnityEngine;
using UnityEngine.UI;

using agora_gaming_rtc;
using agora_utilities;


// this is an example of using Agora Unity SDK
// It demonstrates:
// How to enable video
// How to join/leave channel
// 
public class TestHelloUnityVideo
{

    // instance of agora engine
    private IRtcEngine mRtcEngine;
    public GameObject myScreen { get; set; }
    public GameObject opScreen { get; set; }
    public GameObject opScreenMini { get; set; }
    // load agora engine
    public void loadEngine(string appId)
    {
        // start sdk
        Debug.Log("initializeEngine");

        if (mRtcEngine != null)
        {
            Debug.Log("Engine exists. Please unload it first!");
            return;
        }

        // init engine
        mRtcEngine = IRtcEngine.GetEngine(appId);

        // enable log
        mRtcEngine.SetLogFilter(LOG_FILTER.DEBUG | LOG_FILTER.INFO | LOG_FILTER.WARNING | LOG_FILTER.ERROR | LOG_FILTER.CRITICAL);
    }

    public void join(string channel, string token)
    {
        Debug.Log("calling join (channel = " + channel + ")");

        if (mRtcEngine == null)
            return;

        // set callbacks (optional)
        mRtcEngine.OnJoinChannelSuccess = onJoinChannelSuccess;
        mRtcEngine.OnUserJoined = onUserJoined;
        mRtcEngine.OnUserOffline = onUserOffline;
        mRtcEngine.OnWarning = (int warn, string msg) =>
        {
            Debug.LogWarningFormat("Warning code:{0} msg:{1}", warn, IRtcEngine.GetErrorDescription(warn));
        };
        mRtcEngine.OnError = HandleError;

        // enable video
        mRtcEngine.EnableVideo();
        // allow camera output callback
        mRtcEngine.EnableVideoObserver();

        // join channel
        /*  This API Assumes the use of a test-mode AppID
             mRtcEngine.JoinChannel(channel, null, 0);
        */

        /*  This API Accepts AppID with token; by default omiting info and use 0 as the local user id */
        mRtcEngine.JoinChannelByKey(channelKey: token, channelName: channel);
    }

    public string getSdkVersion()
    {
        string ver = IRtcEngine.GetSdkVersion();
        return ver;
    }

    public void leave()
    {
        Debug.Log("calling leave");

        if (mRtcEngine == null)
            return;

        // leave channel
        mRtcEngine.LeaveChannel();
        // deregister video frame observers in native-c code
        mRtcEngine.DisableVideoObserver();
    }

    // unload agora engine
    public void unloadEngine()
    {
        Debug.Log("calling unloadEngine");

        // delete
        if (mRtcEngine != null)
        {
            IRtcEngine.Destroy();  // Place this call in ApplicationQuit
            mRtcEngine = null;
        }
    }

    public void EnableVideo(bool pauseVideo)
    {
        if (mRtcEngine != null)
        {
            if (!pauseVideo)
            {
                mRtcEngine.EnableVideo();
            }
            else
            {
                mRtcEngine.DisableVideo();
            }
        }
    }

    // accessing GameObject in Scnene1
    // set video transform delegate for statically created GameObject
    public void onSceneHelloVideoLoaded()
    {
        // Attach the SDK Script VideoSurface for video rendering
        if (ReferenceEquals(myScreen, null))
        {
            Debug.Log("failed to find Quad");
            return;
        }
        else
        {
            myScreen.AddComponent<VideoSurface>();
        }
    }

    void HandleHelp()
    {
#if UNITY_2020_3_OR_NEWER && PLATFORM_STANDALONE_OSX
        // this very easy to forget for MacOS
        HandleError(-2, "if you don't see any video, did you set the MacOS plugin bundle to AnyCPU?");
#else
        HandleError(-1, "if you don't see any video, please check README for help");
#endif
    }

    // implement engine callbacks
    private void onJoinChannelSuccess(string channelName, uint uid, int elapsed)
    {
        Debug.Log("JoinChannelSuccessHandler: uid = " + uid);
        // GameObject textVersionGameObject = GameObject.Find("VersionText");
        // textVersionGameObject.GetComponent<Text>().text = "SDK Version : " + getSdkVersion();
    }

    // When a remote user joined, this delegate will be called. Typically
    // create a GameObject to render video on it
    private void onUserJoined(uint uid, int elapsed)
    {
        Debug.Log("onUserJoined: uid = " + uid + " elapsed = " + elapsed);
        // this is called in main thread


        // create a GameObject and assign to this new user
        //VideoSurface videoSurface = makeImageSurface(uid);
        opScreen.AddComponent<VideoSurface>();
        VideoSurface videoSurface = opScreen.GetComponent<VideoSurface>();
        if (!ReferenceEquals(videoSurface, null))
        {
            Debug.Log("opScreen's Surface is Null");
            // configure videoSurface
            opScreen.name = uid.ToString();
            videoSurface.SetForUser(uid);
            videoSurface.SetEnable(true);
            videoSurface.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        }
        opScreenMini.AddComponent<VideoSurface>();
        VideoSurface videoSurface2 = opScreenMini.GetComponent<VideoSurface>();
        if (!ReferenceEquals(videoSurface, null))
        {
            // configure videoSurface
            opScreenMini.name = uid.ToString();
            videoSurface2.SetForUser(uid);
            videoSurface2.SetEnable(true);
            videoSurface2.SetVideoSurfaceType(AgoraVideoSurfaceType.RawImage);
        }
    }

    public void onShowMiniScreen()
    {
        VideoSurface videoSurface = opScreen.GetComponent<VideoSurface>();
        //opScreenMini.AddComponent<VideoSurface>(videoSurface);
    }

    // public VideoSurface makePlaneSurface(string goName)
    // {
    //     GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);

    //     if (go == null)
    //     {
    //         return null;
    //     }
    //     go.name = goName;
    //     // set up transform
    //     go.transform.Rotate(-90.0f, 0.0f, 0.0f);
    //     float yPos = Random.Range(3.0f, 5.0f);
    //     float xPos = Random.Range(-2.0f, 2.0f);
    //     go.transform.position = new Vector3(xPos, yPos, 0f);
    //     go.transform.localScale = new Vector3(0.25f, 0.5f, .5f);

    //     // configure videoSurface
    //     VideoSurface videoSurface = go.AddComponent<VideoSurface>();
    //     return videoSurface;
    // }

    private const float Offset = 100;
    // public VideoSurface makeImageSurface(uint uid)
    // {
    //     GameObject go;
    //     if (uid == myUid) go = myScreen;
    //     else go = opScreen;

    //     go.name = uid.ToString();
    //     // to be renderered onto
    //     go.AddComponent<RawImage>();
    //     // make the object draggable
    //     go.AddComponent<UIElementDragger>();
    //     // configure videoSurface
    //     VideoSurface videoSurface = go.AddComponent<VideoSurface>();
    //     return videoSurface;
    // }
    // When remote user is offline, this delegate will be called. Typically
    // delete the GameObject for this user
    private void onUserOffline(uint uid, USER_OFFLINE_REASON reason)
    {
        // remove video stream
        Debug.Log("onUserOffline: uid = " + uid + " reason = " + reason);
        // this is called in main thread
        GameObject go = GameObject.Find(uid.ToString());
        if (!ReferenceEquals(go, null))
        {
            Object.Destroy(go);
        }
    }

    #region Error Handling
    private int LastError { get; set; }
    private void HandleError(int error, string msg)
    {
        if (error == LastError)
        {
            return;
        }

        if (string.IsNullOrEmpty(msg))
        {
            msg = string.Format("Error code:{0} msg:{1}", error, IRtcEngine.GetErrorDescription(error));
        }

        switch (error)
        {
            case 101:
                msg += "\nPlease make sure your AppId is valid and it does not require a certificate for this demo.";
                break;
        }

        LastError = error;
    }

    #endregion
}
