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

        public TMP_InputField idInput;

        public void Start()
        {

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
                this.enabled = false;
                PlayerPrefs.SetString(UserNamePlayerPref, idInput.text);
            }
        }

    }
}