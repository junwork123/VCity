using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using TMPro;
using UnityEngine.SceneManagement;

public class NetworkManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static NetworkManager instance;
    [SerializeField] TMP_InputField UserIdInputField;
    [SerializeField] TMP_InputField UserPwInputField;
    [SerializeField] TMP_InputField roomNameInputField;
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    public static int maxPlayer = 4;
    public static int currentPlayer = 0;

    void Awake()
    {
        instance = this;//메서드로 사용
    }
    void Start()
    {
        // 앱 실행 화면의 창 크기를 고정한다.
        //Screen.SetResolution(1080, 1920, FullScreenMode.);

        // 데이터 송수신 빈도를 초당 30회로 설정한다.
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AddCallbackTarget(gameObject);

        // 어플리케이션의 버전을 설정한다.
        PhotonNetwork.GameVersion = "0.1";
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
        if (string.IsNullOrEmpty(UserIdInputField.text) && string.IsNullOrEmpty(UserPwInputField.text))
        {
            return;//ID, PW가 빈값이면 로그인 불가
        }

        // @TODO : 여기에 로그인 값을 인증하는 코드 삽입하기


        // 로그인 성공 시
        // 닉네임을 설정하고 자동 동기화 옵션을 켠 뒤 접속한다.
        int num = Random.Range(0, 1000);
        PhotonNetwork.NickName = UserIdInputField.text;

        // 마스터 클라이언트(방장)가 구성한 씬 환경을 방에 접속한 플레이어들과 자동 동기화한다.
        PhotonNetwork.AutomaticallySyncScene = true;
        // 마스터 서버에 접속한다.
        PhotonNetwork.ConnectUsingSettings();
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
            SceneManager.LoadScene("NetworkTest");

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
}
