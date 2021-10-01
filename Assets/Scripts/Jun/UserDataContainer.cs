using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserDataContainer
{

    public string userId;
    public string userName;
    //public string userPw;
    //public List<Channel> channels;
    public Dictionary<string, List<CustomMsg>> channels;

    public UserDataContainer()
    {
        userId = null;
        userName = "Mr.temp";
        channels = new Dictionary<string, List<CustomMsg>>();
        channels["Region"] = new List<CustomMsg>();
        //channels["Guild"] = new List<CustomMsg>();
    }

    public UserDataContainer(string _userId, string _userName = "Ms.temp")
    {
        userId = _userId;
        userName = _userName;
        channels = new Dictionary<string, List<CustomMsg>>();
        channels["Region"] = new List<CustomMsg>();
        //channels["Guild"] = new List<CustomMsg>();
    }

}
