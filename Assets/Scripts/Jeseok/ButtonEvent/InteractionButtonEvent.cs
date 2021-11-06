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
                UIManager.instance.ShowApplyPanel();
                break;

            case InteractionType.LOG:
                ChatManager.Instance.GetComponent<MenuManager>().OpenMenu("DISPLAY_ROOMS");
                break;

            case InteractionType.HELP:
                break;

            case InteractionType.EXIT:
                break;

            case InteractionType.ETC:
                break;

            default:
                break;
        }
    }
}
