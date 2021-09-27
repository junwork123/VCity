using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public static ButtonEvent instance;


    public PlayerController playerController;

    public bool activeActionButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OnClickActionButton()
    {
        activeActionButton = true;
    }

    public void OnClickChatButton()
    {
        print("Click ChatButton");
    }

    public void OnClickMapButton()
    {

    }
}
