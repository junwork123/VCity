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
        // create app if nonexistent
        if (ReferenceEquals(app, null))
        {
            app = new TestHelloUnityVideo(); // create app
            id = DataManager.Instance.videoCallInfo["id"];
            roomName = DataManager.Instance.videoCallInfo["roomName"];
            token = DataManager.Instance.videoCallInfo["token"];
            app.loadEngine(id); // load engine
        }

        // join channel and jump to next scene
        app.join(roomName, token);

        app.SetMyScreen(myScreen);
        app.SetOpScreen(opScreen);
        app.onSceneHelloVideoLoaded(); // call this after scene is loaded
        Debug.Log("영상 통화가 준비되었습니다");
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
        }
    }

    public void OnApplicationQuit()
    {
        if (!ReferenceEquals(app, null))
        {
            app.unloadEngine();
        }
    }
}
