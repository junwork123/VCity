// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


namespace Photon.Chat
{
    public class ChannelSelector : MonoBehaviour, IPointerClickHandler
    {
        public string Channel;

        public void SetChannel(string channel)
        {
            this.Channel = channel;
            TMP_Text t = this.GetComponentInChildren<TMP_Text>();
            t.text = this.Channel;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ChatManager handler = FindObjectOfType<ChatManager>();
            handler.ShowChannel(this.Channel);

            PanelSelector ps = FindObjectOfType<PanelSelector>();
            ps.CloseChatMenu(ChatMenu.ChannelBar);
            ps.OpenChatMenu((int)ChatMenu.ChatOutput);
            ps.OpenChatMenu((int)ChatMenu.InputBar);
        }
    }
}