using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class UserData
{
    [FirestoreProperty] public string Name { get; set; }
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public string Nickname { get; set; }
    [FirestoreProperty] public string Address { get; set; }
    [FirestoreProperty] public string DetailedAddress { get; set; }
    [FirestoreProperty] public string PhoneNum { get; set; }
    [FirestoreProperty] public bool Push { get; set; }
    [FirestoreProperty] public string Sex { get; set; }
    [FirestoreProperty] public string Character { get; set; }
    [FirestoreProperty] public string Profile { get; set; }

    // <친구id, 채널명에 대한 Firestore 참조값>
    [FirestoreProperty] public List<string> Friends { get; set; }
    [FirestoreProperty] public Dictionary<string, string> MyChannels { get; set; }

    //[FirestoreProperty] public string IpAddress { get; set; }

    public UserData()
    {
        Name = "Name";
        Id = null;
        Nickname = "Nickname";
        Address = "Address";
        DetailedAddress = "DetailedAddress";
        PhoneNum = "010-0000-0000";
        Push = false;
        Sex = "male";
        Character = "John Doe";
        Profile = null;
        Friends = new List<string>();
        MyChannels = new Dictionary<string, string>();
    }

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["Name"] = Name;
        result["Id"] = Id;
        result["Nickname"] = Id;
        result["Address"] = Address;
        result["DetailedAddress"] = DetailedAddress;
        result["PhoneNum"] = Profile;
        result["Push"] = Profile;
        result["Sex"] = Profile;
        result["Character"] = Profile;
        result["Profile"] = Profile;
        result["Friends"] = Friends;
        result["MyChannels"] = MyChannels;

        return result;
    }
}
