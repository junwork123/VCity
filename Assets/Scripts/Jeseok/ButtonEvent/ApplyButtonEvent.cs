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
                break;

            case ApplyType.ISSUANCE_CERTIFIED_RESIDENT_REGISTRATION:
                Service.Instance.GetComponent<MenuManager>().OpenMenu("SELECT_SERVICE");
                break;

            case ApplyType.COUNSEL:
                // Service.Instance.GetComponent<MenuManager>().OpenMenu("VIDEO_CALL");
                break;

            case ApplyType.ETC:
                break;
        }
    }
}
