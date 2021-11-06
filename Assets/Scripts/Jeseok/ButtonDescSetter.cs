using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonDescSetter : MonoBehaviour
{
    public TextMeshProSetter textMeshProSetter;
    [SerializeField]
    Color descColor;

    public void SetDesc(string desc)
    {
        if (textMeshProSetter == null)
            return;

        textMeshProSetter.SetText(desc);
        textMeshProSetter.SetColor(descColor);
    }
}
