using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Photon.Chat;
using ExitGames.Client.Photon;
using Firebase.Auth;
using Firebase.Database;

public class DataManager : MonoBehaviour, IChatClientListener
{
    public static DataManager instance;
    // 데이터 매니저는 싱글톤으로 존재
    public UserDataContainer udc { get; set; }
    void Awake()
    {
        if (instance == null) instance = new DataManager();
        else Destroy(gameObject);

    }
    void Start()
    {

        #region @Test 테스트용 코드(추후 삭제)
        // udc = new UserDataContainer();
        // udc.userId = "1234";
        // udc.userName = "Bob";
        // udc.channels["Region"].Add(new CustomMsg("Joe", "[yyyy-MM-dd HH:mm]", "Hello world!"));
        // DataSaveText(udc, "1234");
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }
    public UserDataContainer LoadDataWithId(string _id)
    {
        // TODO : Firebase DB와 연결하여 정보 받아오기
        udc = LoadFromFile<UserDataContainer>(_id);
        // 처음 접속하는 유저일 경우 빈 UDC가 반환되므로
        // 이름을 다시 설정해준다
        if (udc == null)
        {
            udc = new UserDataContainer(_id);
            SaveAsFile<UserDataContainer>(udc, udc.userId);
        }

        return udc;
    }

    public void UpdateMsg(string _channelName, string _chatContents)
    {
        // 마지막 메시지가 같지 않다면
        // 내용을 업데이트
        // if(_chatContents.EndsWith(udc.channels[_channelName][-1].ToString()) == false){
        //     _chatContents
        // }

        // udc.channels[_channelName] = 
    }
    public void AppendMsg(string _channelName, CustomMsg _msg)
    {
        udc.channels[_channelName].Add(_msg);
        SaveAsFile(udc, udc.userId);
        Debug.Log("append received Messages");
        return;
    }

    public void SaveAsFile<T>(T data, string userId)
    {
        string path = Application.persistentDataPath + "/UserJson";

        try
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, userId), FileMode.Create);
            //string jsonData = JsonUtility.ToJson(data, true);
            string jsonData = ObjectToJson(data);
            byte[] jsonbytes = Encoding.UTF8.GetBytes(jsonData);
            fileStream.Write(jsonbytes, 0, jsonbytes.Length);
            fileStream.Close();


            if (jsonData.Equals("{}"))
            {
                Debug.Log("json null");
                return;
            }
            Debug.Log(jsonData);
        }
        catch (FileNotFoundException e)
        {
            Debug.Log("The file was not found:" + e.Message);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("The directory was not found: " + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("The file could not be opened:" + e.Message);
        }
    }
    public T LoadFromFile<T>(string userId)
    {
        string path = Application.persistentDataPath + "/UserJson";

        try
        {
            FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, userId), FileMode.OpenOrCreate);

            byte[] jsonbytes = new byte[fileStream.Length];
            fileStream.Read(jsonbytes, 0, jsonbytes.Length);
            fileStream.Close();
            string jsonData = Encoding.UTF8.GetString(jsonbytes);

            if (jsonData.Equals("{}"))
            {
                Debug.Log("json null");
                return default(T);
            }
            Debug.Log(jsonData);

            T t = JsonToOject<T>(jsonData);
            return t;
        }
        // 처음 접속하는 아이디일 경우
        catch (FileNotFoundException e)
        {
            // FileStream fileStream = new FileStream(string.Format("{0}/{1}.json", path, userId), FileMode.Create);
            // string jsonData = "";
            // byte[] jsonbytes = Encoding.UTF8.GetBytes(jsonData);
            // fileStream.Write(jsonbytes, 0, jsonbytes.Length);
            // fileStream.Close();
            // T t = JsonToOject<T>(jsonData);
            Debug.Log("The file was not found and New File created:" + e.Message);
            return default(T);
        }
        catch (DirectoryNotFoundException e)
        {
            Debug.Log("The directory was not found: " + e.Message);
        }
        catch (IOException e)
        {
            Debug.Log("The file could not be opened:" + e.Message);
        }
        return default(T);
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnConnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnChatStateChange(ChatState state)
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        string time = "";
        string text = "";
        string timeFormat = "[yyyy-MM-dd HH:mm]";
        for (int i = 0; i < senders.Length; i++)
        {
            string msg = messages[i].ToString();
            if (msg.Length >= timeFormat.Length)
            {
                time = msg.Substring(0, timeFormat.Length);
                text = msg.Substring(timeFormat.Length);

            }
            else
            {
                Debug.Log("Can't Find timeFormat in received text");
                time = timeFormat;
                text = msg;
            }
            AppendMsg(channelName, new CustomMsg(senders[i], time, text));
        }

        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        // string frontMsg = "";
        // string rearMsg = "";
        // string fulltext = "";
        // string datetime = "[yyyy-MM-dd HH:mm]";
        // string msg = message.ToString();
        // if (msg.Length >= datetime.Length)
        // {
        //     frontMsg = msg.Substring(0, datetime.Length);
        //     rearMsg = msg.Substring(datetime.Length);
        //     fulltext = fulltext + frontMsg + " " + sender + " : " + rearMsg + "\n";
        // }
        // else
        //     fulltext = message + "\n";

        // udc.channels[channelName].Add(new CustomMsg(sender, frontMsg, rearMsg));
        // SaveAsFile(udc, udc.userId);

        throw new System.NotImplementedException();
    }
    string ObjectToJson(object obj)
    {
        return JsonConvert.SerializeObject(obj);
    }

    T JsonToOject<T>(string jsonData)
    {
        return JsonConvert.DeserializeObject<T>(jsonData);
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
    private void OnApplicationQuit()
    {
        SaveAsFile<UserDataContainer>(udc, udc.userId);
    }
}
