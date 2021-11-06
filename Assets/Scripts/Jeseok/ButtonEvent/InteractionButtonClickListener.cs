using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionButtonClickListener : ButtonClickListener<InteractionType>
{
    ButtonDescSetter descSetter;

    private void Start()
    {
        descSetter = GetComponent<ButtonDescSetter>();

        // descSetter.SetDesc(buttonType.ToString());
    }
}
