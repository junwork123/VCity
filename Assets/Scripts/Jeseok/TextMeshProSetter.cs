using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMeshProSetter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;

    [SerializeField]
    string defaultText;

    private void Start()
    {
    }

    public void SetText(string text)
    {
        textMeshProUGUI.text = text;
    }

    public void SetColor(Color color)
    {
        textMeshProUGUI.color = color;
    }
}
