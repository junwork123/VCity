using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI timeText;
    public float time;
 
    // private void Awake () {
    //     time = 180f;
    // }
 
    private void Update () {
        time -= Time.deltaTime;
        timeText.text = "Remain " + $"{time:N2}" + "s";
    }
}
