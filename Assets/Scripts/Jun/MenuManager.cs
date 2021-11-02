using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    Stack<Menu> menuStack;
    [SerializeField] Menu[] menus;
    private void Awake()
    {
        Instance = this;
        menuStack = new Stack<Menu>();
    }

    public void OpenMenu(string menuName)
    {
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName == menuName)//string을 받아서 해당이름 가진 메뉴를 여는 스크립트
            {
                OpenMenu(menus[i]);
                menuStack.Push(menus[i]);
                return;
            }
            else if (menus[i].open)
            {
                CloseMenu(menus[i]);
            }
        }
    }

    public void OpenMenu(Menu menu)
    {
        menu.Open();
        menuStack.Push(menu);
        return;
    }

    public void CloseMenu(Menu menu)
    {
        if (menuStack.Peek() == menu)
        {
            menu.Close();
            menuStack.Pop();
        }
        else
        {
            Debug.Log("[Menu] : " + "가장 위에 열린 메뉴가 아닙니다");
        }

    }
    private void Update()
    {
        //안드로이드인 경우
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape)) //뒤로가기 키 입력
            {
                CloseMenu(menuStack.Peek());
                //처리할 내용
            }

        }
    }
}