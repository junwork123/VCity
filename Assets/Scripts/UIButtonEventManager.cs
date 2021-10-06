using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonEventManager : Singleton<UIButtonEventManager>
{
    [HideInInspector]
    public bool activeActionButton;


    public void OnClickMenuButton()
    {
        // AndroidToastManager.instance.ShowToast("Menu Button");
    }

    public void OnClickActionButton()
    {
        // AndroidToastManager.instance.ShowToast("Action Button");

        activeActionButton = true;
        StartCoroutine(CheckAction());
    }

    public void OnClickSettingButton()
    {
        // AndroidToastManager.instance.ShowToast("Setting Button");
    }

    public void OnClickMapButton()
    {
        // AndroidToastManager.instance.ShowToast("Setting Button");
    }

    // Interactionable 외부에서 Action 버튼 입력했을 때 한 프레임 이후에 해제
    IEnumerator CheckAction()
    {
        yield return null;
        activeActionButton = false;
    }
}
