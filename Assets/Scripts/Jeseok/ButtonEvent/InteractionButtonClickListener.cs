using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionButtonClickListener : MonoBehaviour, IButtonClickListener<InteractionType>
{
    [SerializeField]
    InteractionType _buttonType;
    public InteractionType buttonType { get => _buttonType; set => _buttonType = value; }
    public Action<InteractionType> onClickCallback { get; set; }

    public void AddClickCallback(Action<InteractionType> callback)
    {
        onClickCallback += callback;
    }

    public void OnClick()
    {
        if (onClickCallback == null)
            return;

        onClickCallback(buttonType);
    }

    public void SetButtonType(InteractionType type)
    {
        buttonType = type;
    }
}
