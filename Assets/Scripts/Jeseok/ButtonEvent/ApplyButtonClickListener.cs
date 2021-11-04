using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyButtonClickListener : ButtonClickListener<ApplyType>
{
    ButtonDescSetter descSetter;

    private void Start()
    {
        descSetter = GetComponent<ButtonDescSetter>();

        //descSetter.SetDesc(buttonType.ToString());
    }
}
