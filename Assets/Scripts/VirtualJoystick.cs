using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    RectTransform lever;
    RectTransform rectTransform;
    Vector2 sizeOffset;


    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    Vector2 inputVector;
    bool isInput;


    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        print(rectTransform);
        sizeOffset = new Vector2(rectTransform.sizeDelta.x * 0.5f, rectTransform.sizeDelta.y * 0.5f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
    }

    public void ControlJoystickLever(PointerEventData eventData)
    {
        inputVector = (eventData.position - (rectTransform.anchoredPosition + sizeOffset));

        Vector2 clampedVector = (inputVector.magnitude < leverRange) ?
            inputVector : inputVector.normalized * leverRange;

        lever.anchoredPosition = clampedVector;

        inputVector = clampedVector / leverRange;
    }
}
