using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            case InteractionType.MESSAGE:
                AndroidToastManager.instance.ShowToast("Message Button");
                // FindObjectOfType<Photon.Chat.PanelSelector>().OpenChatMenu((int)Photon.Chat.ChatMenu.ChannelBar);

                #region 메시지 호출부
                FindObjectOfType<Photon.Chat.PanelSelector>().OpenChatMenu((int)Photon.Chat.ChatMenu.ChannelBar);
                #endregion

                break;

            case InteractionType.TELEPORT:
                AndroidToastManager.instance.ShowToast("Teleport Button");
                GameManager.instance.TeleportPlayer();
                GameManager.instance.joystickRange.SetActive(true);
                break;

            case InteractionType.APPLY:
                AndroidToastManager.instance.ShowToast("Apply Button");
                UIManager.instance.ShowApplyPanel();
                break;

            case InteractionType.ETC:
                AndroidToastManager.instance.ShowToast("ETC Button");
                break;

            default:
                break;
        }
    }
}
