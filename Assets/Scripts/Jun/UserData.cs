using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class UserData
{
    [FirestoreProperty] public string UID { get; set; }
    [FirestoreProperty] public string Name { get; set; }
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public string Nickname { get; set; }
    [FirestoreProperty] public string Address { get; set; }
    [FirestoreProperty] public string DetailedAddress { get; set; }
    [FirestoreProperty] public string PhoneNum { get; set; }
    [FirestoreProperty] public bool Push { get; set; }
    [FirestoreProperty] public string Gender { get; set; }
    [FirestoreProperty] public string Character { get; set; }
    [FirestoreProperty] public string Profile { get; set; }

    // <친구id, 채널명에 대한 Firestore 참조값>
    [FirestoreProperty] public List<string> Friends { get; set; }
    [FirestoreProperty] public Dictionary<string, string> MyChannels { get; set; }

    //[FirestoreProperty] public string IpAddress { get; set; }

    public UserData()
    {
        UID = "0000";
        Name = "Name";
        Id = null;
        Nickname = "Nickname";
        Address = "Address";
        DetailedAddress = "DetailedAddress";
        PhoneNum = "010-0000-0000";
        Push = false;
        Gender = "male";
        Character = "man1";
        Profile = null;
        Friends = new List<string>();
        MyChannels = new Dictionary<string, string>();
    }

    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();

        result["UID"] = UID;
        result["Name"] = Name;
        result["Id"] = Id;
        result["Nickname"] = Nickname;
        result["Address"] = Address;
        result["DetailedAddress"] = DetailedAddress;
        result["PhoneNum"] = PhoneNum;
        result["Push"] = Push;
        result["Gender"] = Gender;
        result["Character"] = Character;
        result["Profile"] = Profile;
        result["Friends"] = Friends;
        result["MyChannels"] = MyChannels;

        return result;
    }
}
