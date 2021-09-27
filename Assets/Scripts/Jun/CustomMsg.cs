using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomMsg
{
    string sender;
    string time;
    string text;

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
}
