using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Firebase.Firestore.FirestoreData]
public class UserDataContainer
{
    [Firebase.Firestore.FirestoreProperty] public string Id { get; set; }
    [Firebase.Firestore.FirestoreProperty] public string Email { get; set; }
    [Firebase.Firestore.FirestoreProperty] public string Name { get; set; }
    [Firebase.Firestore.FirestoreProperty] public string Profile { get; set; }

    // <친구id, 채널명에 대한 Firestore 참조값>
    [Firebase.Firestore.FirestoreProperty] public Dictionary<string, object> friends { get; set; }
    // <채널명, 채널명에 대한 Firestore 참조값>
    [Firebase.Firestore.FirestoreProperty] public Dictionary<string, object> channels { get; set; }

    public UserDataContainer()
    {
        Id = null;
        Email = null;
        Name = "Mr.temp";
        Profile = null;
        friends = new Dictionary<string, object>();
        channels = new Dictionary<string, object>();
        channels["region"] = "/rooms/region";
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
        channels["region"] = null;
    }
    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["id"] = Id;
        result["Email"] = Email;
        result["Name"] = Name;
        result["profile"] = Profile;
        result["friends"] = friends;
        result["channels"] = channels;

        return result;
    }
}
