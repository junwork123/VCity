using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordUI : MonoBehaviour
{
    // public GameObject myRecord;
    // public GameObject componentRecord;

    public Text iptext;
    void Awake() {
        DontDestroyOnLoad(this);
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        //Debug.Log("[Record] : " + NetServer.GetExternalIPAddress());
        //CloseConnection();
    }
    public void DisplayIp(){
        iptext.text = UnityChatDataHandler.GetExternalIPAddress();
    }
    // public void OpenRecordUI()
    // {
    //     myRecord.SetActive(true);
    //     componentRecord.SetActive(true);
    // }
    // public void CloseRecordUI()
    // {
    //     myRecord.SetActive(false);
    //     componentRecord.SetActive(false);
    // }
    // Update is called once per frame
    void Update()
    {

    }
}
