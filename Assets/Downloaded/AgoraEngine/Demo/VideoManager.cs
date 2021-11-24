using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif
using System.Collections;
using System;

/// <summary>
///    TestHome serves a game controller object for this application.
/// </summary>
public class VideoManager : Singleton<VideoManager>
{

    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static VideoHandler app = null;

    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    private string id;
    private string roomName;
    private string token;

    [SerializeField] RectTransform frontPanel;
    [SerializeField] RectTransform screenPanel;
    [SerializeField] RectTransform bottomPanel;
    [SerializeField] GameObject myScreen;
    [SerializeField] GameObject opScreen;

    Vector2 PORTRAIT_SCALE = new Vector2(320, 480);
    void Awake()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        permissionList.Add(Permission.Microphone);
        permissionList.Add(Permission.Camera);
#endif

        // keep this alive across scenes
        DontDestroyOnLoad(this.gameObject);
    }

    void Start()
    {
        screenPanel.gameObject.AddComponent<ScreenTouchHandler>();
        screenPanel.gameObject.AddComponent<agora_utilities.UIElementDragger>();
        screenPanel.GetComponent<ScreenTouchHandler>().mOnPointerUp += OnPointerUp;
        screenPanel.GetComponent<ScreenTouchHandler>().enabled = false;
        screenPanel.GetComponent<agora_utilities.UIElementDragger>().enabled = false;
        screenPanel.gameObject.SetActive(false);
        bottomPanel.gameObject.SetActive(false);
        RectTransform myRect = myScreen.GetComponent<RectTransform>();
        myRect.sizeDelta = PORTRAIT_SCALE;
    }

    void Update()
    {
        CheckPermissions();
    }

    public IEnumerator CheckAppId()
    {
        yield return StartCoroutine(DataManager.Instance.GetVideoCallInfo());
        id = DataManager.Instance.videoCallInfo["id"];
        roomName = DataManager.Instance.videoCallInfo["roomName"];
        token = DataManager.Instance.videoCallInfo["token"];
    }

    /// <summary>
    ///   Checks for platform dependent permissions.
    /// </summary>
    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
        foreach (string permission in permissionList)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Permission.RequestUserPermission(permission);
            }
        }
#endif
    }

    public void StartVideoCall()
    {
        id = DataManager.Instance.videoCallInfo["id"];
        roomName = DataManager.Instance.videoCallInfo["roomName"];
        token = DataManager.Instance.videoCallInfo["token"];
        // create app if nonexistent
        if (ReferenceEquals(app, null))
        {
            app = new VideoHandler(); // create app
            app.loadEngine(id); // load engine
            app.join(roomName, token);
            //app.SetScreenSize(1280, 720);
            app.myScreen = myScreen;
            app.opScreen = opScreen;
            app.onSceneHelloVideoLoaded(); // call this after scene is loaded
            Debug.Log("영상 통화가 준비되었습니다");

            screenPanel.gameObject.SetActive(true);
            bottomPanel.gameObject.SetActive(true);
        }
    }

    public void CloseCall()
    {
        if (!ReferenceEquals(app, null))
        {
            app.leave(); // leave channel
            app.unloadEngine(); // delete engine
            app = null; // delete app

            OnPointerUp();
            screenPanel.gameObject.SetActive(false);
            bottomPanel.gameObject.SetActive(false);
            frontPanel.gameObject.SetActive(false);
        }

    }

    public void OnBackButtonClicked()
    {
        bottomPanel.gameObject.SetActive(false);

        RectTransform myRect = myScreen.GetComponent<RectTransform>();
        myRect.position = new Vector3(-500, 500, 0);

        RectTransformExtensions.SetAnchor(screenPanel, AnchorPresets.MiddleCenter);
        screenPanel.sizeDelta = PORTRAIT_SCALE;

        screenPanel.GetComponent<ScreenTouchHandler>().enabled = true;
        screenPanel.GetComponent<agora_utilities.UIElementDragger>().enabled = true;

        //opScreen.GetComponentInChildren<Button>().enabled = true;
    }

    public void OnPointerUp()
    {
        screenPanel.GetComponent<ScreenTouchHandler>().enabled = false;
        screenPanel.GetComponent<agora_utilities.UIElementDragger>().enabled = false;

        RectTransformExtensions.SetAnchor(screenPanel, AnchorPresets.StretchAll);

        RectTransform myRect = myScreen.GetComponent<RectTransform>();
        RectTransformExtensions.SetAnchor(myRect, AnchorPresets.MiddleCenter);
        myRect.anchoredPosition = new Vector3(0, -325, 0);

        bottomPanel.gameObject.SetActive(true);
    }

    public void OnFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (!ReferenceEquals(app, null))
        {
            app.onSceneHelloVideoLoaded(); // call this after scene is loaded
        }
    }

    public void OnApplicationPause(bool paused)
    {
        if (!ReferenceEquals(app, null))
        {
            app.EnableVideo(paused);
            StartVideoCall();
        }
    }

    public void OnApplicationQuit()
    {
        if (!ReferenceEquals(app, null))
        {
            app.unloadEngine();
        }
    }
    // private void OnDisable()
    // {
    //     if (!ReferenceEquals(app, null))
    //     {
    //         app.leave(); // leave channel
    //         app.unloadEngine(); // delete engine
    //         app = null; // delete app
    //     }
    // }
    // private void OnEnable()
    // {
    //     if (!ReferenceEquals(app, null))
    //     {
    //         StartVideoCall();
    //     }
    // }
}
