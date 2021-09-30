using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    public string playerName;


    // 취소버튼 대기시간
    public float waitTimeQuitApp = 2f;
    bool isQuitWait;


    private void Awake() {
        
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
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    IEnumerator CheckQuitApplication()
    {
        yield return new WaitForSeconds(waitTimeQuitApp);
        isQuitWait = false;
    }
}
