using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyButtonClickListener : MonoBehaviour, IButtonClickListener<ApplyType>
{
    [SerializeField]
    ApplyType _buttonType;
    public ApplyType buttonType { get => _buttonType; set => _buttonType = value; }
    public Action<ApplyType> onClickCallback { get; set; }

    public void AddClickCallback(Action<ApplyType> callback)
    {
        onClickCallback += callback;
    }

    public void OnClick()
    {
        if (onClickCallback == null)
            return;

        onClickCallback(buttonType);
    }

    public void SetButtonType(ApplyType type)
    {
        buttonType = type;
    }
}
