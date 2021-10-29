using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickListener<T> : MonoBehaviour, IButtonClickListener<T>
{
    [SerializeField]
    T _buttonType;
    public T buttonType { get => _buttonType; set => _buttonType = value; }
    public Action<T> onClickCallback { get; set; }

    public void AddClickCallback(Action<T> callback)
    {
        onClickCallback += callback;
    }

    public void OnClick()
    {
        if (onClickCallback == null)
            return;

        onClickCallback(buttonType);
    }

    public void SetButtonType(T type)
    {
        buttonType = type;
    }
}
