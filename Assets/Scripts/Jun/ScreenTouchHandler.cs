using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.EventSystems;

public class ScreenTouchHandler : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    RectTransform rect;
    bool isDrag = false;

    public delegate void FuncHandler();
    public FuncHandler mOnPointerUp;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        Vector3.Lerp(rect.position, eventData.position, 0.1f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDrag)
        {
            
            mOnPointerUp();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = false;
    }

}
