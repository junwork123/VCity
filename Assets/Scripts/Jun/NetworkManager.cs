using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;

// 1. Firebase에 접속, 아이디를 확인하고, 데이터 매니저(DB)가 값을 불러올 수 있게 함.
// 2. Photon 서버에 접속하고 서로 연결할 수 있도록 함.
public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static NetworkManager instance;
    [SerializeField] TMP_InputField UserIdInputField;
    [SerializeField] TMP_InputField UserPwInputField;
    [SerializeField] TMP_InputField UserNameInputField;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    public static int maxPlayer = 4;
    public static int currentPlayer = 0;

    FirebaseAuth auth;
    FirebaseUser user;
    Firebase.DependencyStatus dependencyStatus;

    void Awake()
    {
        instance = this;//메서드로 사용
        
    }
    void Start()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Can't not resolve Firebase Dependencies : " + dependencyStatus);
            }
        });
        dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
        // 앱 실행 화면의 창 크기를 고정한다.
        //Screen.SetResolution(1080, 1920, FullScreenMode.);

        // 데이터 송수신 빈도를 초당 30회로 설정한다.
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AddCallbackTarget(gameObject);

        // 어플리케이션의 버전을 설정한다.
        PhotonNetwork.GameVersion = "0.1";
    }

    void InitializeFirebase()
    {
        Debug.Log("Firebase Authentication initialize");
        auth = FirebaseAuth.DefaultInstance;
        // 재접속 시 함수 실행
        auth.StateChanged += AuthStateChanged;
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (signedIn == false && user != null)
            {
                Debug.Log("Signed out " + user.UserId);
            }
            user = auth.CurrentUser;
            if (signedIn == true)
            {
                Debug.Log("Signed in " + user.UserId);
            }
            else
            {
                Login();
            }
        }
    }

    // 자신 주변 랜덤한 위치에 플레이어 생성
    void MakePlayerInstance()
    {
        Vector3 randPos = new Vector3(-100, 5, -30);

        PhotonNetwork.Instantiate("test", transform.position + randPos, Quaternion.identity);
        currentPlayer++;
    }

    private void Update()
    {
        #region @test 테스트용 코드
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            //EventTime.ExecutionRaiseEvent(EventTime.Ready);
        }

        #endregion
    }
    void getRoomList()
    {
        //PhotonNetwork.GetCustomRoomList();
    }
    public void Login()
    {
        //ID, PW가 빈값이면 로그인 불가
        if (string.IsNullOrEmpty(UserIdInputField.text) && string.IsNullOrEmpty(UserPwInputField.text))
        {
            Debug.LogError("아이디와 PW를 모두 입력해주세요");
            return;
        }
        // 이미 로그인 되어있는 경우
        if (user != null)
        {
            Debug.Log("[Network] " + "이미 로그인되어있음 : " + "(" + user.UserId + ")");
            return;
        }

        // 제공되는 함수 : 이메일과 비밀번호로 로그인 시켜 줌
        auth.SignInWithEmailAndPasswordAsync(UserIdInputField.text, UserPwInputField.text).ContinueWith(
            task =>
            {
                if (task.IsCompleted)
                {
                    user = task.Result;
                    Debug.Log("[Network] " + "로그인 완료 : " + "(" + user.UserId + ")");
                    // 로그인 성공 시
                    // 닉네임을 설정하고 자동 동기화 옵션을 켠 뒤 접속한다.
                    DataManager.instance.GetUsers(user.UserId, UserNameInputField.text);
                    Photon.Chat.ChatManager chatManager = FindObjectOfType<Photon.Chat.ChatManager>();
                    chatManager.Connect(user.UserId);
                    PhotonNetwork.NickName = UserNameInputField.text;

                    // 마스터 클라이언트(방장)가 구성한 씬 환경을 방에 접속한 플레이어들과 자동 동기화한다.
                    PhotonNetwork.AutomaticallySyncScene = true;
                    // 마스터 서버에 접속한다.
                    PhotonNetwork.ConnectUsingSettings();
                }
                else if (task.IsFaulted)
                {
                    Debug.Log("[Network] " + "로그인 실패 : " + task.Exception);
                    return;
                }
                else if (task.IsCanceled)
                {
                    Debug.Log("[Network] " + "로그인이 취소됨");
                    return;
                }
            }
        );
    }
    public void Logout()
    {
        Debug.Log("[Network] " + "로그아웃 : " + "(" + user.UserId + ")");
        auth.SignOut();
    }
    public void Register()
    {
        // 제공되는 함수 : 이메일과 비밀번호로 회원가입 시켜 줌
        auth.CreateUserWithEmailAndPasswordAsync(UserIdInputField.text, UserPwInputField.text).ContinueWith(
            task =>
            {
                if (task.IsCompleted)
                {
                    Debug.Log(UserIdInputField.text + "로 회원가입\n");
                }
                else
                    Debug.Log("회원가입 실패\n");
            }
            );
    }
    public FirebaseUser GetCurrentUser()
    {
        if (user != null)
            return user;
        else
            return null;
    }
    public void CreateRoom()//방만들기
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))
        {
            return;//방 이름이 빈값이면 방 안만들어짐
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions { MaxPlayers = (byte)maxPlayer, IsVisible = true });//포톤 네트워크기능으로 roomNameInputField.text의 이름으로 방을 만든다.
        //MenuManager.Instance.OpenMenu("loading");//로딩창 열기
    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//포톤 네트워크의 JoinRoom기능 해당이름을 가진 방으로 접속한다. 
        //MenuManager.Instance.OpenMenu("loading");//로딩창 열기
    }

    #region 포톤 콜백함수 모음
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 접속 완료!");

        // 로비에 진입한다.
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 접속 완료!");
    }

    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount > maxPlayer)
        {
            Debug.Log("플레이어가 가득 차 참여할 수 없습니다.");
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        }
        else
        {
            SceneManager.LoadScene("Net2");

            Debug.Log("룸에 입장!");
        }
    }
    public override void OnCreateRoomFailed(short returnCode, string message)//방 만들기 실패시 작동
    {
        // errorText.text = "Room Creation Failed: " + message;
        // MenuManager.Instance.OpenMenu("error");//에러 메뉴 열기
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)//포톤의 룸 리스트 기능
    {
        foreach (Transform trans in roomListContent)//존재하는 모든 roomListContent
        {
            Destroy(trans.gameObject);//룸리스트 업데이트가 될때마다 싹지우기
        }
        for (int i = 0; i < roomList.Count; i++)//방갯수만큼 반복
        {
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().Setup(roomList[i]);
            //instantiate로 prefab을 roomListContent위치에 만들어주고 그 프리펩은 i번째 룸리스트가 된다. 
        }
    }
    #endregion

    public void OnEvent(EventData eventData)
    {

    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
    public void OnApplicationQuit()
    {
        Logout();
    }
}
