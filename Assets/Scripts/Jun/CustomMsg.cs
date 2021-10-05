using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMsg
{
    public string sender { get; set; }
    public string time { get; set; }

    public string text { get; set; }

    public CustomMsg(){ sender = ""; time = ""; text = "";}
    public CustomMsg(string _sender, string _datetime, string _text)
    {
        sender = _sender;
        time = _datetime;
        text = _text;
    }

    public CustomMsg(CustomMsg _msg)
    {
        sender = _msg.sender;
        time = _msg.time;
        text = _msg.text;
    }
    public override string ToString()
    {
        return time + " " + sender + " : " + text + "\n";
    }
}
