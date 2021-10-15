using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Photon.Chat;
using ExitGames.Client.Photon;
using Firebase.Auth;
using Firebase.Firestore;
using Firebase.Extensions;

// 네트워크 매니저가 넘겨준 아이디를 받아서, 
// 사용자 정보(이름, 프로필, 대화내역 등)을 불러오기, 저장하기를 수행
public class DataManager : MonoBehaviour, IChatClientListener
{
    public static DataManager instance;
    public const string REGION_CHANNEL_ID = "hf5yBbH0JEyNBJpmLI6p";
    FirebaseFirestore db;
    // 데이터 매니저는 싱글톤으로 존재
    public UserDataContainer udc { get; set; }
    void Awake()
    {
        if (instance == null) instance = new DataManager();
        else Destroy(gameObject);
        DontDestroyOnLoad(this);
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // CRUD Operation @POST
    public void AddUser(string _id, string _email, string _name)
    {
        // 유저를 먼저 불러오도록 요청한 뒤
        // 없다면 유저를 새로 생성한다
        udc = GetUser(_id);
        if (udc == null)
        {
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            // users 콜렉션 지정
            CollectionReference usersRef = db.Collection("users");
            // 기본적인 유저 데이터 컨테이너 생성
            udc = new UserDataContainer(_id, _email, _name);
            udc.channels = new Dictionary<string, object>();
            DocumentReference docRef = db.Collection("users").Document(_id);
            Dictionary<string, object> userData = udc.ToDictionary();

            docRef.SetAsync(userData).ContinueWithOnMainThread(task =>
            {
                Debug.Log("[Database] " + "사용자 정보를 새로 생성했습니다. : " + udc.Email);
            });
        }
        else
        {
            Debug.Log("[Database] " + "이미 UDC 인스턴스가 생성되어 있습니다. : " + udc.Email);
        }
    }
    // CRUD Operation about 'users' collection @GET
    public UserDataContainer GetUser(string _id)
    {
        db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        Debug.Log("[Database] " + "사용자 정보 불러오기 시작");
        DocumentReference document = db.Collection("users").Document(_id);
        document.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log("[Database] " + "등록된 사용자 정보가 있습니다.");
                udc = snapshot.ConvertTo<UserDataContainer>();
                Debug.Log("[Database] " + "등록된 사용자 정보 불러오기 완료. : " + udc.Email);
            }
            else
            {
                // 유저 신규 생성 및 Region 채널을 구독 추가
                Debug.Log("[Database] " + "사용자 정보 불러오기 실패");
            }
            return udc;
        });
        return null;
    }
    // CRUD Operation @PUT
    public void UpdateUser()
    {
        if (udc != null)
        {
            Debug.Log("[Database] " + "사용자 정보 업데이트 시작");
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            Debug.Log("[Database] " + "사용자 정보 업데이트 요청 : " + udc.Id);
            DocumentReference userRef = db.Collection("users").Document(udc.Id);
            Dictionary<string, object> userData = udc.ToDictionary();
            userRef.SetAsync(userData).ContinueWithOnMainThread(task =>
            {
                Debug.Log("[Database] " + "사용자 정보 업데이트 성공");
            });
        }
        else
        {
            Debug.Log("[Database] " + "UDC가 비어있습니다");
        }

    }
    // CRUD Operation @PUT
    public void CreateChannel(string _targetUserId)
    {
        if (udc != null && udc.channels != null)
        {
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            // 채널 id 부여 및 생성 
            DocumentReference channelRef = db.Collection("channels").Document();
            Channel channel = new Channel(channelRef.Id, udc.Id, _targetUserId);
            Dictionary<string, object> channelData = channel.ToDictionary();

            // 사용자 정보에 채널 추가
            DocumentReference userRef = db.Collection("users").Document(udc.Id);
            udc.channels[channel.Id] = channel;

            channelRef.SetAsync(channelData).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                    userRef.UpdateAsync("channels", udc.channels)
                        .ContinueWithOnMainThread(task =>
                        {
                            if (task.IsCompleted)
                                Debug.Log("[Database] " + "채널 추가 성공");
                            else if (task.IsFaulted)
                                Debug.Log("[Database] " + "채널 추가 실패");
                        });

            });

            // Batch를 통해 한번에 처리
            // Channel 콜렉션에 문서 PUT + 사용자 정보에 UPDATE
            // WriteBatch batch = db.StartBatch();
            // batch.Set(channelRef, channelData);
            // batch.Update(userRef, userData);
            // batch.CommitAsync().ContinueWithOnMainThread(task =>
            // {
            //     if (task.IsCompleted)
            //         Debug.Log("[Database] " + "채널 추가 성공");
            //     else if (task.IsFaulted)
            //         Debug.Log("[Database] " + "채널 추가 실패");
            // });
        }
    }
    public void SubcribeChannel(string _channelId)
    {
        if (udc.channels.ContainsKey(_channelId))
        {
            Debug.Log("[Database] " + "이미 채널을 구독하고 있습니다 : " + udc.channels[_channelId]);
            return;
        }
        else
        {
            Debug.Log("[Database] " + "채널 구독 작업 시작 : " + _channelId);
            System.Threading.Tasks.Task<Channel> task = GetChannel(_channelId);
            if (task.IsCompleted)
            {
                Channel channel = task.Result;
                udc.channels[_channelId] = channel;
                channel.Members.Add(udc.Id);
                UpdateUser();
                UpdateChannel(channel);
                Debug.Log("[Database] " + "채널 구독 완료 : " + channel.Id);
            }else{
                Debug.Log("[Database] " + "채널을 불러오지 못했습니다.");
            }

        }

    }
    // CRUD Operation @GET
    public async System.Threading.Tasks.Task<Channel> GetChannel(string _channelId)
    {
        if (udc != null && udc.channels != null)
        {
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);

            if (!udc.channels.ContainsKey(_channelId))
            {
                Debug.Log("[Database] " + "해당 채널에 속해있지 않습니다 : " + udc.channels.Keys);
                return null;
            }
            DocumentReference channelRef = db.Collection("channels").Document(_channelId);
            await channelRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Channel channel = snapshot.ConvertTo<Channel>();
                    Debug.Log("[Database] " + "채널 가져오기 성공");
                    return channel;
                }
                else
                {
                    Debug.Log("[Database] " + "채널 가져오기 실패");
                    return null;
                }
            });
        }
        return null;
    }
    public Dictionary<string, object> GetChannelList()
    {
        return udc.channels;
    }
    public void UpdateChannel(Channel _channel)
    {
        if (udc != null)
        {
            Debug.Log("[Database] " + "채널 업데이트 시작");
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            Debug.Log("[Database] " + "채널 업데이트 요청 : " + _channel.Id);
            DocumentReference channelRef = db.Collection("rooms").Document(_channel.Id);
            Dictionary<string, object> channelData = _channel.ToDictionary();
            Debug.Log("[Database] " + "채널 정보 업데이트 요청");
            channelRef.UpdateAsync(channelData).ContinueWithOnMainThread(task =>
            {
                Debug.Log("[Database] " + "사용자 정보 업데이트 성공");
            });
        }
        else
        {
            Debug.Log("[Database] " + "UDC가 비어있습니다");
        }

    }
    public void AppendMsg(string _channelId, CustomMsg _msg)
    {
        //UpdateDialog(_channelName);
        //SaveAsFile(udc, udc.userId);
        Debug.Log("[Database] " + "append received Messages");
        if (udc != null)
        {
            Debug.Log("[Database] " + "메시지 추가 시작");
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);

            System.Threading.Tasks.Task<Channel> task = GetChannel(_channelId);
            if (task.IsCompleted)
            {
                Channel channel = task.Result;
                channel.ChatContents.Add(_msg);
                UpdateChannel(channel);
                Debug.Log("[Database] " + "메시지 추가 성공 : " + _msg.text);
            }else{
                Debug.Log("[Database] " + "해당 채널에 속해있지 않아 전송 실패");
            }
        }
        else
        {
            Debug.Log("[Database] " + "메시지 추가 실패");
        }
        return;
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
            //AppendMsg(channelName, new CustomMsg(senders[i], time, text));
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
        SaveAsFile<UserDataContainer>(udc, udc.Email);
    }

    #region useless
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
    #endregion
}
