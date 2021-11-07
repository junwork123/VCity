using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Menu[] menus;
    [SerializeField] Stack<Menu> menuStack;
    [SerializeField] GameObject mainPanel;
    private void Awake()
    {
        menuStack = new Stack<Menu>();
        CloseAllMenus();
    }

    public void OpenMenu(string menuName)
    {
        if (menuName.Equals("")) return;
        for (int i = 0; i < menus.Length; i++)
        {
            if (menus[i].menuName.Equals(menuName))//string을 받아서 해당이름 가진 메뉴를 여는 스크립트
            {
                OpenMenu(menus[i]);
                return;
            }
        }
    }

    public void OpenMenu(Menu menu = null)
    {
        // 전달된 메뉴가 없다면
        // 가장 첫번째 메뉴를 열기로 한다.
        if (menu == null) menu = menus[0];

        // 메뉴 스택이 Null이면 만들어준다.
        if (menuStack == null) menuStack = new Stack<Menu>();

        // 첫 번째 메뉴를 열게 된다면 먼저 메인 패널을 활성화한다.
        if (mainPanel.activeSelf != true)
        {
            mainPanel.SetActive(true);
        }

        // 메뉴를 열고 스택에 저장한다.
        menu.Open();
        menuStack.Push(menu);
    }

    public void OpenMenuSelf

    public void CloseMenu(Menu menu)
    {
        if (menuStack.Count > 0)
        {
            if (menuStack.Peek() == menu)
            {
                menu.Close();
                menuStack.Pop();
            }
            else
                Debug.Log("[Menu] : " + "가장 위에 열린 메뉴가 아닙니다");

            // 마지막 메뉴라면 메인 패널도 닫는다.
            if (menuStack.Count == 0) mainPanel.gameObject.SetActive(false);
        }
    }
    public void CloseAllMenus()
    {
        mainPanel.SetActive(false);
        for (int i = 0; i < menus.Length; i++)
        {
            menus[i].gameObject.SetActive(false);
        }
        menuStack.Clear();
    }
    private void FixedUpdate()
    {
        //안드로이드인 경우
        // 뒤로가기 키로 메뉴를 종료할 수 있다.
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape)) //뒤로가기 키 입력
            {
                CloseMenu(menuStack.Peek());
            }
        }
    }
}