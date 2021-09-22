using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    // Start is called before the first frame update
    public float x, y, z;
    void Start()
    {   
        gameObject.transform.localScale = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopupUi(){
        iTween.ScaleTo(gameObject, iTween.Hash(
            "x", x,
            "y", y,
            "z", z,
            "time", 0.5f,
            "easetype", iTween.EaseType.easeOutBounce
        ));
    }

    public void ShrinkUi(){
        iTween.ScaleTo(gameObject, iTween.Hash(
            "x", 0,
            "y", 0,
            "z", 0,
            "time", 0.5f,
            "easetype", iTween.EaseType.easeOutBounce
        ));
    }
}
