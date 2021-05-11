using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class DragCameraController : CameraController
{
    public float dragSpeed = 1;
    public float zoomSpeed = 1;
    public float minDistance = 10;
    public float maxDistance = 100;
    public Transform centerPoint;


    void Start()
    {
        Input.simulateMouseWithTouches = true;
        transform.LookAt(centerPoint);
    }
    
    protected void Zoom(float amount)
    {
        Vector3 newPosition = transform.position + transform.forward * amount;
        float distance = Vector3.Distance(newPosition, centerPoint.position);
        float oldDistance = Vector3.Distance(transform.position , centerPoint.position);
        if (distance < minDistance || (distance > oldDistance && amount > 0))
        {
            transform.position = centerPoint.position - transform.forward * minDistance;
        }
        else if (distance > maxDistance)
        {
            transform.position = centerPoint.position - transform.forward * maxDistance;
        }
        else
        {
            transform.position = newPosition;
        }
    }
}