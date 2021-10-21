using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
using System.IO;

/// <summary>
/// Server class creates a Server based Network where all Clients
/// connect to the Server via ip and port. The Server accepts connections
/// and assignes server id's to each Client on start. Thereafter the Server
/// can be used to relay messages from a specific Client to all other Clients
/// on the network.
/// </summary>
public class NetServer : MonoBehaviour
{
    #region private members     
    /// <summary>   
    /// TCPListener to listen for incomming TCP connection  
    /// requests.   
    /// </summary>  
    private TcpListener tcpListener;
    /// <summary> 
    /// Background thread for TcpServer workload.   
    /// </summary>  
    private Thread tcpListenerThread;
    /// <summary>   
    /// Create handle to connected tcp client.  
    /// </summary>  
    private TcpClient connectedTcpClient;


    #endregion

    /// <summary>
    /// Create handle to connected tcpClients.
    /// </summary>
    private List<TcpClient> clientsList = new List<TcpClient>();

    /// <summary>
    /// The port the tcpServer runs over.
    /// </summary>
    public int port = 8052;

    private TcpClient pendingClient;

    [SerializeField]
    private bool debug = false;

    /// <summary>
    /// The buffer of incoming messages from TcpClients.
    /// </summary>
    [HideInInspector]
    public List<string> incomingBuffer = new List<string>();


    private bool serverStarted = false;

    public List<int> netIds = new List<int>();

    public bool beginServerOnStart = true;

    // Use this for initialization
    void Start()
    {
        if (beginServerOnStart)
        {
            StartServer();
        }
    }

    public void StartServer()
    {
        serverStarted = true;
        clientsList = new List<TcpClient>();
        // Start TcpServer background thread        
        tcpListenerThread = new Thread(new ThreadStart(ListenForIncommingRequests));
        tcpListenerThread.IsBackground = true;
        tcpListenerThread.Start();
        StartCoroutine(RunUpdate());

    }


    IEnumerator RunUpdate()
    {
        yield return new WaitForSeconds(0f);
        Update();
        StartCoroutine(RunUpdate());
    }

    // Update is called once per frame
    void Update()
    {

        if (serverStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space) && debug)
            {
                print(clientsList.Count);

                SendAllMessage("TestA");

            }
            try
            {
                if (tcpListener != null)
                {
                    bool isPending = tcpListener.Pending();
                    if (isPending)
                    {
                        if (debug)
                            print("pending connection:" + isPending);

                        TcpClient client = tcpListener.AcceptTcpClient();
                        clientsList.Add(client);
                        connectedTcpClient = client;
                        if (debug)
                            print("new client connected");
                        pendingClient = client;
                        Thread thread = new Thread(new ThreadStart(ReadPendingListener));
                        thread.Start();

                    }
                }

            }
            catch (InvalidOperationException e)
            {
                print(e.Message);
            }
        }



    }



    private void ReadPendingListener()
    {
        TcpClient client = pendingClient;
        this.OnClientConnected(client);
        NetworkStream stream = client.GetStream();
        int length;
        Byte[] bytes = new byte[1024];
        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
        {
            var incommingData = new byte[length];
            Array.Copy(bytes, 0, incommingData, 0, length);
            // Convert byte array to string message.                            
            string clientMessage = Encoding.ASCII.GetString(incommingData);
            if (debug)
                Debug.Log("client message received as: " + clientMessage);
            bool isInitializing = CheckForInitialization(clientMessage);
            if (!isInitializing)
            {
                incomingBuffer.Add(clientMessage);
                this.OnMessageArrive(clientMessage);
            }

        }
    }
    public static string GetInternalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());

        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }

        throw new Exception("No network adapters with an IPv4 address in the system!");
    }

    public static string GetExternalIPAddress()
    {
        string externalip = new WebClient().DownloadString("http://ipinfo.io/ip").Trim(); //http://icanhazip.com

        if (String.IsNullOrWhiteSpace(externalip))
        {
            externalip = GetInternalIPAddress();//null경우 Get Internal IP를 가져오게 한다.
        }

        return externalip;
    }
    /// <summary>   
    /// Runs in background TcpServerThread; Handles incomming TcpClient requests    
    /// </summary>  
    private void ListenForIncommingRequests()
    {
        try
        {
            if (debug)
                print("Using ipv4:" + System.Net.IPAddress.Any);
            // Create listener on localhost port 8052.  

            tcpListener = new TcpListener(IPAddress.Any, port);

            Socket s = tcpListener.Server;
            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 1000);
            // The socket will linger for 10 seconds after Socket.Close is called.
            LingerOption lingerOption = new LingerOption(true, 10);

            s.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, lingerOption);

            tcpListener.Start();
            Debug.Log("Server is listening");
            Byte[] bytes = new Byte[1024];

            while (true)
            {

                using (connectedTcpClient = tcpListener.AcceptTcpClient())
                {
                    print("found connection");
                    if (!clientsList.Contains(connectedTcpClient))
                    {
                        clientsList.Add(connectedTcpClient);
                        this.OnClientConnected(connectedTcpClient);
                    }
                    // Get a stream object for reading                  
                    using (NetworkStream stream = connectedTcpClient.GetStream())
                    {

                        int length;
                        // Read incomming stream into byte arrary.                      
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            var incommingData = new byte[length];
                            Array.Copy(bytes, 0, incommingData, 0, length);
                            // Convert byte array to string message.                            
                            string clientMessage = Encoding.ASCII.GetString(incommingData);
                            if (debug)
                                Debug.Log("client message received as: " + clientMessage);


                            bool isInitializing = CheckForInitialization(clientMessage);

                            if (!isInitializing)
                            {
                                incomingBuffer.Add(clientMessage);
                                this.OnMessageArrive(clientMessage);
                            }
                        }


                    }

                }

            }
        }
        catch (Exception socketException)
        {
            Debug.Log("SocketException " + socketException.ToString());
        }
    }
    /// <summary>   
    /// Send message to client using socket connection.     
    /// </summary>  
    private void SendMessage()
    {
        if (connectedTcpClient == null)
        {
            return;
        }

        try
        {
            // Get a stream object for writing.             
            NetworkStream stream = connectedTcpClient.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = "This is a message from your server.";
                // Convert string message to byte array.                 
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                // Write byte array to socketConnection stream.               
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                if (debug)
                    Debug.Log("Server sent his message - should be received by client");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void SendDirectMessage(TcpClient client, string customMsg = "")
    {
        if (client == null)
            return;
        try
        {
            // Get a stream object for writing.             
            NetworkStream stream = client.GetStream();
            if (stream.CanWrite)
            {
                string serverMessage = customMsg + NetParser.splitter;
                // Convert string message to byte array.                 
                byte[] serverMessageAsByteArray = Encoding.ASCII.GetBytes(serverMessage);
                // Write byte array to socketConnection stream.               
                stream.Write(serverMessageAsByteArray, 0, serverMessageAsByteArray.Length);
                if (debug)
                    Debug.Log("Server sent his message - should be received by client");
            }
        }
        catch (Exception socketException)
        {
            if (socketException is IOException)
            {
                if (debug)
                    print("Client has disconnected");

                int id = this.clientsList.IndexOf(client);

                this.clientsList.Remove(client);

                this.OnClientDisconnected(client, id);
                SendAllMessage(NetParser.Join("dcx1234##:" + id));


            }

            Debug.Log("Socket exception: " + socketException.Message);
        }
    }
    public void SendBytes(byte[] bytes)
    {
        if (clientsList == null && clientsList.Count == 0)
            return;
        TcpClient client = clientsList[0];
        try
        {
            // Get a stream object for writing.             
            NetworkStream stream = client.GetStream();
            if (stream.CanWrite)
            {
                // Write byte array to socketConnection stream.               
                stream.Write(bytes, 0, bytes.Length);
                if (debug)
                    Debug.Log("Server sent his message - should be received by client");
            }
        }
        catch (Exception socketException)
        {
            if (socketException is IOException)
            {
                if (debug)
                    print("Client has disconnected");

                int id = this.clientsList.IndexOf(client);

                this.clientsList.Remove(client);

                this.OnClientDisconnected(client, id);
                SendAllMessage(NetParser.Join("dcx1234##:" + id));
            }

            Debug.Log("Socket exception: " + socketException.Message);
        }
    }
    /// <summary>
    /// Sends all clients a message.
    /// </summary>
    /// <param name="message">Message being sent to all connected clients.</param>
    public void SendAllMessage(string message)
    {
        try
        {
            if (!serverStarted)
            {
                throw new InvalidOperationException("Server not yet started.");
            }
            foreach (TcpClient client in clientsList)
            {
                SendDirectMessage(client, message);
            }

        }
        catch (Exception e)
        {
            if (e is InvalidOperationException) { }

            if (e is SocketException)
                Debug.Log("Socket exception: " + e.Message);
        }

    }

    /// <summary>
    /// Checks if client is requesting initilization. If the client is, the netId is assigned
    /// [based on clientCount] and then the id is added to the rest of the message.
    /// </summary>
    /// <returns><c>true</c>, if for initialization was checked, <c>false</c> otherwise.</returns>
    /// <param name="message">Message.</param>
    private bool CheckForInitialization(string message)
    {
        if (debug)
            print("Checking for initilization request:" + message);
        message = message.Replace(NetParser.splitter, "");
        string[] parsedMessage = NetParser.Parse(message);

        if (parsedMessage[0].Equals("Initialization"))
        {

            if (parsedMessage.Length == 2)
            {
                this.netIds.Add(clientsList.Count);
                SendAllMessage(NetParser.JoinForParse(parsedMessage[0], parsedMessage[1].Replace(NetParser.parser, ""), clientsList.Count.ToString()));

                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }

    }

    /// <summary>
    /// Method is run once a message is recieved by the server.
    /// </summary>
    /// <param name="message">Message that was recieved from a client.</param>
    public virtual void OnMessageArrive(string message)
    {
        return;
    }

    /// <summary>
    /// Method is used once a TcpClient object connects to the server.
    /// </summary>
    /// <param name="client">Client that connected.</param>
    public virtual void OnClientConnected(TcpClient client)
    {

    }
    /// <summary>
    /// Method is used once a TcpClient object disconnects from the server.
    /// </summary>
    /// <param name="client">Client that disconnected.</param>
    public virtual void OnClientDisconnected(TcpClient client, int netId)
    {

    }

    /// <summary>
    /// The number of connections established.
    /// </summary>
    /// <returns>The number of connections as an int.</returns>
    public int ConnectionCount()
    {
        return this.clientsList.Count;
    }

    /// <summary>
    /// Checks whether the server has started yet.
    /// </summary>
    /// <returns><c>true</c>, if the server has started, <c>false</c> otherwise.</returns>
    public bool ServerStarted()
    {
        return this.serverStarted;
    }

    //Quits socket when application is quit out of.
    private void OnApplicationQuit()
    {
        this.tcpListener.Stop();
    }

    /// <summary>
    /// Stops the server.
    /// </summary>
    public void StopServer()
    {
        this.tcpListener.Stop();
    }

}
