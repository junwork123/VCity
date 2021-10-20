using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonEventManager : Singleton<UIButtonEventManager>
{
    public UIButtonClickListener[] clickListener;

    [HideInInspector]
    public bool activeActionButton;

    public GameObject minimap;


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
                OnClickActionButton();
                break;
            case UIButtonType.SETTING:
                ToggleSettingUI();
                break;
            case UIButtonType.MESSAGE:
                OnMessage();
                break;
            case UIButtonType.MAP:
                ToggleMinimapUI();
                break;
        }
    }

    public void OnClickActionButton()
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

    public void ToggleSettingUI()
    {
        AndroidToastManager.instance.ShowToast(System.Reflection.MethodBase.GetCurrentMethod().ToString());
        throw new System.NotImplementedException();
    }

    void OnMessage()
    {
        AndroidToastManager.instance.ShowToast(System.Reflection.MethodBase.GetCurrentMethod().ToString());
        throw new System.NotImplementedException();
    }

    void ToggleMinimapUI()
    {
        minimap.SetActive(!minimap.activeSelf);
    }
}
