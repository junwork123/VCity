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

    public void SetPlayerInfo()
    {
        UIManager.instance.InitPlayerInfo();
    }

    protected void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
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
