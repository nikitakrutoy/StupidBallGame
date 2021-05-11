using UnityEngine;

public class MouseDragCameraController : DragCameraController
{
    private Vector3 _prevDragPosition;
    private bool _bIsMouseDragging = false;


    // Start is called before the first frame update

    void ProcessInput()
    {
        if (Input.touchCount > 0) return;
        if (Input.GetMouseButtonDown(0) && !_bIsMouseDragging)
        {
            _bIsMouseDragging = true;
            _prevDragPosition = Input.mousePosition;
            return;
        }

        if (Input.GetMouseButtonUp(0) && _bIsMouseDragging)
        {
            _bIsMouseDragging = false;
        }

        if (_bIsMouseDragging)
        {
            Vector3 screenDiff = Input.mousePosition - _prevDragPosition;
            Vector3 viewportDiff = Camera.main.ScreenToViewportPoint(screenDiff);
            transform.RotateAround(centerPoint.position, Vector3.up, viewportDiff.x * dragSpeed);
            transform.LookAt(centerPoint);
            _prevDragPosition = Input.mousePosition;
        }

        float zoomAmount = Input.GetAxis("Mouse ScrollWheel");
        if(zoomAmount!= 0) {
            Zoom(zoomAmount * zoomSpeed);
        }
    }

    void Update()
    {
        ProcessInput();
    }
}