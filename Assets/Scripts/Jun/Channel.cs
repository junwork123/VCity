using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Firebase.Firestore.FirestoreData]
public class Channel
{
    [Firebase.Firestore.FirestoreProperty] public string Id { get; set; }

    [Firebase.Firestore.FirestoreProperty] public string Name { get; set; }

    [Firebase.Firestore.FirestoreProperty] public string Kind { get; set; }

    [Firebase.Firestore.FirestoreProperty] public List<string> Members { get; set; }

    [Firebase.Firestore.FirestoreProperty] public List<CustomMsg> ChatContents { get; set; }

    public Channel(string _channelId, string _memberId, string _memberId2)
    {
        Id = _channelId;
        Kind = "request";
        Name = Kind + " : " + _memberId2;
        Members = new List<string>();
        Members.Add(_memberId);
        Members.Add(_memberId2);
        ChatContents = new List<CustomMsg>();
    }
    public Dictionary<string, System.Object> ToDictionary()
    {
        Dictionary<string, System.Object> result = new Dictionary<string, System.Object>();
        result["id"] = Id;
        result["name"] = Name;
        result["members"] = Members;
        result["chatContents"] = ChatContents;

        return result;
    }
}
