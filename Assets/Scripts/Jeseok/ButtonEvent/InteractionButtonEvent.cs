using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;

public class InteractionButtonEvent : MonoBehaviour
{
    public InteractionButtonClickListener[] clickListener;
    // Start is called before the first frame update
    void Start()
    {
        clickListener = GetComponentsInChildren<InteractionButtonClickListener>();

        for (int i = 0; i < clickListener.Length; ++i)
            clickListener[i].AddClickCallback(Interaction);
    }

    void Interaction(InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.APPLY:
                AndroidToastManager.instance.ShowToast("APPLY");
                UIManager.instance.ShowApplyPanel();
                break;

            case InteractionType.LOG:
                AndroidToastManager.instance.ShowToast("LOG");
                ChatManager.Instance.GetComponent<MenuManager>().OpenMenu("DISPLAY_ROOMS");
                break;

            case InteractionType.HELP:
                AndroidToastManager.instance.ShowToast("HELP");
                break;
            case InteractionType.EXIT:
                AndroidToastManager.instance.ShowToast("EXIT");
                break;

            // case InteractionType.TELEPORT:
            //     AndroidToastManager.instance.ShowToast("Teleport Button");
            //     GameManager.instance.TeleportPlayer();
            //     GameManager.instance.joystickRange.SetActive(true);
            //     break;


            case InteractionType.ETC:
                AndroidToastManager.instance.ShowToast("ETC Button");
                break;

            default:
                break;
        }
    }
}
