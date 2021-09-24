using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement movementBehavior;


    // Start is called before the first frame update
    void Start()
    {
        movementBehavior = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 moveVector = new Vector3(h, 0, v);

        bool isMove = moveVector.magnitude != 0;

        if (isMove == true)
        {
            movementBehavior.Move(moveVector);
        }
    }

    public void Move(Vector3 moveVector)
    {
        movementBehavior.Move(moveVector);
    }
}
