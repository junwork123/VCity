using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Photon.Pun;

public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public KeyCode InteractionKeyCode;

    [Header("Player")]
    public GameObject player;
    public GameObject joystickRange;
    public string playerName;

    [Header("Task")]
    public Transform teleportPosition;

    [Header("System")]
    // 취소버튼 대기시간
    public float delayToQuitApp = 2f;

    bool isQuitWait;


    private void Start()
    {
        UIManager.instance.SetPlayerName();
        //PhotonNetwork.Instantiate("RecordManager", Vector3.zero, Quaternion.identity, 0);
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

        #region joystick
        // /// 조이스틱을 사용중일 때 커서가 버튼 위로 이동하면 조이스틱을 비활성화함
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // RaycastHit hit;
        // LayerMask layerMask = LayerMask.GetMask("UI");
        // if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        // {
        //     // DEBUG
        //     // AndroidToastManager.instance.ShowToast("Disable Joystick");
        //     joystickRange.SetActive(false);


        //     // if (Input.GetMouseButton(0))
        //     //     hit.collider.GetComponent<ButtonClickListener>().OnClick();
        // }
        // else
        //     joystickRange.SetActive(true);
        #endregion

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
