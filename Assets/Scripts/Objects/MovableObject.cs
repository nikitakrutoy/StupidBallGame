// source: https://www.patreon.com/posts/unity-3d-drag-22917454

using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum MovableObjectType
{
    Standard = 0,
    NoCollision = 1,
    Gravity = 2
} 
public class MovableObject : MonoBehaviour
{
    public bool IsActive { get; private set; }

    private bool _bAcceptPosition;

    [HideInInspector]
    public bool createdFromPanel = false;
    
    private Renderer _renderer;
    private Collider _collider;
    private Material _defaultMaterial;
    private HashSet<GameObject> _collidedObjectsSet;
    
    public MovableObjectController controller { get; private set; }


    public Material acceptMaterial;
    public Material declineMaterial;
    public bool allowDelete = false;
    public MovableObjectType movableObjectType = MovableObjectType.Standard;
    
    public MovableObjectManager manager;
    
    public TransformContainer restTransform { get; private set; }

    public void OnDisableController() { controller.blockInput = true;}
    public void OnEnableController() { controller.blockInput = false;}
    
    void OnEnable()
    {
        MovableObjectManager.OnDisableInput += OnDisableController;
        MovableObjectManager.OnEnableInput += OnEnableController;
    }


    void OnDisable()
    {
        MovableObjectManager.OnDisableInput -= OnDisableController;
        MovableObjectManager.OnEnableInput -= OnEnableController;
    }
    

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        _collidedObjectsSet = new HashSet<GameObject>();
        restTransform = new TransformContainer();
        restTransform.copyFrom(transform);
        controller = GetComponent<MovableObjectController>();
        _renderer = GetComponent<Renderer>();
        _collider = GetComponent<Collider>();
        _defaultMaterial = _renderer.sharedMaterial;
        IsActive = false;
        if (!manager)
        {
            manager = FindObjectOfType<MovableObjectManager>();
        }

        manager.Register(this);
    }

    public void Activate()
    {
        IsActive = true;
        gameObject.layer = 8;
        manager.SetActive(this);
        _renderer.material = acceptMaterial;
        _collider.isTrigger = true;
        restTransform.copyFrom(transform);
        _bAcceptPosition = true;
    }
    
    private void Deactivate()
    {
        _renderer.material = _defaultMaterial;
        _collider.isTrigger = false;
        IsActive = false;
        gameObject.layer = 0;
    }
    
    public bool Accept()
    {
        if (_bAcceptPosition)
        {
            Deactivate();
        }
        return _bAcceptPosition;
    }

    public void Decline()
    {
        Deactivate();
        restTransform.copyTo(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsActive && movableObjectType != MovableObjectType.NoCollision)
        {
            _renderer.material = declineMaterial;
            _bAcceptPosition = false;
            _collidedObjectsSet.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsActive && movableObjectType != MovableObjectType.NoCollision)
        {
            _collidedObjectsSet.Remove(other.gameObject);
            if (_collidedObjectsSet.Count == 0)
            {
                _renderer.material = acceptMaterial;
                _bAcceptPosition = true;
            }   
        }
    }
}