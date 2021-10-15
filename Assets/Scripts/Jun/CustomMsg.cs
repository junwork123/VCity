using System.Collections;
using System.Collections.Generic;
using Firebase.Firestore;

[FirestoreData]
public class CustomMsg
{
    [FirestoreProperty] public string Sender { get; set; }
    [FirestoreProperty] public string Time { get; set; }

    [FirestoreProperty] public string Text { get; set; }

    public CustomMsg(){}
    public CustomMsg(string _sender, string _datetime, string _text)
    {
        Sender = _sender;
        Time = _datetime;
        Text = _text;
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
        result["Time"] = Time;
        result["Text"] = Text;

        return result;
    }
    public override string ToString()
    {
        return Time + " " + Sender + " : " + Text + "\n";
    }
}
