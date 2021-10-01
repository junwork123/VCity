using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventManager : MonoBehaviour
{
    public static ButtonEventManager instance;


    public PlayerController playerController;

    public bool activeActionButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OnClickMenuButton()
    {
        AndroidToastManager.instance.ShowToast("Menu Button");
    }

    public void OnClickActionButton()
    {
        AndroidToastManager.instance.ShowToast("Action Button");

        activeActionButton = true;
        StartCoroutine(CheckAction());
    }

    public void OnClickSettingButton()
    {
        AndroidToastManager.instance.ShowToast("Setting Button");
    }

    // 외부에서 Action 버튼 입력했을 때 한 프레임 이후에 해제
    IEnumerator CheckAction()
    {
        yield return null;
        activeActionButton = false;
    }
}
