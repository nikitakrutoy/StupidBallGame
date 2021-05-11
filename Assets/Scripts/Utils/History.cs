using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class EditAction
{
    public GameObject gameObject;
    public abstract void Cancel();

    public abstract void Apply();
}

public class MoveEditAction : EditAction
{
    private TransformDelta delta;

    public MoveEditAction(GameObject gameObject, TransformDelta delta)
    {
        this.gameObject = gameObject;
        this.delta = delta;
    }

    public override void Cancel()
    {
        delta.ApplyInverse(gameObject.transform);
    }
    
    public override void Apply()
    {
        delta.Apply(gameObject.transform);
    }
}


public class CreateEditAction : EditAction
{
    private GameObject button;

    public CreateEditAction(GameObject gameObject, GameObject button)
    {
        this.gameObject = gameObject;
        this.button = button;
    }

    public override void Cancel()
    {
        gameObject.SetActive(false);
        button.GetComponent<ItemCreator>().Increment();
    }
    
    public override void Apply()
    {
        gameObject.SetActive(true);
        button.GetComponent<ItemCreator>().DecrementAndDestroy();
    }

    ~CreateEditAction()
    {
        Object.Destroy(gameObject);
    }
}

public class DeleteEditAction : EditAction
{
    private GameObject button;

    public DeleteEditAction(GameObject gameObject, GameObject button)
    {
        this.gameObject = gameObject;
        this.button = button;
    }

    public override void Cancel()
    {
        gameObject.SetActive(true);
        button.GetComponent<ItemCreator>().Decrement();
    }
    
    public override void Apply()
    {
        gameObject.SetActive(false);
        button.GetComponent<ItemCreator>().Increment();
    }
}

;
public class History
{
    private int _currentActionId = -1;
    public List<EditAction> actions;
    public Dictionary<GameObject, TransformContainer> defaultLevelState;
    public Dictionary<GameObject, TransformContainer> CurrentLevelState;

    public History()
    {
        actions = new List<EditAction>();
        defaultLevelState = new Dictionary<GameObject, TransformContainer>();
        CurrentLevelState = new Dictionary<GameObject, TransformContainer>();
    }

    public void Add(EditAction a)
    {
        if (actions.Count - 1 != _currentActionId)
            actions.RemoveRange(
                _currentActionId + 1, 
                actions.Count - (_currentActionId + 1));
        actions.Add(a);
        _currentActionId += 1;
        
        if (a is DeleteEditAction)
        {
            CurrentLevelState.Remove(a.gameObject);
        }
        else
        {
            CurrentLevelState[a.gameObject] = a.gameObject.GetComponent<MovableObject>().restTransform.copy();
                ;
        }
    }

    public bool Undo()
    {
        bool success = _currentActionId >= 0;
        if (success)
        {
            actions.ElementAt(_currentActionId).Cancel();
            _currentActionId -= 1;
        }

        return success;

    }

    public bool Redo()
    {
        bool success = _currentActionId + 1 < actions.Count;
        if (success)
        {
            actions.ElementAt(_currentActionId + 1).Apply();
            _currentActionId += 1;
        }
        return success;
    }

    public void Reset()
    {
        foreach (var pair in defaultLevelState)
        {
            pair.Value.copyTo(pair.Key.transform);
            pair.Key.SetActive(true);
        }
        foreach (var pair in CurrentLevelState)
        {
            if (!defaultLevelState.ContainsKey(pair.Key))
            {
                GameObject.Destroy(pair.Key);
            }
        }
        CurrentLevelState = new Dictionary<GameObject, TransformContainer>(defaultLevelState);
        actions.Clear();
        _currentActionId = -1;
    }
    
}