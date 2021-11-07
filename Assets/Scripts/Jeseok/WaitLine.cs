using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaitLine : MonoBehaviour
{
    [Header("TextMesh")]
    [SerializeField]
    TextMeshProUGUI waitNumberText;
    [SerializeField]
    TextMeshProUGUI remainNumberText;

    [SerializeField]
    GameObject nextPanel;
    [SerializeField]
    float delayToPanelChange = 3f;


    [Header("Test")]
    public int waitNum;
    [SerializeField]
    int startNum;
    public int remainNum;
    [SerializeField]
    int delayTimeRange;

    // Start is called before the first frame update
    void OnEnable()
    {
        remainNum = startNum;
        waitNumberText.text = waitNum.ToString();

        UpdateRemainNumberUI();
        StartCoroutine(PopWait());
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

        UIManager.instance.OpenPanel(nextPanel);
        UIManager.instance.ClosePanel(gameObject);
    }
}
