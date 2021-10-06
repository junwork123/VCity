using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : Singleton<GameManager>
{
    public string playerName;
    public KeyCode InteractionKeyCode;

    // 취소버튼 대기시간
    public float waitTimeQuitApp = 2f;
    bool isQuitWait;

    public GameObject player;
    public GameObject joystick;

    public GameObject centerPanel;
    public Transform teleportPosition;


    private void Start() {
        HideCenterPanel();
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
        /// 조이스틱을 사용중일 때 커서가 버튼 위로 이동하면 조이스틱을 비활성화함
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("UI");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // DEBUG
            // AndroidToastManager.instance.ShowToast("Disable Joystick");
            joystick.SetActive(false);

            if (Input.GetMouseButton(0))
                hit.collider.GetComponent<ButtonClickListener>().OnClick();
        }
        else
            joystick.SetActive(true);
        #endregion

    }


    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public void SetInteractionKey(KeyCode keyCode)
    {
        InteractionKeyCode = keyCode;
    }

    public void ShowCenterPanel()
    {
        centerPanel.SetActive(true);
    }

    public void HideCenterPanel()
    {
        centerPanel.SetActive(false);
    }

    public void TeleportPlayer()
    {
        player.transform.position = teleportPosition.position;
    }

    IEnumerator CheckQuitApplication()
    {
        yield return new WaitForSeconds(waitTimeQuitApp);
        isQuitWait = false;
    }
}
