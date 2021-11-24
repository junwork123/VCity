using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
#if(UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
using UnityEngine.Android;
#endif
using System.Collections;

/// <summary>
///    TestHome serves a game controller object for this application.
/// </summary>
public class TestHome : MonoBehaviour
{

    // Use this for initialization
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList();
#endif
    static TestHelloUnityVideo app = null;

    // PLEASE KEEP THIS App ID IN SAFE PLACE
    // Get your own App ID at https://dashboard.agora.io/
    private string id;
    private string roomName;
    private string token;

    public GameObject myScreen;
    public GameObject opScreen;
    public GameObject opScreenMini;

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
        //StartCoroutine(CheckAppId());
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

    public void onJoinButtonClicked()
    {
        id = DataManager.Instance.videoCallInfo["id"];
        roomName = DataManager.Instance.videoCallInfo["roomName"];
        token = DataManager.Instance.videoCallInfo["token"];


        StartVideoCall();
    }
    public void StartVideoCall()
    {
        // create app if nonexistent
        if (ReferenceEquals(app, null))
        {
            app = new TestHelloUnityVideo(); // create app
            app.loadEngine(id); // load engine
            app.join(roomName, token);
            //app.SetScreenSize(1280, 720);
            app.myScreen = myScreen;
            app.opScreen = opScreen;
            app.opScreenMini = opScreenMini;
            app.onSceneHelloVideoLoaded(); // call this after scene is loaded
            Debug.Log("영상 통화가 준비되었습니다");
        }
    }

    public void onLeaveButtonClicked()
    {
        if (!ReferenceEquals(app, null))
        {
            app.leave(); // leave channel
            app.unloadEngine(); // delete engine
            app = null; // delete app
        }
        Destroy(gameObject);
    }

    public void OnBackButtonClicked()
    {
        RectTransform myRect = myScreen.GetComponent<RectTransform>();
        myRect.position = new Vector3(-500, 500, 0);

        RectTransform opRect = opScreen.GetComponent<RectTransform>();
        RectTransformExtensions.SetAnchor(opRect, AnchorPresets.MiddleCenter, 480, 320, 1);
    }

    public void OnMiniScreenTouched()
    {
        RectTransform myRect = myScreen.GetComponent<RectTransform>();
        myRect.position = new Vector3(0, -325, 0);

        RectTransform opRect = opScreen.GetComponent<RectTransform>();
        RectTransformExtensions.SetAnchor(opRect, AnchorPresets.StretchAll);
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
