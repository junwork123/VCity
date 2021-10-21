using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class LobbyServer : NetServer
{



    // Start is called before the first frame update
    void Start()
    {
        //starts the server.
        base.StartServer();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //The connection method which is automatically run when a client connects to the server.
    public override void OnClientConnected(TcpClient client)
    {
        //not necessary
        base.OnClientConnected(client);


        foreach(int netid in netIds)
        {
            base.SendDirectMessage(client, "Joined:" + netid.ToString());
        }


    }

    //The method  which is automatically run when a method is recieved from a client.
    public override void OnMessageArrive(string message)
    {
        //Broadcasts messages to all clients.
        base.SendAllMessage(message);

    }

}
