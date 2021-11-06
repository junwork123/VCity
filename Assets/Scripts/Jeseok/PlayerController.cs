using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MovementBehavior movementBehavior;
    Animator animator;
    Rigidbody rigidbody;

    public KeyCode interactionKey = KeyCode.X;


    // Start is called before the first frame update
    void Start()
    {
        movementBehavior = GetComponent<MovementBehavior>();
        animator = GetComponentInChildren<Animator>();
        rigidbody = GetComponent<Rigidbody>();

        GameManager.instance.SetInteractionKey(interactionKey);
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(h, 0, v);
        Move(moveVector);

        bool isAction = Input.GetKeyDown(interactionKey);
        if (isAction == true)
        {
            Interaction();
        }
    }

    public void Move(Vector3 moveVector)
    {
        bool isMove = moveVector.magnitude != 0;

        animator.SetBool("isMove", isMove);

        if (isMove == false)
            return;

        // movementBehavior.Move(moveVector);
        movementBehavior.MoveFPS(moveVector);

    }

    public void Interaction()
    {
        UIButtonEventManager.instance.OnClickAction();
    }
}
