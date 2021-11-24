using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QRTimer : MonoBehaviour
{
    public int refreshTime = 15;
    public TextMeshProUGUI timerText;

    private void OnEnable()
    {
        StartCoroutine(QRTimerCoroutine());
    }

    private void OnDisable()
    {
        StopCoroutine(QRTimerCoroutine());
    }

    IEnumerator QRTimerCoroutine()
    {
        int time = refreshTime;
        while (true)
        {
            timerText.text = "남은 시간 <color=red>" + time + "초</color>";

            yield return new WaitForSeconds(1);

            --time;
            if (time < 0)
            {
                time = refreshTime;

                // TODO Change QRImage
            }
        }
    }
}
