using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class loadingbar : MonoBehaviour
{

    private RectTransform rectComponent;
    private Image imageComp;
    public float speed;

    public float lowFillAmount;


    // Use this for initialization
    void Start()
    {
        rectComponent = GetComponent<RectTransform>();
        imageComp = rectComponent.GetComponent<Image>();
        imageComp.fillAmount = lowFillAmount;
    }

    void Update()
    {
        if (imageComp.fillAmount != 1f)
        {
            imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;

        }
        else
        {
            imageComp.fillAmount = lowFillAmount;

        }
    }
}
