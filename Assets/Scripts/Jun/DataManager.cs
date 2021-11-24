using System.Text;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
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
    public static DataManager Instance;
    public static string REGION_CHANNEL_ID = "3yxIU5G3AZx4IN0iRPSH";
    public Dictionary<string, string> videoCallInfo;
    FirebaseFirestore db;
    // 데이터 매니저는 싱글톤으로 존재
    public UserData userCache { get; set; }
    public Dictionary<string, List<CustomMsg>> chatCache;

    public List<Channel> roomInfoList;

    public static string TimeFormat = "[yyyy년 MM월 dd일 HH:mm:ss]";
    void Awake()
    {
        if (Instance == null) Instance = this;
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
    public async void AddUser(string _UID, UserData _userData)
    {

        db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        // users 콜렉션 지정
        CollectionReference usersRef = db.Collection("Users");
        // UID와 유저 정보를 연결한 뒤,
        // 기본적인 유저 데이터 컨테이너 생성
        _userData.UID = _UID;
        userCache = _userData;
        DocumentReference docRef = db.Collection("Users").Document(userCache.UID);
        Dictionary<string, object> userData = userCache.ToDictionary();
        chatCache = new Dictionary<string, List<CustomMsg>>();
        roomInfoList = new List<Channel>();

        await docRef.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            Debug.Log("[Database] " + "사용자 정보를 새로 생성했습니다. : " + userCache.UID);
        });

    }
    // CRUD Operation about 'users' collection @GET
    public IEnumerator GetUser(string _id)
    {
        db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        Debug.Log("[Database] " + "사용자 정보 불러오기 시작");
        DocumentReference document = db.Collection("Users").Document(_id);
        Task task = document.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log("[Database] " + "등록된 사용자 정보가 있습니다.");
                userCache = snapshot.ConvertTo<UserData>();
                chatCache = new Dictionary<string, List<CustomMsg>>();
                roomInfoList = new List<Channel>();
                StartCoroutine(GetVideoCallInfo());
                Photon.Chat.ChatManager chatManager = FindObjectOfType<Photon.Chat.ChatManager>();
                chatManager.Connect(DataManager.Instance.userCache.Nickname);

                Debug.Log("[Database] " + "등록된 사용자 정보 불러오기 완료. : " + userCache.UID);
            }
            else
            {
                // 유저 신규 생성 및 Region 채널을 구독 추가
                Debug.Log("[Database] " + "사용자 정보 불러오기 실패");
            }
        });
        yield return new WaitUntil(() => task.GetAwaiter().IsCompleted);
    }
    public UserData GetCurrentUser()
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
            DocumentReference userRef = db.Collection("Users").Document(userCache.UID);
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
        db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        // 채널 생성(id는 firebase에서 생성받음) 
        DocumentReference channelRef = db.Collection("Channels").Document();

        // 지역 공지 채널일 경우 Static 변수로 저장
        if (_channelName.Equals("Region") && REGION_CHANNEL_ID.Equals(""))
            REGION_CHANNEL_ID = channelRef.Id;

        // 멤버 리스트에 ID를 추가
        List<string> memberList = new List<string>();
        // memberList.Add(userCache.Id);

        // firestore에 저장하기 위한 Channel 객체 생성
        Channel channel = new Channel(channelRef.Id, _channelName, memberList);
        Dictionary<string, object> channelData = channel.ToDictionary();

        // firestore에 저장요청
        await channelRef.SetAsync(channelData).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                string nowtime = DateTime.Now.ToString((TimeFormat));
                channelRef.Collection("ChatContents").AddAsync(new CustomMsg("System", nowtime, "채팅 상담을 시작합니다", userCache.Character));
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
    public async void SubscribeChannel(string _channelId)
    {
        if (userCache != null && userCache.MyChannels != null)
        {
            // 이미 채널을 구독하고 있는 경우
            if (userCache.MyChannels.ContainsKey(_channelId))
            {
                Debug.Log("[Database] " + "이미 채널을 구독하고 있습니다 : " + userCache.MyChannels[_channelId]);
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
                    roomInfoList.Add(channel);

                    // 채널 정보에 현재 유저 추가
                    if (!channel.Members.Contains(userCache.UID))
                        channel.Members.Add(userCache.UID);

                    // 유저 정보에 현재 채널 추가
                    // 공지사항인 경우는 제목을 따로 설정하지 않는다
                    if (channel.Id != REGION_CHANNEL_ID)
                        userCache.MyChannels[channel.Id] = "민원상담 " + DateTime.Now.ToString(("(yyyy-MM-dd)")) + ")";

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
        if (userCache != null && userCache.MyChannels != null)
        {
            // 사용자가 해당 채널에 속해있는지 확인
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            if (!userCache.MyChannels.ContainsKey(_channelId))
            {
                Debug.Log("[Database] " + "해당 채널에 속해있지 않습니다");
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
                    Debug.Log("[Database] " + "채널 가져오기 성공 : " + snapshot.Id);
                    return snapshot;
                }
                else
                {
                    Debug.Log("[Database] " + "채널 가져오기 실패 : " + snapshot.Id);
                    return null;
                }
            });
        }
        return null;
    }
    public IEnumerator LoadAllMessages(string _channelId)
    {
        if (userCache != null && userCache.MyChannels != null)
        {
            // 사용자가 해당 채널에 속해있는지 확인
            db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
            if (!userCache.MyChannels.ContainsKey(_channelId))
            {
                Debug.Log("[Database] " + "해당 채널에 속해있지 않습니다");
            }
            // 사용자가 해당 채널에 속해있다면
            // 채널 정보를 캐시에 저장한다
            CollectionReference channelRef = db.Collection("Channels").Document(_channelId).Collection("ChatContents");
            Task task = channelRef.GetSnapshotAsync().ContinueWith(task =>
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
                        int Profile = 0;

                        item.TryGetValue("Sender", out Sender);
                        item.TryGetValue("Text", out Text);
                        item.TryGetValue("Time", out Time);
                        item.TryGetValue("Profile", out Profile);
                        Debug.Log(Time + " " + Sender + " : " + Text);
                        CustomMsg msg = new CustomMsg(Sender, Time, Text, Profile);

                        chatCache[_channelId].Add(msg);
                    }
                    Debug.Log("[Database] " + "채널 메시지 가져오기 성공");
                }
                else
                {
                    Debug.Log("[Database] " + "채널 메시지 가져오기 실패");

                }
            });
            yield return new WaitUntil(() => task.GetAwaiter().IsCompleted);
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
                Debug.Log("[Database] " + "채널 업데이트 성공");
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
                    LoadAllMessages(_channelId);
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
    public void SendMsg(string _channelName, string Sender, string text)
    {
        db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        // 채널 생성(id는 firebase에서 생성받음) 
        CollectionReference channelRef = db.Collection("Channels").Document(_channelName).Collection("ChatContents");

        // firestore에 저장요청
        string nowtime = DateTime.Now.ToString((TimeFormat));
        CustomMsg msg = new CustomMsg(Sender, nowtime, text, 4);
        channelRef.AddAsync(msg).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                Debug.Log("[Database] " + "메시지 전송 성공");
            }
            else
            {
                Debug.Log("[Database] " + "메시지 전송 실패");
            }
        });
    }
    public IEnumerator GetVideoCallInfo()
    {

        db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        DocumentReference tokenRef = db.Collection("VideoCall").Document("Token");
        Task task = tokenRef.GetSnapshotAsync().ContinueWith(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            videoCallInfo = new Dictionary<string, string>();
            string id, roomName, token;
            snapshot.TryGetValue<string>(nameof(id), out id);
            snapshot.TryGetValue<string>(nameof(roomName), out roomName);
            snapshot.TryGetValue<string>(nameof(token), out token);
            videoCallInfo[nameof(id)] = id;
            videoCallInfo[nameof(roomName)] = roomName;
            videoCallInfo[nameof(token)] = token;
            Debug.Log("[Video] id : " + videoCallInfo[nameof(id)]);
            Debug.Log("[Video] roomName : " + videoCallInfo[nameof(roomName)]);
            Debug.Log("[Video] Token : " + videoCallInfo[nameof(token)]);
        });

        yield return new WaitUntil(() => task.IsCompleted == true);
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

    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {

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
}
