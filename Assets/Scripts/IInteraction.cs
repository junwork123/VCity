using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    Object,
    NPC,
}

/// <summary>
/// Interaction 인터페이스.
/// </summary>
public interface IInteraction
{
    /// <summary>
    /// 인터랙션 타입(Object, NPC, ...).
    /// </summary>
    InteractionType InterType { get; }
    /// <summary>
    /// 플레이어가 이 인터랙션을 사용할 수 있는지 여부.
    /// </summary>
    bool enable { get; set; }
    // string InterString { get; set; }
    // string sInterPlayerAni { get; }

    /// <summary>
    /// 인터랙션할 수 있는 오브젝트임을 출력하는 동작.
    /// </summary>
    void ShowInter();
    //
    /// <summary>
    /// ShowInter 상태에서 enable이 true이고 Action 키를 입력 실행했을 때 동작.
    /// </summary>
    void ActionKey();
    /// <summary>
    /// 인터랙션이 끝난 후 동작.
    /// </summary> 
    void EndInter();
    /// <summary>
    /// 인터랙션할 수 없을 때 동작.
    /// </summary>
    void NonShowInter();
}
