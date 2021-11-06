using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServiceImage : MonoBehaviour
{
    public static ServiceImage Instance;
    public Sprite[] servicesImg;
    private void Awake()
    {
        Instance = this;
    }
    public Sprite GetServiceImg(int _serviceNum)
    {
        return servicesImg[_serviceNum];
    }
}
