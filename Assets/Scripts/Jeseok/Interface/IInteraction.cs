using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InteractionType
{
    NPC,
    UNMANNED,
    ETC,
}

/// <summary>
/// Interaction 인터페이스.
/// </summary>
public interface IInteraction
{
    /// <summary>
    /// 상호작용 종류(NPC, UNMANNED, ...).
    /// </summary>
    InteractionType interactionType { get; set; }
    /// <summary>
    /// 플레이어가 이 오브젝트를 사용할 수 있는지 여부.
    /// </summary>
    bool enable { get; set; }
    // string InterString { get; set; }
    // string sInterPlayerAni { get; }


    /// <summary>
    /// 범위 내에서 상호작용할 수 있는 오브젝트임을 출력하는 동작
    /// </summary>
    void ShowInter();
    /// <summary>
    /// 상호작용을 비활성화하는 동작
    /// </summary> 
    void EndInter();
    /// <summary>
    /// 범위내에 오브젝트가 없을 때 동작
    /// </summary>
    void NonShowInter();
    /// <summary>
    /// ShowInter 상태에서 enable이 true이고 Action 키를 입력 실행했을 때 동작.
    /// </summary>
    void Action();
}
