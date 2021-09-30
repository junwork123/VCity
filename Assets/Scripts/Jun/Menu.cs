using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string menuName;
    public bool open;
    public void Open()
    {
        open = true;
        gameObject.SetActive(true);//특정 메뉴 켜지기
    }

    public void Close()
    {
        open = false;
        gameObject.SetActive(false);
    }
}