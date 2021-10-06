using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickListener : MonoBehaviour
{
    public TaskType taskType;
    Action<TaskType> onClickCallback;


    public void SetTaskType(TaskType type)
    {
        taskType = type;
    }

    public void AddClickCallback(Action<TaskType> callback)
    {
        onClickCallback += callback;
    }

    public void OnClick()
    {
        if (onClickCallback == null)
            return;

        onClickCallback(taskType);
    }
}
