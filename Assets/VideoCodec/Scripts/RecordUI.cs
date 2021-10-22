using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordUI : MonoBehaviour
{
    public GameObject server;
    public GameObject client;
    public GameObject myRecord;
    public GameObject componentRecord;

    public Text iptext;
    void Awake() {
        DontDestroyOnLoad(this);
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("[Record] : " + NetServer.GetExternalIPAddress());
        //CloseConnection();
    }
    public void OpenRecordUI()
    {
        myRecord.SetActive(true);
        componentRecord.SetActive(true);
    }
    public void CloseRecordUI()
    {
        myRecord.SetActive(false);
        componentRecord.SetActive(false);
    }
    public void SetAsServer()
    {
        Debug.Log("SetAsServer");
        server.SetActive(true);
        client.SetActive(false);
        //server.GetComponent<MySimpleServer>().StartServer();
        // GetComponent<UnityChatSet>().CaptureCamera = Camera.main;
        // GetComponent<UnityChatSet>().SetUnityCam();
        GetComponent<UnityChatSet>().SetDeciveCam();
        GetComponent<UnityChatDataHandler>().StartVideoChat();
        iptext.text = NetServer.GetExternalIPAddress();
    }
    public void SetAsClient(string _serverAddress)
    {
        Debug.Log("SetAsClient");
        server.SetActive(false);
        client.SetActive(true);
        GetComponent<UnityChatSet>().CaptureCamera = Camera.main;
        GetComponent<UnityChatSet>().SetUnityCam();
        client.GetComponent<MySimpleClient>().serverIP = _serverAddress;
        //client.GetComponent<MySimpleClient>().StartClient();
        GetComponent<UnityChatDataHandler>().StartVideoChat();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
