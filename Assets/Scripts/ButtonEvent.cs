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

    public void OnClickMenuButton()
    {
        AndroidToast.instance.ShowToast("Menu Button");
    }

    public void OnClickActionButton()
    {
        AndroidToast.instance.ShowToast("Action Button");

        activeActionButton = true;
    }

    public void OnClickSettingButton()
    {
        AndroidToast.instance.ShowToast("Setting Button");
    }
}
