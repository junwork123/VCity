using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager>
{
    [Header("Player Profile")]
    public TextMeshPro nameText;
    public TextMeshProUGUI typeTextUI;
    public TextMeshProUGUI nameTextUI;
    public TextMeshProUGUI MyPagenameTextUI;

    [Space(10f)]
    public GameObject actionButton;
    public GameObject dialog;
    public TextMeshProUGUI dialogText;

    [Header("Panel")]
    [SerializeField]
    List<GameObject> panels;
    public GameObject minimapPanel;
    public GameObject applyPanel;


    void Start()
    {
        InitPanel();

        InactiveActionButton();
    }

    void InitPanel()
    {
        for (int i = 0; i < panels.Count; ++i)
            panels[i].SetActive(false);
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
            MyPagenameTextUI.text = DataManager.Instance.userCache.Nickname;
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
        minimapPanel.SetActive(true);
    }

    public void HideMinimap()
    {
        minimapPanel.SetActive(false);
    }

    public void ToggleMinimap()
    {
        minimapPanel.SetActive(!minimapPanel.activeSelf);
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
