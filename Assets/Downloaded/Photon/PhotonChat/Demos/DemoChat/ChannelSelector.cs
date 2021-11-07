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
        public string channelId;
        [SerializeField]
        public TMP_Text date;
        public TMP_Text roomName;
        public Image image;

        public void SetChannelId(string _channelId)  
        {
            this.channelId = _channelId;
        }
        public void setRoomName(string roomName)
        {
            this.roomName.text = roomName;
        }
        public void setDate(string date)
        {
            this.date.text = date;
        }
        public void setImage(int _serviceNum)
        {
            image.sprite = ImageSet.Instance.GetServiceImg(_serviceNum);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            ChatManager handler = FindObjectOfType<ChatManager>();
            handler.ShowChannel(this.channelId);

            // PanelSelector ps = FindObjectOfType<PanelSelector>();
            // ps.CloseChatMenu(ChatMenu.ChannelBar);
            // ps.OpenChatMenu((int)ChatMenu.ChatOutput);
            // ps.OpenChatMenu((int)ChatMenu.InputBar);
        }
    }
}