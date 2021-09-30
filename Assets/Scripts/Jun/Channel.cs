using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Channelqwer
{
    public string name;
    public List<CustomMsg> chatContents;

    public Channelqwer(string _name){
        name = _name;
        chatContents = new List<CustomMsg>();
    }
}
