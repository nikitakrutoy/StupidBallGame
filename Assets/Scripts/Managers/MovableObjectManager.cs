using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.Serialization;


public class MovableObjectManager : MonoBehaviour
{
    private MovableObject _activeObject;
    private Transform _activeObjectParent;
    private History _history;
    public GameObject toolsPanel;
    public GameObject itemsPanel;

    public bool RestrictModifications { get; set; } = false;
    
    public delegate void DisableInputAction();
    public static event DisableInputAction OnDisableInput;
    public Transform movableObjectsParent = null;
    
    public delegate void EnableInputAction();
    public static event EnableInputAction OnEnableInput;

    public void EnableInput()
    {
        RestrictModifications = false;
        if (OnEnableInput != null) OnEnableInput();
    }

    public void DisableInput()
    {
        DeclineActive();
        RestrictModifications = true;
        if (OnDisableInput != null) OnDisableInput();
    }
    
    public void SetMode(int mode)
    {
        _activeObject.controller.toolMode = ((ToolMode)mode);
        if (mode == 0) toolsPanel.GetComponent<EditPanel>().SetMoveMode();
        if (mode == 1) toolsPanel.GetComponent<EditPanel>().SetRotateMode();
    }

    public void SwitchSnap()
    {
        _activeObject.controller.doSnap = !_activeObject.controller.doSnap;
        toolsPanel.GetComponent<EditPanel>().SetSnap(_activeObject.controller.doSnap);
    }
    public void SetActive(MovableObject o)
    {
        if (_activeObject)
        {
            _activeObject.Decline();
        }
        
        _activeObject = o;
        _activeObjectParent = o.transform.parent;
        _activeObject.transform.parent = movableObjectsParent;
        toolsPanel.SetActive(true);
        toolsPanel.GetComponent<EditPanel>().Setup(
            _activeObject.controller.doSnap, _activeObject.allowDelete);
    }

    public void AcceptActive()
    {
        
        if (_activeObject.Accept())
        {
            EditAction editAction;
            toolsPanel.SetActive(false);
            if (!(_activeObject.createdFromPanel))
            {
                editAction  = new MoveEditAction(
                    _activeObject.gameObject,
                    new TransformDelta(_activeObject.restTransform, _activeObject.transform)
                );
            }
            else
            {
                GameObject button = itemsPanel.GetComponent<ItemsPanel>().GetOrCreateButton(_activeObject.gameObject);
                button.GetComponent<ItemCreator>().DecrementAndCreate();
                editAction = new CreateEditAction(
                    _activeObject.gameObject,
                    button
                );
                _activeObject.createdFromPanel = false;
            }
            _history.Add(editAction);
            _activeObject.controller.toolMode = ((ToolMode)0);
            _activeObject = null;
        }
        else
        {
            Debug.Log("Bad Position");
        }    
    }

    public void DeleteActive()
    {
        if (!RestrictModifications)
        {
            if (_activeObject.allowDelete)
            {
                toolsPanel.SetActive(false);
                _activeObject.Decline();
                GameObject button = itemsPanel.GetComponent<ItemsPanel>().GetOrCreateButton(_activeObject.gameObject);
                button.GetComponent<ItemCreator>().Increment();
                DeleteEditAction editAction = new DeleteEditAction(
                    _activeObject.gameObject,
                    button
                );
                _history.Add(editAction);
                _activeObject.gameObject.SetActive(false);
                _activeObject = null;
            }
            else
            {
                Debug.Log("Delete is not allowed");
            }

        }
    }
    
    public void DeclineActive()
    {
        if (_activeObject)
        {
            _activeObject.Decline();
            _activeObject.transform.parent = _activeObjectParent;
            _activeObjectParent = null;
            _activeObject.controller.toolMode = ((ToolMode)0);
            _activeObject = null;
            toolsPanel.SetActive(false);
        }
    }

    public void Undo()
    {
        if (!RestrictModifications)
            _history.Undo();
    }

    public void Redo()
    {
        if (!RestrictModifications)
            _history.Redo();
    }

    public void ResetObjectsState()
    {
        if (!RestrictModifications)
        {
            if (_activeObject)
            {
                DeclineActive();
            }
            _history.Reset();
            itemsPanel.GetComponent<ItemsPanel>().Reset();
        }
    }
    // Start is called before the first frame update

    public void Register(MovableObject obj)
    {
        if (!obj.createdFromPanel && obj.enabled)
        {
            _history.defaultLevelState[obj.gameObject] = obj.restTransform.copy();
            _history.CurrentLevelState[obj.gameObject] = obj.restTransform.copy();
        }
    }
    void Start()
    {
        _history = new History();
    }
    
}
