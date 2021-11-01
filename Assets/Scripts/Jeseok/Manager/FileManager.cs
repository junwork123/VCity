using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.IO;
using TMPro;

public class FileManager : Singleton<FileManager>
{
    public bool devTest;
    string rootPath;
    public string fileName;

    UnityWebRequest www;

    public TextMeshProUGUI filePathText;
    public TextMeshProUGUI fileListText;

    DirectoryInfo dir;



    // Start is called before the first frame update
    void Start()
    {
        if (devTest == true)
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    rootPath = Application.persistentDataPath;
                    break;
                case RuntimePlatform.Android:
                    rootPath = Application.streamingAssetsPath;
                    break;
            }
        }
        else
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                    rootPath = Application.persistentDataPath;
                    break;

                case RuntimePlatform.WindowsPlayer:
                    rootPath = Application.persistentDataPath;
                    break;

                case RuntimePlatform.Android:
                    rootPath = Application.persistentDataPath;
                    break;

                case RuntimePlatform.IPhonePlayer:
                    rootPath = Application.persistentDataPath;
                    break;

                default:
                    rootPath = Application.persistentDataPath;
                    break;
            }
        }

        filePathText.text = rootPath;
        www = UnityWebRequest.Get(rootPath);
        print(string.Format("www : {0}", www));

        string[] dirs = Directory.GetFiles(rootPath, "*", SearchOption.TopDirectoryOnly);

        for (int i = 0; i < dirs.Length; ++i)
        {
            print(string.Format("dirs[{0}] : {1}", i, dirs[i]));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FileOpen()
    {
        print(System.Reflection.MethodBase.GetCurrentMethod());
    }
}
