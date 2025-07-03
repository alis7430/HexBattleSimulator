using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

// Click, Drag등의 UI이벤트를 캐치하여 콜백으로 날려줌
public class UI_EventHandler : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public Action<PointerEventData> OnClickHandler = null;
    public Action<PointerEventData> OnDragHandler = null;
    
    public void OnDrag(PointerEventData eventData)
    {
        OnDragHandler?.Invoke(eventData);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClickHandler?.Invoke(eventData);
    }
}
