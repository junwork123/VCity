using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public string playerName;
    public KeyCode InteractionKeyCode;

    // 취소버튼 대기시간
    public float waitTimeQuitApp = 2f;
    bool isQuitWait;

    public GameObject joystick;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
            // 취소 버튼 두번 눌렀을 경우 게임 종료
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isQuitWait == false)
                {
                    AndroidToastManager.instance.ShowToast("한번 더 누르시면 종료됩니다.");
                    StartCoroutine(CheckQuitApplication());
                    isQuitWait = true;
                }
                // isQuitWait : true
                else
                {
                    Application.Quit();
                }
            }
#endif

        #region 
        /// 조이스틱을 사용중일 때 커서가 버튼 위로 이동하면 조이스틱을 비활성화함
        if (joystick != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            LayerMask layerMask = LayerMask.GetMask("UI");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
            {
                joystick.SetActive(false);
            }
            else
                joystick.SetActive(true);
        }
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

    IEnumerator CheckQuitApplication()
    {
        yield return new WaitForSeconds(waitTimeQuitApp);
        isQuitWait = false;
    }
}
