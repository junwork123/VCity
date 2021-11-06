using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    public TextMeshPro nameText;
    public TextMeshProUGUI typeTextUI;
    public TextMeshProUGUI nameTextUI;

    public GameObject actionButton;
    public GameObject dialog;
    public TextMeshProUGUI dialogText;
    public GameObject applyPanel;
    public GameObject minimap;


    void Start()
    {
        HideMinimap();
        HideApplyPanel();

        HideDialog();
        InactiveActionButton();
    }

    public void UpdatePlayerInfo()
    {
        typeTextUI.text = "민원신청인";
        if (DataManager.Instance == null)
        {
            nameText.text = "테스트";
            nameTextUI.text = "테스트";
        }
        else
        {
            nameText.text = DataManager.Instance.userCache.Nickname;
            nameTextUI.text = DataManager.Instance.userCache.Nickname;
        }
    }

    public void AtciveActionButton()
    {
        actionButton.GetComponent<Button>().interactable = true;
    }

    public void InactiveActionButton()
    {
        actionButton.GetComponent<Button>().interactable = false;
    }

    public void ShowMinimap()
    {
        minimap.SetActive(true);
    }

    public void HideMinimap()
    {
        minimap.SetActive(false);
    }

    public void ToggleMinimap()
    {
        minimap.SetActive(!minimap.activeSelf);
    }

    public void ShowApplyPanel()
    {
        applyPanel.SetActive(true);
    }

    public void HideApplyPanel()
    {
        applyPanel.SetActive(false);
    }

    public void ShowDialog()
    {
        dialog.SetActive(true);
    }

    public void HideDialog()
    {
        dialog.SetActive(false);
        SetDialogMessage("");
    }

    public void SetDialogMessage(string message)
    {
        dialogText.text = message;
    }

    public void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void ClosePanel(GameObject panel)
    {
        panel.SetActive(false);
    }
}
