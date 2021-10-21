using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;

/// <summary>
/// Client class is used to connect to a Server via ip and port. On connection
/// the server assigns a netId to the client. All other objects can be tracked
/// via their Id number. All messages relayed from the server are stacked in
/// a buffer.
/// The Client class can also send messages to the Server. This message will
/// be relayed accross the network and will be relayed back into the buffer.
/// </summary>
/// 
public class NetClient : MonoBehaviour
{
    #region private members     
    private TcpClient socketConnection;
    private Thread clientReceiveThread;
    #endregion

    #region
    [Header("Debug:")]
    private string outboundMessage = "";
    public bool debug = false;

    [Header("Connection:")]
    public string ipAddress  = "localhost";
    public int port = 8052;

    [HideInInspector]
    public bool initializedOnNetwork = false;

    private int initializationID;
    [HideInInspector]
    public int networkID;

    [HideInInspector]
    private List<string> incomingBuffer = new List<string>();

    private bool clientStarted = false;
    public bool beginClientOnStart = true;
    private string incomingTotal = "";
    #endregion
    // Use this for initialization  
    void Start()
    {
        if (beginClientOnStart)
            StartClient();
    }

    public void StartClient()
    {
        clientStarted = true;
        ConnectToTcpServer();
        InitializeOnNetwork();
        StartCoroutine(loopUpdate());
    }

    private IEnumerator loopUpdate()
    {

        yield return new WaitForSeconds(0f);
        if (!initializedOnNetwork)
        {
            InitializeOnNetwork();

        }
        while (this.HasMessage())
        {
            string serverMessage = this.GetNextMessage();
            if (serverMessage != null)
            {
                if (incomingTotal.Length >= NetParser.splitter.Length)
                {
                    print(incomingTotal.Substring(0, NetParser.splitter.Length));
                    if (incomingTotal.Substring(0, NetParser.splitter.Length) == NetParser.splitter)
                    {

                        incomingTotal = incomingTotal.Remove(0, NetParser.splitter.Length);

                    }
                }


                incomingTotal += serverMessage;
                if (NetParser.Split(incomingTotal).Length > 1 || incomingTotal.Contains(NetParser.splitter))
                {

                    ReadIncomingMessage(NetParser.Split(incomingTotal)[0].Replace(NetParser.splitter, ""), true);
                    if (NetParser.Split(incomingTotal)[0].Contains(NetParser.splitter))
                    {
                        incomingTotal = incomingTotal.Remove(0, NetParser.Split(incomingTotal)[0].Length + NetParser.splitter.Length);

                    }
                    else
                    {
                        incomingTotal = incomingTotal.Remove(0, NetParser.Split(incomingTotal)[0].Length);

                    }
                }
                else
                {
                    ReadIncomingMessage(serverMessage);
                }


            }


        }
        StartCoroutine(loopUpdate());

    }

    void ReadIncomingMessage(string serverMessage, bool checkIfComplete = false)
    {
        print("incoming:" + serverMessage);

        if (serverMessage.Contains(":") || true && serverMessage.Length > 0)
        {

            if (NetParser.Split(serverMessage)[0].Equals("Initialization") ||
            NetParser.Parse(serverMessage)[0].Equals("Initialization"))
            {

            }
            else if (NetParser.Split(serverMessage)[0].Split(':')[0].Equals("dcx1234##"))
            {
                int networkId = int.Parse(NetParser.Split(serverMessage)[0].Split(':')[1]);
                OnUserDisconnectFromServer(networkId);
            }
            else
            {

                this.OnMessageReceive(serverMessage);
            }
        }
        else
        {
            this.OnMessageReceive(serverMessage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (clientStarted)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendNetMessage();
            }
            if (!initializedOnNetwork)
            {
                InitializeOnNetwork();
            }

        }

    }
    /// <summary>   
    /// Setup socket connection.    
    /// </summary>  
    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.Log("On client connect exception " + e);
        }
    }
    /// <summary>   
    /// Runs in background clientReceiveThread; Listens for incomming data.     
    /// </summary>     
    private void ListenForData()
    {
        try
        {
            if (debug)
                print("Connecting to:" + ipAddress);
            socketConnection = new TcpClient(ipAddress, port);
            Byte[] bytes = new Byte[1024];
            while (true)
            {
                // Get a stream object for reading              
                using (NetworkStream stream = socketConnection.GetStream())
                {
                    int length;
                    // Read incomming stream into byte arrary.                  
                    while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        var incommingData = new byte[length];
                        Array.Copy(bytes, 0, incommingData, 0, length);
                        // Convert byte array to string message.                        
                        string serverMessage = Encoding.ASCII.GetString(incommingData);
                        if (debug)
                            Debug.Log("server message received as: " + serverMessage);
                        Debug.Log("initialized?:" + initializedOnNetwork);


                        foreach (string msg in NetParser.Split(serverMessage))
                        {
                            if (!initializedOnNetwork)
                            {
                                CheckForNetIdAssignment(msg);

                            }
                            else
                            {

                                incomingBuffer.Add(msg);

                            }
                        }


                    }
                }
            }
        }
        catch (Exception socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }
    /// <summary>   
    /// Send message to server using socket connection.     
    /// </summary>  
    public void SendNetMessage(string input = "")
    {
        if (socketConnection == null)
        {
            return;
        }
        if (!clientStarted)
        {
            throw new InvalidOperationException("Client not yet started.");
        }
        try
        {
            // Get a stream object for writing.             
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                string clientMessage = "This is a message from one of your clients.";
                clientMessage = input;
                if (input.Length == 0)
                    clientMessage = this.outboundMessage;
                clientMessage += NetParser.splitter;
                // Convert string message to byte array.                 
                byte[] clientMessageAsByteArray = Encoding.ASCII.GetBytes(clientMessage);
                // Write byte array to socketConnection stream.                 
                stream.Write(clientMessageAsByteArray, 0, clientMessageAsByteArray.Length);
                if (debug)
                    Debug.Log("Client sent his message - should be received by server");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }

    public void SendBytes(byte[] bytes)
    {
        if (socketConnection == null)
        {
            return;
        }
        if (!clientStarted)
        {
            throw new InvalidOperationException("Client not yet started.");
        }
        try
        {
            // Get a stream object for writing.             
            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                // Write byte array to socketConnection stream.                 
                stream.Write(bytes, 0, bytes.Length);
                if (debug)
                    Debug.Log("Client sent his message - should be received by server");
            }
        }
        catch (SocketException socketException)
        {
            Debug.Log("Socket exception: " + socketException);
        }
    }


    /// <summary>
    /// Sends random initialization id to server in id generation process.
    /// Server responds with initialization id and networkId assigned.
    /// Message sent: "Initialization:[random initializationId]"
    /// </summary>
    private void InitializeOnNetwork()
    {
        this.initializationID = UnityEngine.Random.Range(0, 100000);
        string message = NetParser.JoinForParse("Initialization", initializationID.ToString());
        this.SendNetMessage(message);
        if (debug)
            Debug.Log("Initializing on network");
    }





    /// <summary>
    /// Checks for net identifier assignment. If message format is:
    /// "Initialization:[random initilization id]:[netId]"
    /// then the netId will be assigned and initializedOnNetwork = true.
    /// </summary>
    /// <param name="message">Message.</param>
    private void CheckForNetIdAssignment(string message)
    {

        string[] parsedMessage = NetParser.Parse(message);
        print("Parsed 0:" + parsedMessage[0]);
        if (parsedMessage[0] == "Initialization" && parsedMessage.Length == 3)
        {
            print(parsedMessage[2]);
            networkID = int.Parse(parsedMessage[2]);

            this.initializedOnNetwork = true;
            this.OnConnectedToServer(networkID);

        }


    }

    /// <summary>
    /// Gets the next message in the buffer.
    /// </summary>
    /// <returns>The next message in the buffer.</returns>
    public string GetNextMessage()
    {
        if (HasMessage())
        {
            string message = this.incomingBuffer[0];
            this.incomingBuffer.RemoveAt(0);
            return message;
        }
        return "";
    }

    /// <summary>
    /// Checks whether there is a message in the buffer.
    /// </summary>
    /// <returns><c>true</c>, if buffer has a message, <c>false</c> otherwise.</returns>
    public bool HasMessage()
    {
        return this.incomingBuffer.Count > 0;
    }

    /// <summary>
    /// Method run when a message from the server is recieved.
    /// </summary>
    /// <param name="message">Message from server.</param>
    public virtual void OnMessageReceive(string message)
    {
        return;
    }

    /// <summary>
    /// Method run when this client connects to a server.
    /// </summary>
    /// <param name="networkId">Network identifier given by the server.</param>
    public virtual void OnConnectedToServer(int networkId)
    {
        return;
    }

    public virtual void OnUserDisconnectFromServer(int networkId)
    {
        return;
    }

    //closes socket connection when application is quit.
    private void OnApplicationQuit()
    {
        try
        {
            this.socketConnection.Close();
        }
        catch (Exception e)
        {
            Debug.Log("Disconnected from server.");
        }

    }

    /// <summary>
    /// Closes the TcpClient socket connection.
    /// </summary>
    public void Disconnect()
    {
        try
        {
            this.socketConnection.Close();

        }
        catch (SocketException e)
        {
            if (debug)
                print("Disconnected from server.");
        }

    }
}
