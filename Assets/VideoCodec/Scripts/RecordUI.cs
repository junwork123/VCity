using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordGUI : MonoBehaviour
{
    public MySimpleServer server;
    public MySimpleClient client;
    public GameObject myRecord;
    public GameObject componentRecord;

    // Start is called before the first frame update
    void Start()
    {
        CloseConnection();
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
        server.enabled = true;
        client.enabled = false;

    }
    public void SetAsClient(string _serverAddress)
    {
        server.enabled = false;
        client.enabled = true;
        client.ipAddress = _serverAddress;
    }
    public void CloseConnection()
    {
        server.enabled = false;
        client.enabled = false;
        CloseRecordUI();
    }
    // Update is called once per frame
    void Update()
    {

    }
}
