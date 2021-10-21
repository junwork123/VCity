using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class MySimpleServer : NetServer
{
    // Start is called before the first frame update
    void Start()
    {
        base.StartServer();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnClientConnected(TcpClient client)
    {
        print("Client connected");
        base.OnClientConnected(client);

    }

    public override void OnMessageArrive(string message)
    {

        base.OnMessageArrive(message);
        ParseMessage(message);

    }

    public override void OnClientDisconnected(TcpClient client, int netId)
    {
        base.OnClientDisconnected(client, netId);


    }

    //Different kinds of messages:
    /*
     * UserName [name]:Message [message]
    */
    private void ParseMessage(string message)
    {
        SendAllMessage(message);
    }
    
}
