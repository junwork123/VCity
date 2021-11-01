using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Register : MonoBehaviour
{
    [SerializeField]
    TMP_InputField nameInput;
    [SerializeField]
    TMP_InputField idInput;
    [SerializeField]
    TMP_InputField pwInput;
    [SerializeField]
    TMP_InputField doublecheck;
    [SerializeField]
    TMP_Text pwCheckText;
    [SerializeField]
    TMP_InputField addressInput;
    [SerializeField]
    TMP_InputField detailedInput;

    UserData userData;
    string userId;
    string userPw;

    public void SetUserData()
    {
        userData = new UserData();
        if (CheckName()) userData.Name = nameInput.text; else return;
        if (CheckId())
        {
            userData.Id = idInput.text;
            userId = idInput.text;
        }
        else
            return;

        if (CheckPassword()) userPw = pwInput.text; else return;
        userData.Address = addressInput.text;
        userData.DetailedAddress = detailedInput.text;
        userData.Profile = "";

        NetworkManager.instance.Register(userId, userPw, userData);
    }
    public bool CheckName() // 한글인지 확인
    {
        foreach (char c in nameInput.text.ToCharArray())
        {
            if (char.GetUnicodeCategory(c) != System.Globalization.UnicodeCategory.OtherLetter)
            {
                Debug.Log("[Register] : " + "이름에 한글이 아닌 문자가 있습니다.");
                return false;
            }
        }
        Debug.Log("[Register] : " + "정상적인 이름 양식입니다");
        return true;
    }
    public bool CheckId()
    {
        return true;
    }
    public bool CheckPassword()
    {
        if (pwInput.text == doublecheck.text)
        {
            Debug.Log("[Register] : " + "비밀번호가 일치합니다.");
            pwCheckText.alpha = 0;
            return true;
        }
        else
        {
            Debug.Log("[Register] : " + "비밀번호가 일치하지 않습니다.");
            pwCheckText.alpha = 1;
            return false;
        }
    }
    public string FindAddress() // 주소검색
    {
        return "";
    }

}
