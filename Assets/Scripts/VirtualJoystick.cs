using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IEndDragHandler, IPointerUpHandler
{
    [SerializeField]
    GameObject joystick;
    [SerializeField]
    GameObject lever;

    RectTransform rectTransform;
    Vector2 sizeOffset;

    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    Vector2 inputVector;
    bool onJoystick;
    bool isInput;

    public PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        joystick.SetActive(false);
        lever.SetActive(true);

        rectTransform = GetComponent<RectTransform>();

        sizeOffset = new Vector2(rectTransform.sizeDelta.x * 0.5f, rectTransform.sizeDelta.y * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInput == true)
        {
            InputControlVector();
        }
    }

    #region Drag Event
    public void OnPointerDown(PointerEventData eventData)
    {
        EnableJoyStick(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onJoystick == false)
            return;

        isInput = true;

        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isInput = false;

        DisableJoystick();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        joystick.SetActive(false);
    }
    #endregion

    void EnableJoyStick(PointerEventData eventData)
    {
        // 화면의 다른 UI를 클릭하면 실행하지 않음
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        LayerMask layerMask = LayerMask.GetMask("UI");

        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);
        for(int i = 0; i < hits.Length; ++i)
        {
            print(hits[i].collider.name);
        }

        // if (Physics.Raycast(ray, out hit))
        // {
        //     print(hit.collider.transform.name);
        //     print(LayerMask.LayerToName(hit.collider.transform.gameObject.layer));
        //     return;
        // }
        // else
        // {
        //     print(hit);
        //     onJoystick = true;
        // }


        joystick.transform.position = eventData.position;
        lever.transform.position = eventData.position;

        joystick.SetActive(true);
    }

    void DisableJoystick()
    {
        joystick.SetActive(false);
    }

    public void ControlJoystickLever(PointerEventData eventData)
    {
        Vector2 inputDir = eventData.position - (Vector2)joystick.transform.position;
        Vector2 clampedVector = (inputDir.magnitude < leverRange) ?
            inputDir : inputDir.normalized * leverRange;

        lever.transform.localPosition = clampedVector;
        inputVector = clampedVector / leverRange;
    }

    private void InputControlVector()
    {
        if (playerController != null)
        {
            Vector3 convertVector = new Vector3(inputVector.x, 0, inputVector.y);
            convertVector.Normalize();
            playerController.Move(convertVector);
        }
    }
}
