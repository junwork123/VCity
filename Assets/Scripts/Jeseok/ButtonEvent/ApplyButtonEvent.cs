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
            #region LIFE
            case ApplyType.ISSUANCE_CERTIFIED_RESIDENT_REGISTRATION:
                ChatManager.Instance.GetComponent<MenuManager>().OpenMenu("DISPLAY_ROOMS");
                break;

            case ApplyType.ISSUANCE_REGISTRATION_CARD:
            case ApplyType.ISSUANCE_PASSPORT:
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
            #endregion

            #region STUDY
            case ApplyType.ISSUANCE_CERTIFICATE_ENROLLMENT:
            case ApplyType.ISSUANCE_CERTIFICATE_GRADUATION:
            case ApplyType.ISSUANCE_CERIFICATE_LEAVEOFABSENCE:
            case ApplyType.ISSUANCE_CERTIFICATE_PAYMENT:
                break;
            #endregion

            #region JOB
            case ApplyType.TECHNICAL_QUALIFICATION:
            case ApplyType.EMPLOYMENT_SPECIAL_LECTURE:
            case ApplyType.ISSUANCE_EMPLOYMENT_ISURANCE_HISTORY:
            case ApplyType.ISSUACNE_CERTIFICATE_HEALTH:
                break;
            #endregion

            #region ETC
            case ApplyType.COUNSEL_LOG:
                ChatManager.Instance.GetComponent<MenuManager>().OpenMenu("DISPLAY_ROOMS");
                break;

            case ApplyType.MY_PAGE:
                Service.Instance.GetComponent<MenuManager>().OpenMenu("DISPLAY_MYPAGE");
                break;
            #endregion

            case ApplyType.NONE:
                print("Not Set Button Event");
                break;
        }
    }
}
