using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveForce = 5f;
    [SerializeField]
    private float rotateSpeed = 2f;

    Rigidbody rigidbody;
    Vector3 velocity;
    Quaternion lookDir;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        // Vector3 dir = (aim.transform.position - transform.position);
        // dir.y = 0;

        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, rotateSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
    }

    public void Move(Vector3 dir)
    {
        velocity = dir * moveForce;

        lookDir = Quaternion.LookRotation(velocity);
    }
}
