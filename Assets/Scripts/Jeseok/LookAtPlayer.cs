using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public float rotateSpeed = 2f;

    Quaternion defaultDir;
    Quaternion lookDir;

    #region debug
    SphereCollider collider;
    public float lookRange = 4f;
    #endregion

    private void Start()
    {
        defaultDir = gameObject.GetComponentInParent<Transform>().rotation;

        collider = GetComponent<SphereCollider>();
        collider.radius = lookRange;
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
            lookDir = Quaternion.LookRotation(lookPosition - transform.position);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            lookDir = defaultDir;
        }
    }

    #region Debug
    private void OnDrawGizmos()
    {
        Color color = new Color(255/255,127/255, 80/255);
        color.a = 0.1f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, lookRange);
    }
    #endregion
}
