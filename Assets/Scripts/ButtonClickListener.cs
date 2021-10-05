using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickListener : MonoBehaviour
{
    int buttonId = -1;
    Action<int> onClickCallback;

    public void SetButtonID(int id)
    {
        buttonId = id;
    }

    public void AddClickCallback(Action<int> callback)
    {
        onClickCallback += callback;
    }

    void OnClick()
    {
        if (onClickCallback == null)
            return;

        onClickCallback(buttonId);
    }
}
