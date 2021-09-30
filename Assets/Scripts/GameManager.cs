using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // 취소버튼 대기시간
    public float waitTimeQuitApp = 2f;
    bool isQuitWait;

    void Update()
    {
#if UNITY_ANDROID
        // 백버튼을 눌렀을 경우 게임 종료
        if (Application.platform == RuntimePlatform.Android)
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isQuitWait == false)
                {
                    AndroidToast.instance.ShowToast("한번 더 누르시면 종료됩니다.");
                    StartCoroutine(CheckQuitApplication());
                    isQuitWait = true;
                }
                else
                {
                    Application.Quit();
                }
            }
#endif
    }

    IEnumerator CheckQuitApplication()
    {
        yield return new WaitForSeconds(waitTimeQuitApp);
        isQuitWait = false;
    }
}
