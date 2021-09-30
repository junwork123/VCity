using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Android 기기에서 실행했을 때 Toast 메시지 출력
/// <list>AndroidToastManager.instance.ShowToast(string message)</list>
/// </summary>
public class AndroidToastManager : MonoBehaviour
{
    public static AndroidToastManager instance;


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
            unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            if (unityPlayer != null)
                currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            if (currentActivity != null)
                context = currentActivity.Call<AndroidJavaObject>("getApplicationContext");
        }
#endif

        DontDestroyOnLoad(this.gameObject);
    }

    // 기존에 출력중인 Toast 제거후 message 출력
    public void ShowToast(string message)
    {
#if UNITY_ANDROID
        CancelToast();

        if (Application.platform == RuntimePlatform.Android)
        {
            if (context == null)
                return;

            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaClass Toast = new AndroidJavaClass("android.widget.Toast");
                AndroidJavaObject javaString = new AndroidJavaObject("java.lang.String", message);

                toast = Toast.CallStatic<AndroidJavaObject>("makeText", context, javaString, Toast.GetStatic<int>("LENGTH_SHORT"));
                toast.Call("show");
            }));
        }
#endif
    }

    public void CancelToast()
    {
#if UNITY_ANDROID
        if (Application.platform == RuntimePlatform.Android)
        {
            if (context == null)
                return;

            if (toast == null)
                return;

            currentActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                toast.Call("cancel");
            }));
        }
#endif
    }
}
