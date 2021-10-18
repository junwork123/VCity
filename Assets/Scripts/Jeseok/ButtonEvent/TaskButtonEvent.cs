using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskButtonEvent : MonoBehaviour
{
    public ButtonClickListener[] clickListener;

    // Start is called before the first frame update
    void Start()
    {
        clickListener = GetComponentsInChildren<ButtonClickListener>();

        for (int i = 0; i < clickListener.Length; ++i)
            clickListener[i].AddClickCallback(TaskAction);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskAction(TaskType taskType)
    {
        switch (taskType)
        {
            case TaskType.MESSAGE:
                AndroidToastManager.instance.ShowToast("Message Button");
                // FindObjectOfType<Photon.Chat.PanelSelector>().OpenChatMenu((int)Photon.Chat.ChatMenu.ChannelBar);
                break;

            case TaskType.TELEPORT:
                AndroidToastManager.instance.ShowToast("Teleport Button");
                GameManager.instance.TeleportPlayer();
                GameManager.instance.joystickRange.SetActive(true);
                break;

            case TaskType.APPLY:
                AndroidToastManager.instance.ShowToast("Apply Button");
                UIManager.instance.ShowApplyPanel();
                break;

            case TaskType.ETC:
                AndroidToastManager.instance.ShowToast("ETC Button");
                break;

            default:
                break;
        }
    }
}
