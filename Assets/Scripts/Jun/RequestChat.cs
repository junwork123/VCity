using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Chat;

public class RequestChat : MonoBehaviour
{

    public GameObject loadingPanel;
    // Start is called before the first frame update
    private void OnEnable()
    {
        loadingPanel.SetActive(true);
        RequestNewChannelWrapper();
    }

    public void RequestNewChannelWrapper()
    {
        StartCoroutine(RequestNewChannel());
    }
    public IEnumerator RequestNewChannel()
    {

        string channelName = "민원상담 " + DateTime.Now.ToString(("(yyyy-MM-dd)")) + ")";
        yield return StartCoroutine(DataManager.Instance.CreateChannel(channelName));
        yield return StartCoroutine(DataManager.Instance.SubscribeChannel(DataManager.Instance.lastCreatedChannel));
        yield return StartCoroutine(ChatManager.Instance.UpdateRoomList());
        ChatManager.Instance.ShowChannel(DataManager.Instance.lastCreatedChannel);
        ChatManager.Instance.GetComponent<MenuManager>().OpenMenu("DISPLAY_MSGS");
        loadingPanel.SetActive(false);
    }
}
