using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    [SerializeField] TMP_InputField UserIdInputField;
    [SerializeField] TMP_InputField UserPwInputField;
    [SerializeField] TMP_InputField roomNameInputField;

    public void LoginAsync(){
        NetworkManager.instance.Login()
    }
}
