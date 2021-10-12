using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserDataContainer
{

    public string userEmail;
    public string userName;
    public Image profile;

    // 채널명, 채널명에 대한 Firestore 참조값
    public Dictionary<string, string> channels;

    public UserDataContainer()
    {
        userEmail = null;
        userName = "Mr.temp";
        //profile = new Image;
        channels = new Dictionary<string, string>();
        channels["region"] = "/rooms/region";
        //channels["Guild"] = new List<CustomMsg>();
    }

    public UserDataContainer(string _userEmail, string _userName = "Ms.temp")
    {
        userEmail = _userEmail;
        userName = _userName;
        channels = new Dictionary<string, string>();
        channels["region"] = "/rooms/region";
    }
    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["Email"] = userEmail;
        result["Name"] = userName;
        result["profile"] = profile;
        result["channels"] = channels;

        return result;
    }
}
