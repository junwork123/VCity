using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Android 기기에서 실행 가능
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
        unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        if (unityPlayer != null)
        {
            print(unityPlayer);
            currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
        if (currentActivity != null)
            context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
#endif

        DontDestroyOnLoad(this.gameObject);
    }

    // 기존에 출력중인 Toast 제거후 message 출력
    public void ShowToast(string message)
    {
#if UNITY_ANDROID
        CancelToast();

        print(message);
        if (context != null)
            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
                AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", message);

                toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
                toast.Call("show");
            }));
#endif
    }

    public void CancelToast()
    {
#if UNITY_ANDROID
        if (context != null)
            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                if (toast != null)
                    toast.Call("cancel");
            }));
#endif
    }
}
