using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Forward 방향으로 Rotation 고정
/// </summary>
public class LookForward : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
