using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public TMPro.TextMeshPro nameTextUI;

    public GameObject applyPanel;


    void Start()
    {
        HideApplyPanel();
    }

    public void SetPlayerName()
    {
        nameTextUI.text = GameManager.instance.playerName;
    }

    public void ShowApplyPanel()
    {
        applyPanel.SetActive(true);
    }

    public void HideApplyPanel()
    {
        applyPanel.SetActive(false);
    }
}
