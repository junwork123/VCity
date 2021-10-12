using System.Text;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Photon.Chat;
using ExitGames.Client.Photon;
using Firebase.Auth;
using Firebase.Firestore;

// 네트워크 매니저가 넘겨준 아이디를 받아서, 
// 사용자 정보(이름, 프로필, 대화내역 등)을 불러오기, 저장하기를 수행
public class DataManager : MonoBehaviour, IChatClientListener
{
    public static DataManager instance;
    FirebaseFirestore db;
    // 데이터 매니저는 싱글톤으로 존재
    public UserDataContainer udc { get; set; }
    void Awake()
    {
        if (instance == null) instance = new DataManager();
        else Destroy(gameObject);

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
        if (udc == null)
        {
            // users 콜렉션 지정
            CollectionReference usersRef = db.Collection("users");
            // 기본적인 유저 데이터 컨테이너 생성
            udc = new UserDataContainer(_email, _name);
            udc.channels = new Dictionary<string, string>();

            DocumentReference docRef = db.Collection("users").Document(_id);
            Dictionary<string, object> info = udc.ToDictionary();

            docRef.SetAsync(info).ContinueWith(task =>
            {
                Debug.Log("Added data to the LA document in the cities collection.");
            });
        }
        else
        {
            Debug.Log("[Database] " + "이미 UDC 인스턴스가 생성되어 있습니다. : " + udc.userEmail);
        }
    }
    // CRUD Operation @GET
    public void GetUsers(string _id, string _email, string _name)
    {
        db = FirebaseFirestore.GetInstance(Firebase.FirebaseApp.DefaultInstance);
        Debug.Log("[Database] " + "사용자 정보 불러오기 시작");
        CollectionReference cref = db.Collection("users");
        Debug.Log(cref);
        DocumentReference document = db.Collection("users").Document(_id);
        document.GetSnapshotAsync().ContinueWith(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Debug.Log(snapshot.ToDictionary());
                //Dictionary<string, object> user = snapshot.ToDictionary();
                string json = snapshot.ToString();
                udc = JsonUtility.FromJson<UserDataContainer>(json);
                Debug.Log("[Database] " + "등록된 사용자 정보 불러오기 완료. : " + udc.userEmail);
            }
            else
            {
                Debug.Log("[Database] " + "사용자 정보 불러오기 실패");
                AddUser(_id, _email, _name);
            }
        });
    }
    // CRUD Operation @PUT
    public void UpdateDialog(string _channelName)
    {
        if (udc != null)
        {
            // string json = JsonConvert.SerializeObject(udc);
            // string key = rootRef.Child("users").Child(udc.userId).Push().Key;

            // Dictionary<string, System.Object> childUpdates = new Dictionary<string, System.Object>();
            // childUpdates[udc.userId + "/" + _channelName + "/" + key] = udc.channels[_channelName];

            // rootRef.Child("users").Child(udc.userId).UpdateChildrenAsync(childUpdates);
        }
    }
    // CRUD Operation @DELETE
    public void DeleteUser()
    {
        // Firebase.Database.FirebaseDatabase dbInstance = Firebase.Database.FirebaseDatabase.DefaultInstance;
        // if (udc != null)
        // {
        //     rootRef.Child("users").Child(udc.userId).SetValueAsync(null);
        // }
    }
    public void AppendMsg(string _channelName, CustomMsg _msg)
    {
        //udc.channels[_channelName].Add(_msg);
        UpdateDialog(_channelName);
        //SaveAsFile(udc, udc.userId);
        Debug.Log("[Database] " + "append received Messages");
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
        SaveAsFile<UserDataContainer>(udc, udc.userEmail);
    }
}
