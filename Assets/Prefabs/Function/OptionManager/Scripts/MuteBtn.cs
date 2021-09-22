using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MuteBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public Image btnImg;
    public Sprite noMuteImg;
    public Sprite muteImg;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeImage(){
        if(btnImg.sprite.Equals(noMuteImg))
            btnImg.sprite = muteImg;
        else if(btnImg.sprite.Equals(muteImg))
            btnImg.sprite = noMuteImg;            
    }

}
