using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ApplyButtonEvent : MonoBehaviour
{
    public ApplyButtonClickListener[] clickListener;
    public GameObject chatManager;
    // Start is called before the first frame update
    void Start()
    {
        clickListener = GetComponentsInChildren<ApplyButtonClickListener>();

        for (int i = 0; i < clickListener.Length; ++i)
            clickListener[i].AddClickCallback(Apply);
    }

    void Apply(ApplyType applyType)
    {
        print(applyType);
        switch (applyType)
        {
            case ApplyType.ISSUANCE_RESIDENT_REGISTRATION:
                break;
            case ApplyType.ISSUANCE_CERTIFIED_RESIDENT_REGISTRATION:
                chatManager.GetComponentInChildren<MenuManager>().OpenMenu();
                break;
            case ApplyType.COUNSEL:
                break;
            case ApplyType.ETC:
                break;
        }
    }
}
