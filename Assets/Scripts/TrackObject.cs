using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackObject : MonoBehaviour
{
    public Transform target;
    public float height;
    public float distance;

    public float trackingSpeed = 1f;


    // Start is called before the first frame update
    void Start()
    {

    }

    private void LateUpdate()
    {
        // 카메라 위치
        transform.position = new Vector3(target.position.x, height, target.position.z - distance);

        // target 방향으로 각도 회전
        Vector3 dir = target.transform.position - transform.position;
        dir.Normalize();
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), trackingSpeed * Time.deltaTime);
    }
}
