#define CharacterCollider

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementBehavior : MonoBehaviour
{
    [SerializeField]
    private float moveForce = 5f;
    [SerializeField]
    private float rotateSpeed = 2f;

    Rigidbody rigidbody;
    CharacterController characterController;
    Vector3 velocity;
    Quaternion lookDir;
    Vector3 lookVector;

    bool isMove;


    private void Start()
    {
#if CharacterCollider
        characterController = GetComponent<CharacterController>();
#else
        rigidbody = GetComponent<Rigidbody>();
#endif
    }

#if CharacterCollider
    private void Update()
    {
        if (isMove == false)
            return;

        // characterController.Move(velocity * Time.deltaTime);
        characterController.SimpleMove(velocity);
        velocity = Vector3.zero;

        lookDir = Quaternion.Euler(lookVector * rotateSpeed * Time.fixedDeltaTime);
        transform.rotation *= lookDir;
        lookVector = Vector3.zero;
    }
#endif


#if Rigidbody
    private void FixedUpdate()
    {
        if (isMove == false)
            return;
            
        rigidbody.MovePosition(rigidbody.position + velocity * Time.deltaTime);
        velocity = Vector3.zero;

        lookDir = Quaternion.Euler(lookVector * rotateSpeed * Time.fixedDeltaTime);
        rigidbody.MoveRotation(rigidbody.rotation * lookDir);
        lookVector = Vector3.zero;

        isMove = false;
    }
#endif


    public void Move(Vector3 dir)
    {
        isMove = true;

        velocity = dir * moveForce;

        lookDir = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, rotateSpeed * Time.deltaTime);
    }

    // First Person Point of view
    public void MoveFPS(Vector3 dir)
    {
        isMove = true;

        velocity = transform.forward * dir.z * moveForce;

        int sign = dir.z >= 0 ? 1 : -1;
        lookVector = new Vector3(0, dir.x * sign, 0);
    }
}
