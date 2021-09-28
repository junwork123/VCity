using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AndroidToast : MonoBehaviour
{

    public static AndroidToast instance;


#if UNITY_ANDROID
    AndroidJavaClass unityPlayer;
    AndroidJavaObject currentActivity;
    AndroidJavaObject context;
    AndroidJavaObject toast;
#endif


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayerActivity");
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        }
#endif

        DontDestroyOnLoad(this.gameObject);
    }

    public void ShowToast(string msg)
    {
#if UNITY_ANDROID
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
            AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", msg);

            toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
            toast.Call("show");
        }));
#endif
    }

    public void CancelToast()
    {
#if UNITY_ANDROID
        currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
        {
            if (toast != null)
                toast.Call("cancel");
        }));
#endif
    }
}
