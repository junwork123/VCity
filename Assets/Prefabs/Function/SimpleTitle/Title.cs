using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject titleImg;
    public Image blinkImg;
    private bool checkPressed;
    void Start()
    {
        iTween.ScaleTo(titleImg, iTween.Hash(
                "x", 12,
                "y", 12,
                "time", 1.5f,
                "easetype", iTween.EaseType.easeOutBounce
            ));
        InvokeRepeating("Blink", 2f, 1f);
        checkPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey && checkPressed == false)
        {
            checkPressed = true;
            CancelInvoke("Blink");
            blinkImg.enabled = false;
            SceneManager.LoadScene("MainMenu");
        }
    }

    void Blink()
    {
        blinkImg.enabled = !blinkImg.enabled;
    }
}
