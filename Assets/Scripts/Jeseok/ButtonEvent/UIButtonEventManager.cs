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

            case UIButtonType.QR_CHECKIN:
                OnClickQRCheckIn();
                break;
        }
    }

    public void OnClickAction()
    {
        activeActionButton = true;
        StartCoroutine(CheckAction());
    }

    // Interactionable 외부에서 Action 버튼 입력했을 때 한 프레임 이후에 해제
    IEnumerator CheckAction()
    {
        yield return null;
        activeActionButton = false;
    }

    public void OnClickQRCheckIn()
    {

    }
}
