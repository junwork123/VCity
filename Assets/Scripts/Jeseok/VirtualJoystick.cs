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
    bool isInput;

    [Header("Controller")]
    public PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        sizeOffset = new Vector2(rectTransform.sizeDelta.x * 0.5f, rectTransform.sizeDelta.y * 0.5f);
    }

    private void OnEnable()
    {
        joystick.SetActive(false);
        lever.SetActive(true);
        isInput = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isInput == true)
        {
            InputControlVector();
        }
    }

    private void OnDisable()
    {
        DisableJoystick();
    }

    #region Drag Event
    public void OnPointerDown(PointerEventData eventData)
    {
        EnableJoyStick(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
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
        joystick.transform.position = eventData.position;
        lever.transform.position = eventData.position;

        joystick.SetActive(true);
    }

    void DisableJoystick()
    {
        playerController.Move(Vector3.zero);
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
