using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class UserData
{
    [FirestoreProperty] public string Id { get; set; }
    [FirestoreProperty] public string Email { get; set; }
    [FirestoreProperty] public string Name { get; set; }
    [FirestoreProperty] public string Address { get; set; }
    [FirestoreProperty] public string DetailedAddress { get; set; }
    [FirestoreProperty] public string Profile { get; set; }

    // <친구id, 채널명에 대한 Firestore 참조값>
    [FirestoreProperty] public List<string> Friends { get; set; }
    // <채널명, 채널명에 대한 Firestore 참조값>
    [FirestoreProperty] public List<string> Channels { get; set; }
    [FirestoreProperty] public Dictionary<string, string> MyChannels { get; set; }

    //[FirestoreProperty] public string IpAddress { get; set; }

    public UserData()
    {
        Id = null;
        Email = null;
        Name = "Mr.temp";
        Address = "temp1";
        DetailedAddress = "temp2";
        Profile = null;
        Friends = new List<string>();
        Channels = new List<string>();
        MyChannels = new Dictionary<string, string>();
        
        //IpAddress = "localhost";

        //channels["Guild"] = new List<CustomMsg>();
    }

    public UserData(string _userId, string _userEmail, string _userName = "Ms.temp")
    {
        Id = _userId;
        Email = _userEmail;
        Name = _userName;
        Address = "temp1";
        DetailedAddress = "temp2";
        Profile = null;
        Friends = new List<string>();
        Channels = new List<string>();
        MyChannels = new Dictionary<string, string>();
        //IpAddress = "localhost";
    }
    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["Id"] = Id;
        result["Email"] = Email;
        result["Name"] = Name;
        result["Address"] = Address;
        result["DetailedAddress"] = DetailedAddress;
        result["Profile"] = Profile;
        result["Friends"] = Friends;
        result["Channels"] = Channels;
        result["MyChannels"] = MyChannels;
        
        //result["IpAddress"] = IpAddress;

        return result;
    }
}
