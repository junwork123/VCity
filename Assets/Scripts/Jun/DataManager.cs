using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    // 데이터 매니저는 싱글톤으로 존재
    void Awake() {
        if(instance == null) instance = new DataManager();
        else Destroy(gameObject);
    }
    void Start()
    {
        UserDataContainer udc = new UserDataContainer();
        // udc.userId = "Bob";
        // udc.dialogs = new List<Dialog>();

        // Dialog temp = new Dialog();
        // temp.channelName = "Resion";
        // temp.chatContents = "1234";

        // udc.dialogs.Add(temp);

        udc = DataLoadText<UserDataContainer>("Test.txt");

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DataSaveText<T>(T data, string _fileName)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);

            if (json.Equals("{}"))
            {
                Debug.Log("json null");
                return;
            }
            string path = Application.dataPath + "/" + _fileName;
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
    public T DataLoadText<T>(string _fileName)
    {
        try
        {
            string path = Application.dataPath + "/" + _fileName;
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
}
