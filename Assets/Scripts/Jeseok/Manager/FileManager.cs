using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using TMPro;

using UnityEngine.UI;

public class FileManager : Singleton<FileManager>
{
    AndroidJavaClass unityPlayer;
    AndroidJavaObject currentActivity;
    AndroidJavaObject context;

    string streamingPath, dataPath;
    public string fileName;
    public string loadFileName;

    public Image image;

    UnityWebRequest www;


    // Start is called before the first frame update
    void Start()
    {

#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidJNIHelper.debug = true;

        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
#endif



        streamingPath = Application.streamingAssetsPath;
        dataPath = Application.persistentDataPath;
        // file:// + dataPath + fileName

        // TODO 
        string sourcePath = string.Format("{0}/{1}", streamingPath, fileName);
        string filepath = string.Format("{0}/{1}", dataPath, fileName);
        StartCoroutine(DownloadFile(sourcePath, filepath));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FileOpen()
    {
        string filePath = string.Format("{0}/{1}", dataPath, fileName);
        www = new UnityWebRequest(filePath);
        switch (Application.platform)
        {
            case RuntimePlatform.WindowsEditor:
            case RuntimePlatform.WindowsPlayer:
                // Application.OpenURL(www.url);
                StartCoroutine(OpenFile("file://" + filePath));
                break;
            case RuntimePlatform.Android:
                Debug.Log(string.Format("url : {0}", www.url));
                StartCoroutine(OpenFile("file://" + filePath));
                break;
        }
    }

    IEnumerator DownloadFile(string sourcePath, string filePath)
    {
        www = UnityWebRequest.Get(sourcePath);
        www.downloadHandler = new DownloadHandlerFile(filePath);

        UnityWebRequestAsyncOperation operation = www.SendWebRequest();

        yield return new WaitUntil(() => operation.isDone);

        if (www.error != null)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler);
        }
    }

    IEnumerator OpenFile(string filePath)
    {
        yield return null;
        #region backup
        // logText.text = "";

        // AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        // AndroidJavaObject context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

        // AndroidJavaClass fileClass = new AndroidJavaClass("java.io.File");
        // AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        // // AndroidJavaClass uriClass = new AndroidJavaClass("androidx.core.content.FileProvider");

        // string packageName = context.Call<string>("getPackageName");
        // string authority = packageName + ".fileprovider";
        // AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", filePath);


        // AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", filePath);
        // // AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("getUriForFile", context, authority, fileObject);

        // AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        // AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        // intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
        // intentObject.Call<AndroidJavaObject>("setType", "application/pdf");
        // intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), fileName);
        // int FLAG_ACTIVITY_NEW_TASK = intentObject.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
        // intentObject.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
        // int FLAG_GRANT_READ_URI_PERMISSION = intentObject.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");
        // intentObject.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION);
        // intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);


        // // bool fileExist = uriObject.Call<bool>("exists");
        // // Debug.Log("[GeneralShare] File exist : " + fileExist);
        // // Debug.Log("[GeneralShare] File path : " + uriObject.Call<string>("getPath"));
        // // if (fileExist == true)
        // // {
        // //     intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
        // // }
        // Debug.Log(string.Format("[IntentObject] intent : {0}", intentObject));
        // AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, fileName);
        // currentActivity.Call("startActivity", jChooser);
        #endregion

        image.sprite = Resources.Load<Sprite>(loadFileName);
    }
}
