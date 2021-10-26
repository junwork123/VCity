using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Photon.Chat;
using ExitGames.Client.Photon;
using Firebase.Firestore;
using Firebase.Extensions;
using System;
using System.Threading.Tasks;



// 네트워크 매니저가 넘겨준 아이디를 받아서, 
// 사용자 정보(이름, 프로필, 대화내역 등)을 불러오기, 저장하기를 수행
public class DataManager : MonoBehaviour, IChatClientListener
{
    public static DataManager instance;
    public static string REGION_CHANNEL_ID = "s1r8QUWh1cOxFm0RUGmV";
    FirebaseFirestore db;
    // 데이터 매니저는 싱글톤으로 존재
    public UserDataContainer userCache { get; set; }
    public Dictionary<string, List<CustomMsg>> chatCache;
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
    public async void AddUser(string _id, string _email, string _name)
    {

        db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        // users 콜렉션 지정
        CollectionReference usersRef = db.Collection("Users");
        // 기본적인 유저 데이터 컨테이너 생성
        userCache = new UserDataContainer(_id, _email, _name);
        DocumentReference docRef = db.Collection("Users").Document(_id);
        Dictionary<string, object> userData = userCache.ToDictionary();
        chatCache = new Dictionary<string, List<CustomMsg>>();

        await docRef.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            Debug.Log("[Database] " + "사용자 정보를 새로 생성했습니다. : " + userCache.Email);
        });

    }
    // CRUD Operation about 'users' collection @GET
    public Task<UserDataContainer> GetUser(string _id)
    {
        db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        Debug.Log("[Database] " + "사용자 정보 불러오기 시작");
        DocumentReference document = db.Collection("Users").Document(_id);
        document.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log("[Database] " + "등록된 사용자 정보가 있습니다.");
                userCache = snapshot.ConvertTo<UserDataContainer>();
                chatCache = new Dictionary<string, List<CustomMsg>>();
                Debug.Log("[Database] " + "등록된 사용자 정보 불러오기 완료. : " + userCache.Email);
            }
            else
            {
                // 유저 신규 생성 및 Region 채널을 구독 추가
                Debug.Log("[Database] " + "사용자 정보 불러오기 실패");
            }
            return userCache;
        });
        return null;
    }
    public UserDataContainer GetCurrentUser()
    {
        return userCache;
    }
    // CRUD Operation @PUT
    public async void UpdateUser()
    {
        if (userCache != null)
        {
            Debug.Log("[Database] " + "사용자 정보 업데이트 요청 시작");
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            DocumentReference userRef = db.Collection("Users").Document(userCache.Id);
            Dictionary<string, object> userData = userCache.ToDictionary();
            await userRef.SetAsync(userData).ContinueWithOnMainThread(task =>
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
    public async void CreateChannel(string _channelName)
    {
        if (userCache != null && userCache.Channels != null)
        {
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            // 채널 생성(id는 firebase에서 생성받음) 
            DocumentReference channelRef = db.Collection("Channels").Document();

            // 지역 공지 채널일 경우 Static 변수로 저장
            if (_channelName.Equals("Region") && REGION_CHANNEL_ID == "")
                REGION_CHANNEL_ID = channelRef.Id;

            // 멤버 리스트에 현재 ID를 추가
            List<string> memberList = new List<string>();
            memberList.Add(userCache.Id);

            // firestore에 저장하기 위한 Channel 객체 생성
            Channel channel = new Channel(channelRef.Id, _channelName, memberList);
            Dictionary<string, object> channelData = channel.ToDictionary();

            // firestore에 저장요청
            await channelRef.SetAsync(channelData).ContinueWithOnMainThread(task =>
            {
                if (task.IsCompleted)
                {
                    string nowtime = DateTime.Now.ToString(("[yyyy-MM-dd HH:mm]"));
                    channelRef.Collection("ChatContents").AddAsync(new CustomMsg("System", nowtime, "Hello, World!"));
                    Debug.Log("[Database] " + "채널 추가 성공");
                }
                else
                {
                    Debug.Log("[Database] " + "채널 추가 실패");
                }


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
    public async void SubscribeChannel(string _channelId)
    {
        if (userCache != null && userCache.Channels != null)
        {
            // 이미 채널을 구독하고 있는 경우
            if (userCache.Channels.Contains(_channelId))
            {
                Debug.Log("[Database] " + "이미 채널을 구독하고 있습니다 : " + userCache.Channels);
                return;
            }

            // 채널 가져오기 요청
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            Debug.Log("[Database] " + "채널 가져오기 요청 시작");
            DocumentReference channelRef = db.Collection("Channels").Document(_channelId);
            await channelRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {

                DocumentSnapshot snapshot = task.Result;
                // 채널이 있는 경우
                if (snapshot.Exists)
                {
                    Debug.Log("[Database] " + "채널 가져오기 성공");
                    Channel channel = snapshot.ConvertTo<Channel>();
                    Debug.Log("[Database] " + "채널 변환 완료 : " + channel.Id);

                    // 유저 정보에 현재 채널 추가
                    // 채널 정보에 현재 유저 추가
                    userCache.Channels.Add(channel.Id);
                    if (!channel.Members.Contains(userCache.Id))
                        channel.Members.Add(userCache.Id);
                    userCache.MyChannels[channel.Id] = "방 이름";

                    // 유저 정보 갱신, 전체 채널 정보 갱신
                    UpdateUser();
                    UpdateChannel(channel);
                    Debug.Log("[Database] " + "채널 구독 완료 : " + channel.Id);
                    return snapshot;
                }
                else
                {
                    Debug.Log("[Database] " + "채널 가져오기 실패");
                    return null;
                }
            });
        }
    }
    // CRUD Operation @GET

    public async Task<DocumentSnapshot> GetChannel(string _channelId)
    {
        if (userCache != null && userCache.Channels != null)
        {
            // 사용자가 해당 채널에 속해있는지 확인
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            if (!userCache.Channels.Contains(_channelId))
            {
                Debug.Log("[Database] " + "해당 채널에 속해있지 않습니다 : " + userCache.Channels);
                return null;
            }
            // 사용자가 채널에 속해있다면
            // 채널 정보를 가져온다
            DocumentReference channelRef = db.Collection("Channels").Document(_channelId);
            await channelRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    Debug.Log("[Database] " + "채널 가져오기 성공");
                    return snapshot;
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
    public async void LoadMessages(string _channelId)
    {
        if (userCache != null && userCache.Channels != null)
        {
            // 사용자가 해당 채널에 속해있는지 확인
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            if (!userCache.Channels.Contains(_channelId))
            {
                Debug.Log("[Database] " + "해당 채널에 속해있지 않습니다 : " + userCache.Channels);
            }
            // 사용자가 해당 채널에 속해있다면
            // 채널 정보를 캐시에 저장한다
            CollectionReference channelRef = db.Collection("Channels").Document(_channelId).Collection("ChatContents");
            await channelRef.GetSnapshotAsync().ContinueWith(task =>
            {

                if (task.IsCompleted)
                {
                    QuerySnapshot snapshots = task.Result;
                    chatCache[_channelId] = new List<CustomMsg>();
                    Debug.Log("[Database] " + "채널 메시지 가져오기 시작");
                    foreach (DocumentSnapshot item in snapshots.Documents)
                    {
                        string Sender = "";
                        string Text = "";
                        string Time = "";


                        item.TryGetValue("Sender", out Sender);
                        item.TryGetValue("Text", out Text);
                        item.TryGetValue("Time", out Time);
                        Debug.Log(Time + " " + Sender + " : " + Text);
                        CustomMsg msg = new CustomMsg(Sender, Time, Text);

                        chatCache[_channelId].Add(msg);
                    }
                    Debug.Log("[Database] " + "채널 메시지 가져오기 성공");
                }
                else
                {
                    Debug.Log("[Database] " + "채널 메시지 가져오기 실패");

                }
            });
        }
    }
    public async void UpdateChannel(Channel _channel)
    {
        if (userCache != null)
        {
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            DocumentReference channelRef = db.Collection("Channels").Document(_channel.Id);
            Dictionary<string, object> channelData = _channel.ToDictionary();

            Debug.Log("[Database] " + "채널 업데이트 요청 시작");
            await channelRef.UpdateAsync(channelData).ContinueWithOnMainThread(task =>
            {
                Debug.Log("[Database] " + "사용자 정보 업데이트 성공");
            });
        }
        else
        {
            Debug.Log("[Database] " + "UDC가 비어있습니다");
        }

    }
    public async void AppendMsg(string _channelId, CustomMsg _msg)
    {
        if (userCache != null)
        {
            
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            CollectionReference msgRef = db.Collection("Channels").Document(_channelId).Collection("ChatContents");

            Debug.Log("[Database] " + "메시지 추가 시작" + msgRef.Id);
            await msgRef.AddAsync(_msg.ToDictionary()).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    LoadMessages(_channelId);
                    Debug.Log("[Database] " + "메시지 추가 성공 : " + _msg.Text);
                }
                else
                {
                    Debug.Log("[Database] " + "메시지 추가 실패");
                }
            });
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
        //SaveAsFile<UserDataContainer>(udc, udc.Email);
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
