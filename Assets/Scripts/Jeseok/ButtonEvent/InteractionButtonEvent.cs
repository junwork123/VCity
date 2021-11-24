using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;

public class InteractionButtonEvent : MonoBehaviour
{
    public InteractionButtonClickListener[] clickListener;

    GameObject rootObject;
    Interactionable interactionable;


    // Start is called before the first frame update
    void Start()
    {
        clickListener = GetComponentsInChildren<InteractionButtonClickListener>();

        for (int i = 0; i < clickListener.Length; ++i)
            clickListener[i].AddClickCallback(Interaction);

        rootObject = transform.parent.gameObject;
        interactionable = rootObject.GetComponent<Interactionable>();
    }

    void Interaction(InteractionType interactionType)
    {
        switch (interactionType)
        {
            case InteractionType.APPLY:
                UIManager.instance.ShowApplyPanel(interactionable.objectType);
                break;

            case InteractionType.LOG:
                ChatManager.Instance.GetComponent<MenuManager>().OpenMenu("DISPLAY_ROOMS");
                break;

            case InteractionType.MYPAGE:
                
                break;

            case InteractionType.EXIT:
                interactionable.ShowInter();
                interactionable.HideInteractionMenu();
                GameManager.instance.joystickRange.SetActive(true);
                break;

            case InteractionType.ETC:
                break;

            default:
                break;
        }
    }
}
