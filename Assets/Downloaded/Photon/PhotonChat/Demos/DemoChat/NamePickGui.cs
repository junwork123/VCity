// --------------------------------------------------------------------------------------------------------------------
// <copyright company="Exit Games GmbH"/>
// <summary>Demo code for Photon Chat in Unity.</summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------


using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

namespace Photon.Chat
{
    [RequireComponent(typeof(ChatManager))]
    public class NamePickGui : MonoBehaviour
    {
        private const string UserNamePlayerPref = "1234";

        public ChatManager chatNewComponent;

        public TMP_InputField idInput;

        public void Start()
        {
            this.chatNewComponent = FindObjectOfType<ChatManager>();


            string prefsName = PlayerPrefs.GetString(UserNamePlayerPref);
            if (!string.IsNullOrEmpty(prefsName))
            {
                this.idInput.text = prefsName;
            }
        }


        // new UI will fire "EndEdit" event also when loosing focus. So check "enter" key and only then StartChat.
        public void EndEditOnEnter()
        {
            if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter))
            {
                this.StartChat();
            }
        }

        public void StartChat()
        {
            ChatManager chatNewComponent = FindObjectOfType<ChatManager>();
            //chatNewComponent.UserName = this.idInput.text.Trim();
            // 유저 데이터를 여기서 불러옴
            UserDataContainer udc = DataManager.instance.LoadDataWithId(idInput.text);
            chatNewComponent.UserName = udc.userName;
            chatNewComponent.Connect();
            this.enabled = false;

            PlayerPrefs.SetString(UserNamePlayerPref, udc.userId);
        }
    }
}