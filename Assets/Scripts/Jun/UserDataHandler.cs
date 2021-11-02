using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserDataHandler : MonoBehaviour
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
    [SerializeField] TMP_InputField RegiEndNum_native;
    [SerializeField] TMP_InputField RegiEndNum_foreign;
    //[SerializeField] Toggle pushBtn;

    UserData userData;
    string userId;
    string userPw;

    void SetUserData()
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
        userData.Gender = GetGender();
        //userData.Push = pushBtn.
        userData.Profile = "";
    }
    public void LoginAsync()
    {
        // 3. 회원 가입이 성공했다면 로그인을 한다.
        StartCoroutine(RegisterAsync());
        NetworkManager.instance.Login(userId, userPw);
    }
    IEnumerator RegisterAsync()
    {
        // 1. 가입할 유저 정보를 확정한다.
        SetUserData();

        // 2. 회원 가입이 성공할 때까지 기다린다.
        yield return StartCoroutine(NetworkManager.instance.Register(userId, userPw, userData));

    }
    string CombinePhoneNum()
    {
        if (phoneNumInput1.text != "" && phoneNumInput2.text != "" && phoneNumInput3.text != "")
        {
            return phoneNumInput1.text + "-" + phoneNumInput2.text + "-" + phoneNumInput3.text;
        }
        else
            return "";
    }
    string GetGender()
    {
        int genderNum;
        string str = "";
        if (RegiEndNum_native != null && RegiEndNum_native.text != "")
            str = RegiEndNum_native.text[0].ToString();
        else if (RegiEndNum_foreign != null && RegiEndNum_foreign.text != "")
            str = RegiEndNum_foreign.text[0].ToString();

        if (int.TryParse(RegiEndNum_native.text[0].ToString(), out genderNum))
        {
            if (genderNum % 2 == 0) return "female";
            else return "male";
        }
        return "ParseError";
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
    public void SetCharacterName(string _name)
    {
        characterName = _name;
    }
}
