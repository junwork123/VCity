using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{

    CharacterController controller;
    public float speed = 1;
    public float jumpPower = 15f;

    public bool movementEnabled = false;

    private Vector3 target;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        target = transform.position;
        int num;
        if (int.TryParse(this.gameObject.name,out num))
        {
            switch (num)
            {
                case 1:
                    SetColor(Color.red);
                    break;
                case 2:
                    SetColor(Color.green);
                    break;
                case 3:
                    SetColor(Color.blue);
                    break;
                
            }
            if (num > 3)
            {
                SetColor(Color.grey);
            }
        }
    }

    void SetColor(Color c)
    {
        this.GetComponent<MeshRenderer>().material.color = c;
    }

    // Update is called once per frame
    void Update()
    {
        float z = Input.GetAxis("Vertical");
        float x = Input.GetAxis("Horizontal");
        float jump = Input.GetAxis("Jump");

        if (movementEnabled)
        {
            //controller.Move(transform.forward * z * Time.deltaTime * speed + transform.right * x * Time.deltaTime * speed + (Physics.gravity * Time.deltaTime));
            controller.Move(transform.forward * z * Time.deltaTime * speed + (Physics.gravity * Time.deltaTime));
            gameObject.transform.Rotate(0, x*10f, 0);
        }else
        {
           transform.position=Vector3.Lerp(transform.position, target, 0.09f);

        }
    





    }

    //sets target position to which object lerps to
    public void SetTarget(Vector3 posn)
    {
        target = posn;
    }
}
