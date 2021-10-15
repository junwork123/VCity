using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UserDataContainer
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Profile { get; set; }

    // <친구id, 채널명에 대한 Firestore 참조값>
    public Dictionary<string, object> friends { get; set; }
    // <채널명, 채널명에 대한 Firestore 참조값>
    public Dictionary<string, object> channels { get; set; }

    public UserDataContainer()
    {
        Id = null;
        Email = null;
        Name = "Mr.temp";
        Profile = null;
        friends = new Dictionary<string, object>();
        channels = new Dictionary<string, object>();
        //channels["Guild"] = new List<CustomMsg>();
    }

    public UserDataContainer(string _userId, string _userEmail, string _userName = "Ms.temp")
    {
        Id = _userId;
        Email = _userEmail;
        Name = _userName;
        Profile = null;
        friends = new Dictionary<string, object>();
        channels = new Dictionary<string, object>();
    }
    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["id"] = Id;
        result["Email"] = Email;
        result["Name"] = Name;
        result["profile"] = Profile;
        result["friends"] = friends;
        result["channels"] = channels;

        return result;
    }
}
