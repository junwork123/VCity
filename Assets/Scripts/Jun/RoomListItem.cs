using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    RoomInfo info;
    public void Setup(RoomInfo _info){
        info = _info;
        text.text = _info.Name;
    }

    public void OnClick(){
        ConnManager.instance.JoinRoom(info);
    }
}
