using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecordUI : MonoBehaviour
{
    void Awake() {
        DontDestroyOnLoad(this);
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        //Debug.Log("[Record] : " + NetServer.GetExternalIPAddress());
        //CloseConnection();
    }
    void Update()
    {

    }
}
