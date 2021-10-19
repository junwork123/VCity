using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private MovementBehavior movementBehavior;

    public KeyCode interactionKey = KeyCode.X;


    // Start is called before the first frame update
    void Start()
    {
        movementBehavior = GetComponent<MovementBehavior>();

        GameManager.instance.SetInteractionKey(interactionKey);
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
            Move(moveVector);
        }

        bool isAction = Input.GetKeyDown(interactionKey);
        if (isAction == true)
        {
            Interaction();
        }
    }

    public void Move(Vector3 moveVector)
    {
        // movementBehavior.Move(moveVector);
        movementBehavior.MoveFPS(moveVector);
    }

    public void Interaction()
    {
        UIButtonEventManager.instance.OnClickActionButton();
    }
}