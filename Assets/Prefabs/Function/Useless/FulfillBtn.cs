using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FulfillBtn : MonoBehaviour
{
    // Start is called before the first frame update
    public Image btnImg;
    public float defaultAlpha = 0.2f;
    private Color color;
    void Start()
    {
        color = btnImg.GetComponent<Image>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseOver() {
        if(color.a < 0.8f){
            color.a += 0.005f;
            btnImg.color = color;
        }
    }
    private void OnMouseExit() {
        color.a = defaultAlpha;
        btnImg.color = color;
    }

}
