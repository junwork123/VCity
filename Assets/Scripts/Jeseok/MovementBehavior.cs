using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MovementBehavior : MonoBehaviour
{
    [SerializeField]
    private float moveForce = 5f;
    [SerializeField]
    private float rotateSpeed = 2f;

    Rigidbody rigidbody;
    Vector3 velocity;
    Quaternion lookDir;
    Vector3 lookVector;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
        velocity = Vector3.zero;

        lookDir = Quaternion.Euler(lookVector * rotateSpeed * Time.fixedDeltaTime);
        rigidbody.MoveRotation(rigidbody.rotation * lookDir);
        lookVector = Vector3.zero;
    }

    public void Move(Vector3 dir)
    {
        velocity = dir * moveForce;

        lookDir = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, rotateSpeed * Time.deltaTime);
    }

    // First Person Point of view
    public void MoveFPS(Vector3 dir)
    {
        velocity = transform.forward * dir.z * moveForce;

        int sign = dir.z >= 0 ? 1 : -1;
        lookVector = new Vector3(0, dir.x * sign, 0);
    }
}
