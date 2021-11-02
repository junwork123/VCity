using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class CheckBox : MonoBehaviour
{
    public Image activeImg;
    public Image inactiveImg;

    private void Start()
    {
        inactiveImg.gameObject.SetActive(true);
        activeImg.gameObject.SetActive(false);
        GetComponent<Toggle>().targetGraphic = inactiveImg;
    }
    public void SetActive()
    {
        activeImg.gameObject.SetActive(inactiveImg.gameObject.activeSelf);
        inactiveImg.gameObject.SetActive(!activeImg.gameObject.activeSelf);
    }

}
