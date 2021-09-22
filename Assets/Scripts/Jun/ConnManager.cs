using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class ConnManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    public static ConnManager instance;

    public static int maxPlayer = 4;
    public static int currentPlayer = 0;
    void Start()
    {
        // 싱글톤 생성
        if (instance == null) instance = new ConnManager();
        else Destroy(gameObject);

        // 앱 실행 화면의 창 크기를 고정한다.
        Screen.SetResolution(1920, 1080, FullScreenMode.Windowed);

        // 데이터 송수신 빈도를 초당 30회로 설정한다.
        PhotonNetwork.SendRate = 30;
        PhotonNetwork.SerializationRate = 30;
        PhotonNetwork.AddCallbackTarget(gameObject);

        // 어플리케이션의 버전을 설정한다.
        PhotonNetwork.GameVersion = "0.1";

        // 사용자의 이름을 설정한다.(Player0001 등)
        int num = Random.Range(0, 1000);
        PhotonNetwork.NickName = "Player" + num.ToString();

        // 마스터 클라이언트(방장)가 구성한 씬 환경을 방에 접속한 플레이어들과 자동 동기화한다.
        PhotonNetwork.AutomaticallySyncScene = true;
        // 마스터 서버에 접속한다.
        PhotonNetwork.ConnectUsingSettings();

    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버에 접속 완료!");

        // 로비에 진입한다.
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 접속 완료!");

        // 방에 대한 설정을 한다.
        RoomOptions ro = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 4
        };
        // 방에 들어가거나 방을 생성한다.
        PhotonNetwork.JoinOrCreateRoom("NetTest", ro, TypedLobby.Default);
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
            //MakePlayerInstance();
            Debug.Log("룸에 입장!");
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
    public void OnEvent(EventData eventData)
    {

    }
    public override void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }
}
