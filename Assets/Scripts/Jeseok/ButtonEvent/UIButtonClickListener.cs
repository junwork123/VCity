using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonClickListener : MonoBehaviour, IButtonClickListener<UIButtonType>
{
    [SerializeField]
    UIButtonType _buttonType;
    public UIButtonType buttonType { get => _buttonType; set => _buttonType = value; }
    public Action<UIButtonType> onClickCallback { get; set; }

    public void AddClickCallback(Action<UIButtonType> callback)
    {
        onClickCallback += callback;
    }

    public void OnClick()
    {
        if (onClickCallback == null)
            return;

        onClickCallback(buttonType);
    }

    public void SetButtonType(UIButtonType type)
    {
        buttonType = type;
    }
}
