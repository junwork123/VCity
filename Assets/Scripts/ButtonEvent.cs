using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{
    public PlayerController playerController;


    public void OnClickButtonX()
    {
        playerController.Interaction();
    }

    public void OnClickButtonY()
    {
        print("Click Y");
    }
}
