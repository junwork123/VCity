using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class MySimpleClient : NetClient
{
    public string userId;
    public string serverIP;
    public GameObject recordManager;
    private Dictionary<int, string> idToName = new Dictionary<int, string>();

    // Start is called before the first frame update
    void Start()
    {
        StartClient();
    }
    public void SetIpAdress(string _ipAddress)
    {
        ipAddress = _ipAddress;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public override void OnConnectedToServer(int networkId)
    {
        base.OnConnectedToServer(networkId);
        SendNetMessage(userId + " has joined the chat!");
        this.idToName.Add(networkId, userId);
    }

    public override void OnMessageReceive(byte[] message)
    {
        recordManager.GetComponent<UnityChatDataHandler>().ReceivedVideoDataQueue.Enqueue(message);
        //base.OnMessageReceive(message);
        // string formattedMessage = message;
        // if (message.Contains("Message"))
        //     formattedMessage = message.Remove(0, 8);

    }

    public override void OnUserDisconnectFromServer(int networkId)
    {
        base.OnUserDisconnectFromServer(networkId);
    }


    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
}
