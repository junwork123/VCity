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
using Firebase.Extensions;
using System.Threading.Tasks;

// 1. Firebase에 접속, 아이디를 확인하고, 데이터 매니저(DB)가 값을 불러올 수 있게 함.
// 2. Photon 서버에 접속하고 서로 연결할 수 있도록 함.
public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static NetworkManager Instance;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    public static int maxPlayer = 4;
    public static int currentPlayer = 0;

    FirebaseAuth auth;
    FirebaseUser user;
    Firebase.DependencyStatus dependencyStatus;

    public GameObject loadingPanel;
    private bool isQuitWait;

    void Awake()
    {
        Instance = this;
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
            bool signedIn = (user != auth.CurrentUser && auth.CurrentUser != null);
            if (signedIn == false && user != null)
            {
                Debug.Log("[Network] " + "Signed out " + user.UserId);
            }
            // @TODO : 자동 로그인 시 여기 수정
            // user = auth.CurrentUser;
            if (signedIn == true && user != null)
            {
                Debug.Log("[Network] " + "Signed in " + user.UserId);
            }
            else
            {
                //Login();
            }
        }
    }

    private void Update()
    {
        // 취소 버튼 두번 눌렀을 경우 게임 종료
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isQuitWait == false)
            {
                AndroidToastManager.instance.ShowToast("한번 더 누르시면 종료됩니다.");
                StartCoroutine(CheckQuitApplication());
                isQuitWait = true;
            }
            else
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                    Application.Quit();
#endif
            }

        }
    }
    void getRoomList()
    {
        //PhotonNetwork.GetCustomRoomList();
    }
    public async void Login(string _userId, string _userPw)
    {
        //ID, PW가 빈값이면 로그인 불가
        if (string.IsNullOrEmpty(_userId) && string.IsNullOrEmpty(_userPw))
        {
            Debug.LogError("[Network]" + "아이디와 PW를 모두 입력해주세요");
            return;
        }
        // 이미 로그인 되어있는 경우
        if (user != null)
        {
            Debug.Log("[Network] " + "이미 로그인되어있음 : " + "(" + user.UserId + ")");
            return;
        }
        loadingPanel.SetActive(true);
        // 제공되는 함수 : 이메일과 비밀번호로 로그인 시켜 줌
        await auth.SignInWithEmailAndPasswordAsync(_userId, _userPw).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCompleted)
                {
                    user = task.Result;

                    // 로그인 성공 시
                    // 닉네임을 설정하고 자동 동기화 옵션을 켠 뒤 접속한다.
                    
                    StartCoroutine(DataManager.Instance.GetUser(user.UserId));
                    // Debug.Log(DataManager.instance.userCache.Nickname);
                    // #region @Test용
                    //DataManager.Instance.SendMsg(DataManager.REGION_CHANNEL_ID, "알리미", "");
                    //DataManager.Instance.SendMsg(DataManager.REGION_CHANNEL_ID, "알리미", "");
                    // //DataManager.instance.CreateChannel("Region");
                    // DataManager.instance.SubscribeChannel(DataManager.REGION_CHANNEL_ID);
                    // #endregion
                    // Debug.Log(DataManager.instance.userCache.Nickname);
                    // PhotonNetwork.NickName = DataManager.instance.userCache.Nickname;

                    // 마스터 클라이언트(방장)가 구성한 씬 환경을 방에 접속한 플레이어들과 자동 동기화한다.
                    PhotonNetwork.AutomaticallySyncScene = true;
                    // 마스터 서버에 접속한다.
                    PhotonNetwork.ConnectUsingSettings();

                    Debug.Log("[Network] " + "로그인 완료 : " + "(" + user.UserId + ")");
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

        //CreateRoom("Room 1");
        //loadingPanel.SetActive(false);
    }
    public bool CheckIdExist() // 중복 가입 확인
    {
        //auth.SignInWithEmailAndPasswordAsync
        return true;
    }
    public void Logout()
    {
        auth.SignOut();
        Debug.Log("[Network] " + "로그아웃 : " + "(" + user.UserId + ")");
    }
    public IEnumerator Register(string _userId, string _userPw, UserData _userData)
    {
        // 제공되는 함수 : 이메일과 비밀번호로 회원가입 시켜 줌
        Task task =
        auth.CreateUserWithEmailAndPasswordAsync(_userId, _userPw).ContinueWith(
            task =>
            {
                if (task.IsCompleted)
                {
                    user = task.Result;
                    Firebase.Auth.UserProfile profile = new Firebase.Auth.UserProfile
                    {
                        DisplayName = _userData.Name,
                        //PhotoUrl = _userData.Profile,
                    };
                    user.UpdateUserProfileAsync(profile).ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.Log("[Network] " + "UpdateUserProfileAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Debug.Log("[Network] " + "UpdateUserProfileAsync encountered an error: " + task.Exception);
                            return;
                        }

                        Debug.Log("[Network] " + "User profile updated successfully.");
                    });
                    user.UpdateEmailAsync(_userId).ContinueWith(task =>
                    {
                        if (task.IsCanceled)
                        {
                            Debug.Log("[Network] " + "UpdateUserProfileAsync was canceled.");
                            return;
                        }
                        if (task.IsFaulted)
                        {
                            Debug.Log("[Network] " + "UpdateUserProfileAsync encountered an error: " + task.Exception);
                            return;
                        }

                        Debug.Log("[Network] " + "User profile updated successfully.");
                    });
                    Debug.Log("[Network] " + _userId + "로 회원가입\n");

                }
                else
                    Debug.Log("[Network] " + "회원가입 실패\n");
            });
        yield return new WaitUntil(() => task.GetAwaiter().IsCompleted);
        DataManager.Instance.AddUser(user.UserId, _userData);
    }
    public FirebaseUser GetCurrentUser()
    {
        if (user != null)
            return user;
        else
            return null;
    }
    public void CreateRoom(string _roomName = "")//방만들기
    {
        if (_roomName.Equals(""))
        {
            PhotonNetwork.CreateRoom("test", new RoomOptions { MaxPlayers = (byte)maxPlayer, IsVisible = true });//포톤 네트워크기능으로 roomNameInputField.text의 이름으로 방을 만든다.
            //MenuManager.Instance.OpenMenu("loading");//로딩창 열기
        }
        else
        {
            PhotonNetwork.CreateRoom(_roomName, new RoomOptions { MaxPlayers = (byte)maxPlayer, IsVisible = true });
        }

    }

    public void JoinRoom(RoomInfo info)
    {
        PhotonNetwork.JoinRoom(info.Name);//포톤 네트워크의 JoinRoom기능 해당이름을 가진 방으로 접속한다. 
        //MenuManager.Instance.OpenMenu("loading");//로딩창 열기
    }

    #region 포톤 콜백함수 모음
    public override void OnConnectedToMaster()
    {
        Debug.Log("[Network] " + "마스터 서버에 접속 완료!");

        // 로비에 진입한다.
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("[Network] " + "로비에 접속 완료!");
        PhotonNetwork.JoinOrCreateRoom("Room 1", new RoomOptions { MaxPlayers = (byte)maxPlayer, IsVisible = true }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount > maxPlayer)
        {
            Debug.Log("[Network] " + "플레이어가 가득 차 참여할 수 없습니다.");
            Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        }
        else
        {
            SceneManager.LoadScene(1);
            loadingPanel.SetActive(false);

            Debug.Log("[Network] " + "룸에 입장!");
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
        //Logout();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
    IEnumerator CheckQuitApplication()
    {
        yield return new WaitForSeconds(2.0f);
        isQuitWait = false;
    }
}
