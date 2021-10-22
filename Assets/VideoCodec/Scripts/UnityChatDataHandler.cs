using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

/// <summary>
/// Processing audio and video codec logic
/// See more: https://github.com/ShanguUncle/UnityChatSDK
/// </summary>
public class UnityChatDataHandler : MonoBehaviour
{

    //this uid used for testing, set your uid in specific application
    public int TestUid = 1001;
    public bool IsStartChat { get; set; }

    public string componentIp = "";

    Queue<VideoPacket> videoPacketQueue = new Queue<VideoPacket>();

    //temp save the data received from the your server and handler in update
    public Queue<byte[]> ReceivedAudioDataQueue = new Queue<byte[]>();
    public Queue<byte[]> ReceivedVideoDataQueue = new Queue<byte[]>();
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

    public void SetAsServer()
    {
        // IPv4, 스트림, TCP --> TCP는 스트림으로, UDP는 데이터그램으로
        using (Socket svrSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(GetExternalIPAddress()), 9999);

            svrSocket.Bind(endPoint);

            byte[] recvBytes = new byte[1024];
            EndPoint cltEP = new IPEndPoint(IPAddress.Parse(componentIp), 0);


            while (true)
            {
                int nRecv = svrSocket.ReceiveFrom(recvBytes, ref cltEP); // 받은 문자 바이트
                string text = Encoding.UTF8.GetString(recvBytes, 0, nRecv);

                Console.WriteLine(text);
                byte[] sendBytes = Encoding.UTF8.GetBytes("Hello: " + text); // 전송

                svrSocket.SendTo(sendBytes, cltEP);
            }

        }
    }
    public void SetAsClient()
    {
        using (Socket cltSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
        {
            EndPoint ServerPoint = new IPEndPoint(IPAddress.Parse(componentIp), 9999);
            EndPoint SendPoint = new IPEndPoint(IPAddress.Parse(GetExternalIPAddress()), 0);

            byte[] sendByte = Encoding.UTF8.GetBytes("I'm udp client");

            cltSocket.SendTo(sendByte, ServerPoint);
            byte[] recvBytes = new byte[1024];
            int nRecv = cltSocket.ReceiveFrom(recvBytes, ref SendPoint);

            string text = Encoding.UTF8.GetString(recvBytes, 0, nRecv);
            Console.WriteLine(text);
            cltSocket.Close(); // 세션 종료
        }
    }
    void Start()
    {

    }
    /// <summary>
    /// start video chat
    /// </summary>
    public void StartVideoChat()
    {
        OnStartChat(ChatType.Video);
    }
    /// <summary>
    /// start audio chat
    /// </summary>
    public void StartAudioChat()
    {
        OnStartChat(ChatType.Audio);
    }

    void OnStartChat(ChatType type)
    {
        try
        {
            UnityChatSDK.Instance.ChatType = type;

            CaptureResult result = UnityChatSDK.Instance.StartCapture();
            Debug.Log("StartChat:" + result);
            IsStartChat = true;
            Debug.Log("[Record] : " + "OnStartChat");
        }
        catch (Exception e)
        {
            Debug.Log("[Record] : " + "OnStartChat error:" + e.Message);
        }
    }

    /// <summary>
    ///Stop chat
    /// </summary>
    public void StopChat()
    {
        StartCoroutine(OnStopChat());
    }

    IEnumerator OnStopChat()
    {
        yield return new WaitForEndOfFrame();
        try
        {
            UnityChatSDK.Instance.StopCapture();
            videoPacketQueue.Clear();
            ReceivedAudioDataQueue.Clear();
            ReceivedVideoDataQueue.Clear();
            IsStartChat = false;
            Debug.Log("[Record] : " + "OnStopChat");
        }
        catch (Exception e)
        {
            Debug.Log("[Record] : " + "OnStopChat error:" + e.Message);
        }
    }


    // after starting chat, in FixedUpdate the capture of audio and video will be transmitted via your own network
    // note: The value of FixedUpdate Time needs to be less than 1 / (Framerate + 5), which can be set to 0.025 or less
    void FixedUpdate()
    {
        if (!IsStartChat)
            return;

        switch (UnityChatSDK.Instance.ChatType)
        {
            case ChatType.Audio:
                SendAudio();
                break;
            case ChatType.Video:
                SendAudio();
                SendVideo();
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        lock (ReceivedAudioDataQueue)
        {
            if (ReceivedAudioDataQueue.Count > 0)
            {
                OnReceiveAudio(ReceivedAudioDataQueue.Dequeue());
            }
        }
        lock (ReceivedVideoDataQueue)
        {
            if (ReceivedVideoDataQueue.Count > 0)
            {
                OnReceiveVideo(ReceivedVideoDataQueue.Dequeue());
            }
        }
    }
    //==================send data========================
    /// <summary>
    /// send audio data
    /// </summary>
    void SendAudio()
    {
        //capture audio data by SDK
        AudioPacket packet = UnityChatSDK.Instance.GetAudio();
        if (packet != null)
        {
            packet.Id = TestUid;//use your userID
            byte[] audio = GetAudioPacketData(packet);

            if (audio != null)
            {
                //send data through your own network,such as TCP，UDP，P2P,Webrct，Unet,Photon...,the demo uses UDP for testing.
                SendDataByYourNetwork(audio);

                //On receiving audio data,just for testing
                //ReceivedAudioDataQueue.Enqueue(audio);
            }
        }
    }
    byte[] GetAudioPacketData(AudioPacket packet)
    {
        //you can codec packet by google.protobuf/protobufNet...(the demo used google.protobuf)
        return ObjectToBytes(packet);
    }

    /// <summary>
    /// send video data
    /// </summary>
    void SendVideo()
    {
        //capture video data by SDK
        VideoPacket packet = UnityChatSDK.Instance.GetVideo();
        if (packet == null || packet.Data == null || packet.Data.Length == 0) return;

        if (UnityChatSDK.Instance.EnableSync)
        {
            videoPacketQueue.Enqueue(packet);

            if (videoPacketQueue.Count >= UnityChatSDK.Instance.Framerate / UnityChatSDK.Instance.AudioSample)
            {
                packet = videoPacketQueue.Dequeue();
            }
            else
            {
                return;
            }
        }

        packet.Id = TestUid;//use your userID
        byte[] video = GetVideoPacketData(packet);
        SendDataByYourNetwork(video);

        //On receiving video data,just for testing
        //ReceivedVideoDataQueue.Enqueue(video);
    }
    byte[] GetVideoPacketData(VideoPacket packet)
    {
        //you can codec packet by google.protobuf/protobufNet...(the demo used google.protobuf)
        return ObjectToBytes(packet);
    }

    //==================onReceive data========================
    /// <summary>
    /// called when audio data is received
    /// </summary>
    /// <param name="data"></param>
    public void OnReceiveAudio(byte[] data)
    {
        //decode audio data and playback
        AudioPacket packet = DecodeAudioPacket(data);
        UnityChatSDK.Instance.DecodeAudioData(packet);
    }
    AudioPacket DecodeAudioPacket(byte[] data)
    {
        //decode bytes to packet
        return BytesToObject<AudioPacket>(data);
    }

    /// <summary>
    /// called when video data is received
    /// </summary>
    /// <param name="data"></param>
    public void OnReceiveVideo(byte[] data)
    {
        //decode video data and render video
        VideoPacket packet = DecodeVideoPacket(data);
        UnityChatSDK.Instance.DecodeVideoData(packet);
    }
    VideoPacket DecodeVideoPacket(byte[] data)
    {
        //decode bytes to packet
        return BytesToObject<VideoPacket>(data);
    }

    /// <summary>
    /// data serialization
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t"></param>
    /// <returns></returns>
    public static byte[] ObjectToBytes<T>(T t)
    {
        if (t == null) return null;
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream();
        formatter.Serialize(stream, t);
        return stream.ToArray();
    }
    /// <summary>
    /// data deserialization
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    public static T BytesToObject<T>(byte[] data)
    {
        if (data == null) return default(T);
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream stream = new MemoryStream(data);
        return (T)formatter.Deserialize(stream);
    }

}
