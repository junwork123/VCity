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
                print("Message Button");
                break;
            case TaskType.TELEPORT:
                AndroidToastManager.instance.ShowToast("Teleport Button");
                print("Teleport Button");
                break;
            case TaskType.APPLY:
                AndroidToastManager.instance.ShowToast("Apply Button");
                print("Apply Button");
                break;
            case TaskType.ETC:
                AndroidToastManager.instance.ShowToast("ETC Button");
                print("ETC Button");
                break;
            default:
                break;
        }
    }
}
