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

    public void SetMsg(CustomMsg _msg)
    {
        nameText.text = _msg.Sender;
        dateText.text = _msg.Time;
        chatText.text = _msg.Text;
        SetProfile(_msg.Profile);
    }

    public bool Equals(CustomMsg _msg)
    {
        return _msg.Sender == nameText.text && _msg.Time == dateText.text && _msg.Text == chatText.text;
    }
    public void SetProfile(int _profileNum)
    {
        profile.sprite = ImageSet.Instance.GetProfileImg(_profileNum);
    }
}
