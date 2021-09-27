using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Photon.Chat;
using ExitGames.Client.Photon;

public class DataManager : MonoBehaviour, IChatClientListener
{
    public static DataManager instance;
    // 데이터 매니저는 싱글톤으로 존재
    UserDataContainer udc { get; set; }
    void Awake()
    {
        if (instance == null) instance = new DataManager();
        else Destroy(gameObject);
        udc = new UserDataContainer();
    }
    void Start()
    {
        #region @Test 테스트용 코드(추후 삭제)
        udc.userId = "1234";
        udc.userName = "Bob";
        udc.dialogs = new List<Dialog>();

        Dialog temp = new Dialog();
        temp.channelName = "Region";
        temp.chatContents = new List<CustomMsg>();
        temp.chatContents.Add(new CustomMsg("Joe", "[yyyy-MM-dd HH:mm]", "Hello world!"));

        udc.dialogs.Add(temp);
        DataSaveText(udc, "1234");
        #endregion
    }

    // Update is called once per frame
    void Update()
    {

    }
    public UserDataContainer LoadDataWithId(string _id)
    {
        // @TODO : 아이디 없을 때 새로 생성하는 예외처리 필요함
        udc = DataLoadText<UserDataContainer>(_id);
        return udc;
    }
    public void UpdateDialog(string _channelName, string _chatContents)
    {
        // foreach (var dialog in udc.dialogs)
        // {
        //     if (dialog.channelName == _channelName)
        //     {
        //         dialog.chatContents = _chatContents + '\n';
        //     }
        //     Debug.Log("append Dialog and Sava Data to File as JSON");
        //     DataSaveText(udc, udc.userId);
        //     break;
        // }
    }
    public void appendDialog(string _channelName, CustomMsg _msg)
    {
        foreach (var dialog in udc.dialogs)
        {
            if (dialog.channelName == _channelName)
            {
                dialog.chatContents.Add(_msg);
            }
            Debug.Log("append Dialog and Sava Data to File as JSON");
            DataSaveText(udc, udc.userId);
            return;
        }
        // TODO : 같은 이름의 채널이 없을 때 
        Debug.Log("New Channel is Open");
    }
    public void DataSaveText<T>(T data, string userId)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);

            if (json.Equals("{}"))
            {
                Debug.Log("json null");
                return;
            }
            string path = Application.dataPath + "/" + userId + ".txt";
            File.WriteAllText(path, json);

            Debug.Log(json);
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
    public T DataLoadText<T>(string userId)
    {
        try
        {
            string path = Application.dataPath + "/" + userId + ".txt";
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                Debug.Log(json);
                T t = JsonUtility.FromJson<T>(json);
                return t;
            }
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
        return default;
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
            appendDialog(channelName, new CustomMsg(senders[i], time, text));
        }

        throw new System.NotImplementedException();
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        foreach (var dialog in udc.dialogs)
        {
            if (dialog.channelName == channelName)
            {
                string frontMsg = "";
                string rearMsg = "";
                string fulltext = "";
                string datetime = "[yyyy-MM-dd HH:mm]";
                string msg = message.ToString();
                if (msg.Length >= datetime.Length)
                {
                    frontMsg = msg.Substring(0, datetime.Length);
                    rearMsg = msg.Substring(datetime.Length);
                    fulltext = fulltext + frontMsg + " " + sender + " : " + rearMsg + "\n";
                }
                else
                    fulltext = message + "\n";
                //dialog.chatContents += fulltext;
            }
            DataSaveText(udc, udc.userId);
            break;
        }
        throw new System.NotImplementedException();
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
}
