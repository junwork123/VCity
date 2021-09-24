using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    RectTransform lever;
    RectTransform rectTransform;
    Vector2 sizeOffset;


    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    Vector2 inputVector;
    bool isInput;

    public Transform player;
    public PlayerController playerController;


    // Start is called before the first frame update
    void Start()
    {
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

    public void OnBeginDrag(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        isInput = true;

        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;

        isInput = false;
    }

    public void ControlJoystickLever(PointerEventData eventData)
    {
        Vector2 inputDir = (eventData.position - (rectTransform.anchoredPosition + sizeOffset));

        Vector2 clampedVector = (inputDir.magnitude < leverRange) ?
            inputDir : inputDir.normalized * leverRange;

        lever.anchoredPosition = clampedVector;

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
