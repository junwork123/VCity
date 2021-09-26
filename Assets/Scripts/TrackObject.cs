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
        Vector3 targetPosition = new Vector3(target.position.x, height, target.position.z - distance);
        transform.position = Vector3.Lerp(this.transform.position, targetPosition, trackingSpeed * Time.deltaTime);
    }
}
