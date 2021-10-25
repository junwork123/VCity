using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public KeyCode InteractionKeyCode;

    [Header("Player")]
    public GameObject player;
    public GameObject joystickRange;
    public string playerType;
    public string playerName;

    [Header("Task")]
    public Transform teleportPosition;

    [Header("System")]
    // 취소버튼 대기시간
    public float delayToQuitApp = 2f;

    bool isQuitWait;


    protected void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        SetResolution();

        UIManager.instance.InitPlayerInfo();
    }

    void Update()
    {
        // 취소 버튼 두번 눌렀을 경우 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isQuitWait == false)
            {
                AndroidToastManager.instance.ShowToast("한번 더 누르시면 종료됩니다.");
                StartCoroutine(CheckQuitApplication());
                isQuitWait = true;
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
            }
        }
    }

    public void SetResolution()
    {
        AndroidToastManager.instance.ShowToast(System.Reflection.MethodBase.GetCurrentMethod().ToString());
        float setWidth = 1080;
        float setHeight = 1920;

        int deviceWidth = Screen.width;
        int deviceHeight = Screen.height;

        // Screen.SetResolution(deviceWidth, (int)((deviceWidth * setHeight) / setWidth), true);

        Rect rect = Camera.main.rect;
        float scaleHeight = ((float)Screen.width / Screen.height) / ((float)9 / 16);
        float scaleWidth = 1f / scaleHeight;

        if (scaleHeight < 1)
        {
            rect.height = scaleHeight;
            rect.y = (1f - scaleHeight) / 2f;
        }
        else
        {
            rect.width = scaleWidth;
            rect.x = (1f - scaleWidth) / 2f;
        }

        Camera.main.rect = rect;
    }

    void OnPreCul() => GL.Clear(true, true, Color.black);

    public void SetInteractionKey(KeyCode keyCode)
    {
        InteractionKeyCode = keyCode;
    }

    public void TeleportPlayer()
    {
        player.transform.position = teleportPosition.position;
    }

    IEnumerator CheckQuitApplication()
    {
        yield return new WaitForSeconds(delayToQuitApp);
        isQuitWait = false;
    }
}
