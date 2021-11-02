using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Register : MonoBehaviour
{
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] TMP_InputField idInput;
    [SerializeField] TMP_InputField pwInput;
    [SerializeField] TMP_InputField pwCheck;
    [SerializeField] TMP_Text pwCheckText;
    [SerializeField] TMP_InputField addressInput;
    [SerializeField] TMP_InputField detailedInput;
    [SerializeField] TMP_InputField phoneNumInput1;
    [SerializeField] TMP_InputField phoneNumInput2;
    [SerializeField] TMP_InputField phoneNumInput3;
    [SerializeField] string characterName;
    [SerializeField] TMP_InputField nicknameInput;
    string Sex;

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
        userData.PhoneNum = CombinePhoneNum();
        userData.Nickname = nicknameInput.text;
        userData.Character = characterName;
        userData.Sex = Sex;
        userData.Profile = "";

        // TODO : 성공하면 바로 씬으로 진입하기
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
        if (pwInput.text == pwCheck.text)
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

    public string CombinePhoneNum()
    {
        if (phoneNumInput1.text != "" && phoneNumInput2.text != "" && phoneNumInput3.text != "")
        {
            return phoneNumInput1.text + "-" + phoneNumInput2.text + "-" + phoneNumInput3.text;
        }
        else
            return "";
    }

    public void SetCharacterName(string _name){
        characterName = _name;
    }
}
