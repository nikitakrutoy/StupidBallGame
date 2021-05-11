using System;
using UnityEngine;


public class JoystickCameraController : CameraController
{
    public Joystick movementJoystick;
    public Joystick rotationJoystick;
    public HeightSlider heightSlider;
    
    public float movementMultiplier = 1f;
    public float heightMultiplier = 1f;
    public float rotateMultiplier = 1f;

    private void Update()
    {
        Vector3 forwardDirection = Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized;
        Vector3 rightDirection = Vector3.ProjectOnPlane(transform.right, Vector3.up).normalized;
        transform.position += forwardDirection * (movementJoystick.Vertical * movementMultiplier);
        transform.position += rightDirection * (movementJoystick.Horizontal * movementMultiplier);
        transform.position += Vector3.up * (heightSlider.Amount * heightMultiplier);

        if (Mathf.Abs(rotationJoystick.Horizontal) > Mathf.Abs(rotationJoystick.Vertical))
        {
            transform.Rotate(
                Vector3.up, rotationJoystick.Horizontal * rotateMultiplier, Space.World);
        }
        else
        {
            transform.Rotate(
                transform.right, -rotationJoystick.Vertical * rotateMultiplier, Space.World);
        }
    }
}