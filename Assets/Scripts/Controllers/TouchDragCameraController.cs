using UnityEngine;

public class TouchDragCameraController : DragCameraController
{
    void ProcessInput()
    {
        Vector3 viewportDiff = Vector3.zero;
        if (Input.touchCount > 0)
            viewportDiff = Input.GetTouch(0).deltaPosition;


        if (viewportDiff != Vector3.zero)
        {
            if (Mathf.Abs(viewportDiff.x) > Mathf.Abs(viewportDiff.y))
            {
                transform.RotateAround(centerPoint.position, Vector3.up, viewportDiff.x * dragSpeed);
                transform.LookAt(centerPoint);
            }
            else
            {
                Zoom(viewportDiff.y * zoomSpeed);
            }
        }
    }

    void Update()
    {
        ProcessInput();
    }
}
