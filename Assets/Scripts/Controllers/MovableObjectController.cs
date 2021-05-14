using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ToolMode 
{
    Move = 0,
    Rotate = 1
}

public class MovableObjectController : MonoBehaviour
{
    public delegate void ActivateAction();
    public static event ActivateAction OnActivate;
    public delegate void DeactivateAction();
    public static event DeactivateAction OnDeactivate;

    private MovableObject _movableObject;

    private Mesh _mesh;
    
    private float _mZCoord;
    private Vector3 _mOffset;
    private Vector3 _mNormal;
    private bool _bIsSnapped = false;
    private Vector3 _mouseStartPosition;
    private SnapDirection _snapDirection = SnapDirection.Y;

    public float snapOffset = 0.1f;
    public bool doSnap = true;
    public float freeRotationCoef = 1f;
    public float snapRotationCoef = 1f;

    [HideInInspector]
    public ToolMode toolMode = ToolMode.Move;
    [HideInInspector]
    public bool blockInput = false; 
    private void Start()
    {
        _movableObject = GetComponent<MovableObject>();
        _mesh = GetComponent<MeshFilter>().sharedMesh;
    }

    public void ProcessRotationOnMouseDown()
    {
        _mNormal = GetSnapDirectionVector(_snapDirection);
        _mouseStartPosition = Input.mousePosition;
    }
    
    public void ProcessRotationOnMouseDrag()
    {
        Vector2 diff = Input.mousePosition - _mouseStartPosition;
        if (doSnap && _bIsSnapped)
        {
            Vector2 roteteDirT =  Camera.main.WorldToScreenPoint(transform.position + _mNormal) - 
                                  Camera.main.WorldToScreenPoint(transform.position);
            ;
            Vector2 rotateDir = Vector2.Perpendicular(roteteDirT);
            float rotateAmount = Vector2.Dot(diff.normalized, rotateDir.normalized);
            transform.rotation *= Quaternion.AngleAxis(rotateAmount * snapRotationCoef, transform.InverseTransformDirection(_mNormal));    
            // transform.rotation *= Quaternion.Euler(0, rotateAmount, 0); 
        }
        else
        {
            transform.rotation *= Quaternion.AngleAxis(
                                      Mathf.Clamp(
                                          diff.normalized.x * freeRotationCoef, -180, 180), 
                                      Vector3.up) *
                                  Quaternion.AngleAxis(
                                      Mathf.Clamp(
                                          diff.normalized.y * freeRotationCoef, -180, 180), 
                                      Vector3.right);
        }

        _mouseStartPosition = Input.mousePosition;
    }

    public void ProcessMovementOnMouseDown()
    {
        _mZCoord = Camera.main.WorldToScreenPoint(
            gameObject.transform.position).z;
        // Store offset = gameobject world pos - mouse world pos
        _mOffset = gameObject.transform.position - GetMouseAsWorldPoint();
    }

    private enum SnapDirection
    {
        X, Y, Z, NX, NY, NZ
    }
    
    private SnapDirection ComputeSnapDirection(RaycastHit hit)
    {
        SnapDirection snapDirection = SnapDirection.Y;
        var y = Vector3.Dot(transform.up, hit.normal);
        var x= Vector3.Dot(transform.right, hit.normal);
        var z = Vector3.Dot(transform.forward, hit.normal);
        if (Mathf.Abs(x) > Mathf.Abs(y) && Mathf.Abs(x) > Mathf.Abs(z) && (x > 0))
            snapDirection = SnapDirection.X;
        if (Mathf.Abs(x) > Mathf.Abs(y) && Mathf.Abs(x) > Mathf.Abs(z) && (x < 0))
            snapDirection = SnapDirection.NX;
        if (Mathf.Abs(y) > Mathf.Abs(x) && Mathf.Abs(y) > Mathf.Abs(z) && (y > 0))
            snapDirection = SnapDirection.Y;
        if (Mathf.Abs(y) > Mathf.Abs(x) && Mathf.Abs(y) > Mathf.Abs(z) && (y < 0))
            snapDirection = SnapDirection.NY;
        if (Mathf.Abs(z) > Mathf.Abs(y) && Mathf.Abs(z) > Mathf.Abs(x) && (z > 0))
            snapDirection = SnapDirection.Z;
        if (Mathf.Abs(z) > Mathf.Abs(y) && Mathf.Abs(z) > Mathf.Abs(x) && (z < 0))
            snapDirection = SnapDirection.NZ;
        return snapDirection;
    }

    private float GetLocalOffset(SnapDirection snapDirection)
    {
        float result = 0;
        switch (snapDirection)
        {
            case SnapDirection.X:
                result = -_mesh.bounds.min.x * transform.localScale.x;
                break;
            case SnapDirection.NX:
                result = _mesh.bounds.max.x * transform.localScale.x;
                break;
            case SnapDirection.Y:
                result = -_mesh.bounds.min.y * transform.localScale.y;
                break;
            case SnapDirection.NY:
                result = _mesh.bounds.max.y * transform.localScale.y;
                break;
            case SnapDirection.Z:
                result = -_mesh.bounds.min.z * transform.localScale.z;
                break;
            case SnapDirection.NZ:
                result = _mesh.bounds.max.z * transform.localScale.z;
                break;
        }
        return result;
    }

    private Vector3 GetSnapDirectionVector(SnapDirection snapDirection)
    {
        Vector3 result = Vector3.zero;
        switch (snapDirection)
        {
            case SnapDirection.X:
                result = transform.right;
                break;
            case SnapDirection.NX:
                result = -transform.right;
                break;
            case SnapDirection.Y:
                result = transform.up;
                break;
            case SnapDirection.NY:
                result = -transform.up;
                break;
            case SnapDirection.Z:
                result = transform.forward;
                break;
            case SnapDirection.NZ:
                result = -transform.forward;
                break;
        }
        return result;
    }
    
    public void ProcessMovementOnMouseDrag()
    {
        RaycastHit hit;
        LayerMask mask = LayerMask.GetMask("Default");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool didHit = false;
        didHit = Physics.Raycast(ray, out hit, Mathf.Infinity, mask.value);
        if (didHit && doSnap)
        {
            if (!_bIsSnapped)
            {
                _bIsSnapped = true;
                _snapDirection = ComputeSnapDirection(hit);
            }

            float localOffset = GetLocalOffset(_snapDirection);
            Vector3 snapDirectionVector = GetSnapDirectionVector(_snapDirection);
            
            if ((snapDirectionVector - hit.normal).magnitude > 0.01)
            {
                Quaternion rotation = Quaternion.FromToRotation(snapDirectionVector, hit.normal);
                Vector3 up = rotation * transform.up;
                Vector3 forward = rotation * transform.forward;
                transform.rotation = Quaternion.LookRotation(forward, up);
            }
            transform.position = hit.point + hit.normal.normalized * (localOffset + snapOffset);
        }
        else
        {
            _bIsSnapped = false;
            transform.position = GetMouseAsWorldPoint() + _mOffset;
        }
    }

    public void OnMouseDown()
    {
        if (blockInput) return;
        if (!_movableObject.IsActive)
        {
            _movableObject.Activate();
        }
        if (OnActivate != null) OnActivate();
        switch (toolMode)
        {
            case ToolMode.Move:
                ProcessMovementOnMouseDown();
                break;
            case ToolMode.Rotate:
                ProcessRotationOnMouseDown();
                break;
        }
    }
    
    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = _mZCoord;
        
        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
    
    void OnMouseDrag()
    {
        if (blockInput) return;
        switch (toolMode)
        {
            case ToolMode.Move:
                ProcessMovementOnMouseDrag();
                break;
            case ToolMode.Rotate:
                ProcessRotationOnMouseDrag();
                break;
        }
        
    }

    private void OnMouseUp()
    {
        if (OnDeactivate != null) OnDeactivate();
    }
    // Start is called before the first frame update
}
