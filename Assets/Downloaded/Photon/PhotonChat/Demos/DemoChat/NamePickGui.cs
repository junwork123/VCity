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
        private const string UserIdPref = "junwork123@gmail.com";
        private const string UserPwPref = "wnswns95";
        private const string UserNamePref = "jun";

        public TMP_InputField idInput;
        public TMP_InputField pwInput;
        public TMP_InputField nameInput;

        public void Start()
        {

            string prefsId = PlayerPrefs.GetString(UserIdPref);
            string prefsPw = PlayerPrefs.GetString(UserPwPref);
            string prefsName = PlayerPrefs.GetString(UserNamePref);
            if (!string.IsNullOrEmpty(prefsId) && !string.IsNullOrEmpty(prefsPw) && !string.IsNullOrEmpty(prefsName))
            {
                this.idInput.text = prefsId;
                this.pwInput.text = prefsPw;
                this.nameInput.text = prefsName;
            }
        }


        // new UI will fire "EndEdit" event also when loosing focus. So check "enter" key and only then StartChat.
        public void EndEditOnEnter()
        {
            this.enabled = false;
            PlayerPrefs.SetString(UserIdPref, idInput.text);
            PlayerPrefs.SetString(UserPwPref, pwInput.text);
            PlayerPrefs.SetString(UserNamePref, nameInput.text);
        }

    }
}