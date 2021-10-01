using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEventManager : MonoBehaviour
{
    public static ButtonEventManager instance;


    public PlayerController playerController;

    public bool activeActionButton;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void OnClickMenuButton()
    {
        AndroidToastManager.instance.ShowToast("Menu Button");
        FindObjectOfType<Photon.Chat.ChatManager>().EnableChatPanel();
    }

    public void OnClickActionButton()
    {
        AndroidToastManager.instance.ShowToast("Action Button");

        activeActionButton = true;
    }

    public void OnClickSettingButton()
    {
        AndroidToastManager.instance.ShowToast("Setting Button");
    }
}
