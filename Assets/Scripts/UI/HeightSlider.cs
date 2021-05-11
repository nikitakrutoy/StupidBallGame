using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeightSlider : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerExitHandler
{
    public float Amount = 0;
    

    public void OnDrag(PointerEventData eventData)
    {
        if (Input.touchCount > 0)
            Amount = Input.GetTouch(0).deltaPosition.normalized.y;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Amount = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Amount = 0;
    }
}
