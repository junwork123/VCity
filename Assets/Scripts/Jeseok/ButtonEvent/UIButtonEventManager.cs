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

    public void OnClickSettingButton()
    {

    }

    public void OnClickMapButton()
    {

    }

    public void OnClickApplyButton()
    {
        
    }
}
