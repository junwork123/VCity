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


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, rotateSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
        velocity = Vector3.zero;
    }

    public void Move(Vector3 dir)
    {
        velocity = dir * moveForce;

        lookDir = Quaternion.LookRotation(dir);
    }
}
