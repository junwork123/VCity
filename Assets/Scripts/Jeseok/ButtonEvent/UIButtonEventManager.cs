using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonEventManager : Singleton<UIButtonEventManager>
{
    public UIButtonClickListener[] clickListener;

    [HideInInspector]
    public bool activeActionButton;


    void Start()
    {
        clickListener = GetComponentsInChildren<UIButtonClickListener>();

        for (int i = 0; i < clickListener.Length; ++i)
            clickListener[i].AddClickCallback(UIEvent);
    }

    void UIEvent(UIButtonType uIButtonType)
    {
        switch (uIButtonType)
        {
            case UIButtonType.ACTION:
                OnClickAction();
                break;
            case UIButtonType.SETTING:
                OnClickSetting();
                break;
            case UIButtonType.MESSAGE:
                OnClickMessage();
                break;
            case UIButtonType.MAP:
                OnClickMap();
                break;
        }
    }

    public void OnClickAction()
    {
        activeActionButton = true;
        StartCoroutine(CheckAction());
        print("click action button");
    }

    // Interactionable 외부에서 Action 버튼 입력했을 때 한 프레임 이후에 해제
    IEnumerator CheckAction()
    {
        yield return null;
        activeActionButton = false;
    }

    public void OnClickSetting()
    {
        // AndroidToastManager.instance.ShowToast(System.Reflection.MethodBase.GetCurrentMethod().ToString());

        // GameManager.instance.SetResolution();
    }

    void OnClickMessage()
    {
        AndroidToastManager.instance.ShowToast(System.Reflection.MethodBase.GetCurrentMethod().ToString());
    }

    void OnClickMap()
    {
        UIManager.instance.ToggleMinimap();
    }
}
