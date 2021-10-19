using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public TMPro.TextMeshPro nameTextUI;

    public GameObject actionButton;
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

    public void AtciveActionButton()
    {
        actionButton.GetComponent<Button>().interactable = true;
    }

    public void InactiveActionButton()
    {
        actionButton.GetComponent<Button>().interactable = false;
    }
}
