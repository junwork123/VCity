using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Photon.Pun;
using UnityEngine.UI;

/// <summary>
/// Processing audio and video codec logic
/// See more: https://github.com/ShanguUncle/UnityChatSDK
/// </summary>
public class UnityChatDataHandler : Photon.Pun.MonoBehaviourPun, IPunObservable
{
    UnityChatDataHandler instance;
    //this uid used for testing, set your uid in specific application
    public int myID;
    public bool IsStartChat { get; set; }
    byte[] sendVideoBytes;
    byte[] sendAudioBytes;
    // public Renderer render;
    // public RawImage rawImage;

    Queue<VideoPacket> videoPacketQueue = new Queue<VideoPacket>();

    //temp save the data received from the your server and handler in update
    public Queue<byte[]> ReceivedAudioDataQueue = new Queue<byte[]>();
    public Queue<byte[]> ReceivedVideoDataQueue = new Queue<byte[]>();

    void Awake()
    {
        if (instance == null) instance = new UnityChatDataHandler();
        else Destroy(gameObject);

#if UNITY_EDITOR
        myID = 1001;
#else
        myID = 2001;
#endif
    }
    void Start()
    {

    }
    /// <summary>
    /// start video chat
    /// </summary>
    public void StartVideoChat()
    {
        GetComponent<UnityChatSet>().SetUnityCam();
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
        if (photonView.IsMine)
        {
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

    }
    private void Update()
    {
        lock (ReceivedAudioDataQueue)
        {
            if (ReceivedAudioDataQueue.Count > 0)
            {
                try
                {
                    OnReceiveAudio(ReceivedAudioDataQueue.Dequeue());
                    Debug.Log("오디오 수신에 성공했습니다");
                }
                catch (System.Exception)
                {
                    Debug.Log("오디오 시불쟝!!!!");
                    throw;
                }

            }
        }
        lock (ReceivedVideoDataQueue)
        {
            if (ReceivedVideoDataQueue.Count > 0)
            {
                try
                {
                    OnReceiveVideo(ReceivedVideoDataQueue.Dequeue());
                    Debug.Log("비디오 수신에 성공했습니다");
                }
                catch (System.Exception)
                {
                    Debug.Log("비디오 시불쟝!!!!");
                    throw;
                }
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
            packet.Id = myID;//use your userID
            sendAudioBytes = GetAudioPacketData(packet);
            Debug.Log("비디오 패킷 발송 준비중");
            //On receiving audio data,just for testing
            ReceivedAudioDataQueue.Enqueue(sendAudioBytes);
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

        packet.Id = myID;//use your userID
        sendVideoBytes = GetVideoPacketData(packet);
        Debug.Log("비디오 패킷 발송 준비중");

        //On receiving video data,just for testing
        ReceivedVideoDataQueue.Enqueue(sendVideoBytes);

    }
    byte[] GetVideoPacketData(VideoPacket packet)
    {
        //you can codec packet by google.protobuf/protobufNet...(the demo used google.protobuf)
        return ObjectToBytes(packet);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            byte[] temp = { 1, 2, 3, 4, 5 };
            stream.SendNext(temp);
            stream.SendNext(sendAudioBytes);
            stream.SendNext(sendVideoBytes);
            Debug.Log("비디오 보내는 중");
        }
        else
        {
            byte[] temp = (byte[])stream.ReceiveNext();
            ReceivedAudioDataQueue.Enqueue((byte[])stream.ReceiveNext());
            ReceivedVideoDataQueue.Enqueue((byte[])stream.ReceiveNext());

            foreach (var item in temp)
            {
                Debug.Log(item);
            }
            Debug.Log("비디오 받는 중");
        }
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
        Texture2D video = UnityChatSDK.Instance.DecodeVideoData(packet);
        // if (render != null)
        // {
        //     render.material.mainTexture = (Texture) video;
        // }
        // if (rawImage != null)
        // {
        //     rawImage.texture = (Texture) video; ;
        // }
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
