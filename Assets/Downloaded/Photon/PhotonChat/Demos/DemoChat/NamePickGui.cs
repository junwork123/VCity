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
    public class NamePickGui : MonoBehaviour
    {

        string UserIdPref = "junwork123@gmail.com";
        string UserPwPref = "wnswns95";

        public TMP_InputField idInput;
        public TMP_InputField pwInput;


        public void Start()
        {
#if UNITY_EDITOR
            UserIdPref = "junwork123@gmail.com";
            UserPwPref = "wnswns95";
#else
        UserIdPref = "sposent7@naver.com";
        UserPwPref = "wnswns95";
#endif

            this.idInput.text = UserIdPref;
            this.pwInput.text = UserPwPref;
        }


        // new UI will fire "EndEdit" event also when loosing focus. So check "enter" key and only then StartChat.
        public void EndEditOnEnter()
        {
            this.enabled = false;
            PlayerPrefs.SetString(UserIdPref, idInput.text);
            PlayerPrefs.SetString(UserPwPref, pwInput.text);
        }

    }
}