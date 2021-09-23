using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    RectTransform lever;
    RectTransform rectTransform;

    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    Vector2 inputVector;
    bool isInput;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        print(inputVector);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
        isInput = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        ControlJoystickLever(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        isInput = false;
    }

    public void ControlJoystickLever(PointerEventData eventData)
    {
        Vector2 inputDir = eventData.position - rectTransform.anchoredPosition;
        Vector2 clampedDir = (inputDir.magnitude < leverRange) ?
            inputDir : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;

        inputVector = clampedDir / leverRange;
    }
}
