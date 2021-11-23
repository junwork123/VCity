using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNavigationController : MonoBehaviour
{

    [Range(1f, 3f)]
    public float moveSpeed = 1f;
    public float rotateSpeed = 120;

    public float stopDistance = 2.5f;

    public Vector3 destination;

    public bool reachedDestination;

    Vector3 velocity;
    Vector3 lastPosition;

    public Animator anim;
    void Start()
    {
        moveSpeed = Random.Range(1f, 3f);
    }

    void Update()
    {
        if (transform.position != destination)
        {
            Vector3 destinationDirection = destination - transform.position;
            destinationDirection.y = 0;

            float destinationDistance = destinationDirection.magnitude;

            if (destinationDistance >= stopDistance)
            {
                //print(destinationDistance);
                reachedDestination = false;
                Quaternion targetRotation = Quaternion.LookRotation(destinationDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
                transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
            }
            else
            {
                reachedDestination = true;
            }

            velocity = (transform.position - lastPosition) / Time.deltaTime;
            velocity.y = 0;
            var valocityMagnitude = velocity.magnitude;
            velocity = velocity.normalized;
            var fwdDontProduct = Vector3.Dot(transform.forward, velocity);
            var rightDotProudct = Vector3.Dot(transform.right, velocity);
            anim.SetBool("isMove", true);
        }
    }

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
        reachedDestination = false;
    }
}
