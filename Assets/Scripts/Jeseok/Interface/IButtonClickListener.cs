using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ButtonClickListener 인터페이스
/// </summary>
public interface IButtonClickListener<T>
{
    /// <summary>
    /// 버튼의 종류(INTERACTION, APPLY, ...)
    /// </summary>
    public T buttonType { get; set; }

    Action<T> onClickCallback { get; set; }

    public void SetButtonType(T type);

    public void AddClickCallback(Action<T> callback);

    public void OnClick();
}
