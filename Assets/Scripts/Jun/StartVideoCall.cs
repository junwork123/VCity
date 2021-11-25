using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartVideoCall : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnEnable() {
        VideoManager.instance.CheckAppId();
        VideoManager.instance.StartVideoCall();
    }
}
