using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoCallWaitLine : MonoBehaviour
{
    [Header("TextMesh")]
    [SerializeField]
    TextMeshProUGUI waitNumberText;
    [SerializeField]
    TextMeshProUGUI remainNumberText;

    [SerializeField]
    GameObject videoCallPanel;
    [SerializeField]
    float delayToPanelChange = 3f;

    int currentNum;



    [Header("Test")]
    public int waitNum;
    public int remainNum;
    [SerializeField]
    int delayTimeRange;

    // Start is called before the first frame update
    void OnEnable()
    {
        waitNumberText.text = waitNum.ToString();

        UpdateRemainNumberUI();
        StartCoroutine(PopWait());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateRemainNumberUI()
    {
        remainNumberText.text = "앞으로 <color=#FF7900>" + remainNum + "</color>명 남았습니다.";
    }

    IEnumerator PopWait()
    {
        while (remainNum > 0)
        {
            int delay = Random.Range(1, delayTimeRange);
            yield return new WaitForSeconds(delay);

            --remainNum;
            UpdateRemainNumberUI();
        }

        yield return new WaitForSeconds(delayToPanelChange);
        UIManager.instance.OpenPanel(videoCallPanel);
        UIManager.instance.ClosePanel(gameObject);
    }
}
