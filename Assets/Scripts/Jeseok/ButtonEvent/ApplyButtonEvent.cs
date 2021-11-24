using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
public class ApplyButtonEvent : MonoBehaviour
{
    public ApplyButtonClickListener[] clickListener;
    // Start is called before the first frame update
    void Start()
    {
        clickListener = GetComponentsInChildren<ApplyButtonClickListener>();

        for (int i = 0; i < clickListener.Length; ++i)
            clickListener[i].AddClickCallback(Apply);
    }

    void Apply(ApplyType applyType)
    {
        switch (applyType)
        {
            case ApplyType.ISSUANCE_REGISTRATION_CARD:
                Service.Instance.GetComponent<MenuManager>().OpenMenu("REQUEST_INFO");
                break;

            case ApplyType.ISSUANCE_CERTIFIED_RESIDENT_REGISTRATION:
                break;

            case ApplyType.COUNSEL:
                UIManager.instance.OpenPanel("COUNSEL_SELECT_PANEL");
                UIManager.instance.ClosePanel("LIFE_APPLY_PANEL");
                break;

            // COUNSEL SELECT
            case ApplyType.VIDEOCALL_COUNSEL_APPLY:
                UIManager.instance.OpenPanel("VIDEOCALL_WAIT_PANEL");
                UIManager.instance.ClosePanel("COUNSEL_SELECT_PANEL");
                break;

            case ApplyType.CHAT_COUNSEL_APPLY:
                UIManager.instance.OpenPanel("CHAT_WAIT_PANEL");
                UIManager.instance.ClosePanel("COUNSEL_SELECT_PANEL");
                break;

            case ApplyType.NONE:
                print("Not Set Button Event");
                break;
        }
    }
}
