using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;

    public TMP_InputField[] inputFields;
    public void Open()
    {
        open = true;
        gameObject.transform.SetAsLastSibling();
        gameObject.SetActive(true);//특정 메뉴 켜지기
    }

    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
    public bool IsCompleted()
    {
        foreach (TMP_InputField item in inputFields)
        {
            if (item.text == null || item.text == "")
            {
                return false;
            }
        }
        return true;
    }
}