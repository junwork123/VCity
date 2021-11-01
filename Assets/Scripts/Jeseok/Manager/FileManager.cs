using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using TMPro;

public class FileManager : Singleton<FileManager>
{
    string streamingPath, dataPath;
    public string fileName;

    UnityWebRequest www;

    public TextMeshProUGUI filePathText;


    // Start is called before the first frame update
    void Start()
    {
        streamingPath = Application.streamingAssetsPath;
        dataPath = Application.persistentDataPath;
        filePathText.text = streamingPath;

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
                Application.OpenURL(www.url);
                break;
            case RuntimePlatform.Android:
                // Application.OpenURL("file://" + filePath);

                StartCoroutine(OpenFile("content://" + filePath));
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
            print(www.error);
        }
        else
        {
            print(www.downloadHandler);
        }
    }

    IEnumerator OpenFile(string filePath)
    {
        yield return null;
        #region sample
        //         AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        //         AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

        //         AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        //         AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");


        //         AndroidJavaClass uriClass = new AndroidJavaClass("android.support.v4.content.FileProvider");
        //         AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", filePath);// Set Image Path Here
        //         AndroidJavaObject stringObject = new AndroidJavaObject("java.lang.String", "com.myproject.project.share.fileprovider");// Set Image Path Here
        //         AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("getUriForFile", currentActivity, stringObject, fileObject);

        //         bool fileExist = fileObject.Call<bool>("exists");
        //         if (fileExist)
        //             intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

        //         currentActivity.Call("startActivity", intentObject);
        #endregion


        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject unityContext = currentActivity.Call<AndroidJavaObject>("getApplicationContext");

        AndroidJavaClass intentObj = new AndroidJavaClass("android.content.Intent");
        string ACTION_VIEW = intentObj.GetStatic<string>("ACTION_VIEW");
        int FLAG_ACTIVITY_NEW_TASK = intentObj.GetStatic<int>("FLAG_ACTIVITY_NEW_TASK");
        int FLAG_GRANT_READ_URI_PERMISSION = intentObj.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION");

        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", ACTION_VIEW);

        AndroidJavaObject fileObj = new AndroidJavaObject("java.io.File", filePath);
        AndroidJavaClass fileProvider = new AndroidJavaClass("androidx.core.content.FileProvider");

        string packageName = unityContext.Call<string>("getPackageName");
        string authority = packageName + ".fileprovider";

        AndroidJavaObject uri = fileProvider.CallStatic<AndroidJavaObject>("FileProvider.getUriForFile", unityContext, authority, fileObj);

        intent.Call<AndroidJavaObject>("setDataAndType", uri,
        "application/vnd.android.package-archive");
        intent.Call<AndroidJavaObject>("addFlags", FLAG_ACTIVITY_NEW_TASK);
        intent.Call<AndroidJavaObject>("setClassName",
        "com.android.packageinstaller",
        "com.android.packageinstaller.PackageInstallerActivity");

        intent.Call<AndroidJavaObject>("addFlags", FLAG_GRANT_READ_URI_PERMISSION);
    }
}
