using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CustomMsg
{
    [FirestoreProperty] public string Sender { get; set; }
    [FirestoreProperty] public string Text { get; set; }
    [FirestoreProperty] public string Time { get; set; }
    [FirestoreProperty] public int Profile { get; set; }



    public CustomMsg() { }
    public CustomMsg(string _sender, string _datetime, string _text, int _characterNum)
    {
        Sender = _sender;
        Text = _text;
        Time = _datetime;
        Profile = _characterNum;
    }

    public CustomMsg(CustomMsg _msg)
    {
        Sender = _msg.Sender;
        Time = _msg.Time;
        Text = _msg.Text;
    }
    public Dictionary<string, object> ToDictionary()
    {
        Dictionary<string, object> result = new Dictionary<string, object>();
        result["Sender"] = Sender;
        result["Text"] = Text;
        result["Time"] = Time;
        result["Profile"] = Profile;


        return result;
    }
    public override string ToString()
    {
        return Time + " " + Sender + " : " + Text + "\n";
    }
}
