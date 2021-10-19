using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickListener : MonoBehaviour
{
    public InteractionType interactionType;
    Action<InteractionType> onClickCallback;


    public void SetInteractionType(InteractionType type)
    {
        interactionType = type;
    }

    public void AddClickCallback(Action<InteractionType> callback)
    {
        onClickCallback += callback;
    }

    public void OnClick()
    {
        if (onClickCallback == null)
            return;

        onClickCallback(interactionType);
    }
}
