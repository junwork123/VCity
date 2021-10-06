// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Photon.Chat
{
    public enum ChatMenu
    {
        ChannelBar,
        ChatOutput,
        InputBar
    }
    public class PanelSelector : MonoBehaviour
    {
        public GameObject ChatPanel;
        public GameObject BackgroundPanel;
        public GameObject ChannelBarPanel;
        public GameObject ChatOutputPanel;
        public GameObject InputBarPanel;

        public void OpenMenu()
        {
            ChatPanel.SetActive(true);
            BackgroundPanel.SetActive(true);
            GetComponentInChildren<Canvas>().sortingOrder = 5;
        }
        public void CloseMenu()
        {
            GetComponentInChildren<Canvas>().sortingOrder = 0;
            BackgroundPanel.SetActive(false);
            ChatPanel.SetActive(false);
        }
        public void OpenChatMenu(int _menu)
        {
            OpenMenu();
            switch ((ChatMenu)_menu)
            {
                case ChatMenu.ChannelBar:
                    ChannelBarPanel.SetActive(true);
                    break;
                case ChatMenu.ChatOutput:
                    ChatOutputPanel.SetActive(true);
                    break;
                case ChatMenu.InputBar:
                    InputBarPanel.SetActive(true);
                    break;
            }
        }
        public void CloseChatMenu(ChatMenu _menu)
        {
            switch (_menu)
            {
                case ChatMenu.ChannelBar:
                    ChannelBarPanel.SetActive(false);
                    break;
                case ChatMenu.ChatOutput:
                    ChatOutputPanel.SetActive(false);
                    break;
                case ChatMenu.InputBar:
                    InputBarPanel.SetActive(false);
                    break;
            }
        }
    }
}