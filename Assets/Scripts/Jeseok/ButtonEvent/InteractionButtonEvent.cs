using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionButtonEvent : MonoBehaviour
{
    public InteractionButtonClickListener[] clickListener;
    public GameObject chatManager;
    // Start is called before the first frame update
    void Start()
    {
        clickListener = GetComponentsInChildren<InteractionButtonClickListener>();

        for (int i = 0; i < clickListener.Length; ++i)
            clickListener[i].AddClickCallback(Interaction);
    }

    void Interaction(InteractionType interactionType)
    {
        MenuManager menuManager = chatManager.GetComponent<MenuManager>();
        switch (interactionType)
        {
            case InteractionType.APPLY:
                AndroidToastManager.instance.ShowToast("APPLY");
                UIManager.instance.ShowApplyPanel();
                menuManager.OpenMenu("SELECT_SERVICE");
                break;

            case InteractionType.LOG:
                AndroidToastManager.instance.ShowToast("LOG");

                #region 메시지 호출부
                menuManager.OpenMenu("SHOW_MSG");
                #endregion

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
