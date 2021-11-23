using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class Channel
{
    [FirestoreProperty] public string Id { get; set; }

    [FirestoreProperty] public string Name { get; set; }

    [FirestoreProperty] public int Kind { get; set; }
    [FirestoreProperty] public List<string> Members { get; set; }


    //[FirestoreDocumentId] public CollectionReference ChatContents { get; set; }
    public Channel() { }
    public Channel(string _channelId, string _channelName, List<string> _memberList)
    {
        Id = _channelId;
        Kind = 0;
        Members = _memberList;
        Name = _channelName;

        //CollectionReference ChatContents = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance).Collection("Channels").Document(_channelId).Collection("ChatContents");
    }
    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["Id"] = Id;
        result["Name"] = Name;
        result["Kind"] = Kind;
        result["Members"] = Members;

        return result;
    }
    public string Set()
    {
        return Name;
    }
}
