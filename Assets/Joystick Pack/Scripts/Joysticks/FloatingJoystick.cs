using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (Input.touchCount == 0) return;
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Default");
        Ray ray = Camera.main.ScreenPointToRay(eventData.pressPosition);
        bool didHit = false;
        didHit = Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value);
        if (!didHit)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            background.gameObject.SetActive(true);
            base.OnPointerDown(eventData);
            isDown = true;
        }
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (Input.touchCount == 0) return;
        if (isDown)
        {
            background.gameObject.SetActive(false);
            base.OnPointerUp(eventData);
            isDown = false;
        }
    }
}