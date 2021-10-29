using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class MsgFactory : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text dateText;
    public TMP_Text chatText;

    public Image profile;
    public Image msgBox;

    public void SetMsg(CustomMsg _msg){
        nameText.text = _msg.Sender;
        dateText.text = _msg.Time;
        chatText.text = _msg.Text;
    }
}
