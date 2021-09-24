using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LookAtPlayer : MonoBehaviour
{
    bool isLook;
    Quaternion lookDir;
    public float rotateSpeed = 2f;

    private void Start() {
        transform.localRotation = Quaternion.LookRotation(Vector3.back);
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 lookPosition = other.transform.position;
            lookPosition.y = 0;
            isLook = true;
            lookDir = Quaternion.LookRotation(lookPosition - transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isLook = false;
            lookDir = Quaternion.LookRotation(Vector3.back);
        }
    }


}
