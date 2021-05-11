using UnityEngine;



public class TransformDelta
{
    public Vector3 position;
    public Quaternion destRotation;
    public Quaternion sourceRotation;

    public TransformDelta (TransformContainer source, Transform dest)
    {
        position = dest.position - source.position;
        destRotation = dest.rotation;
        sourceRotation = source.rotation;
        // rotation = dest.rotation * Quaternion.Inverse(source.rotation);
    }

    public void Apply(Transform t)
    {
        t.position += position;
        // t.rotation *= rotation;
        t.rotation = destRotation;
    }
    
    
    public void ApplyInverse(Transform t)
    {
        t.position -= position;
        // t.rotation *= Quaternion.Inverse(rotation);
        t.rotation = sourceRotation;
    }
    
}

public class TransformContainer
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 localScale;
 
    public TransformContainer (Vector3 newPosition, Quaternion newRotation, Vector3 newLocalScale)
    {
        position = newPosition;
        rotation = newRotation;
        localScale = newLocalScale;
    }
 
    public TransformContainer ()
    {
        position = Vector3.zero;
        rotation = Quaternion.identity;
        localScale = Vector3.one;
    }
 
    public TransformContainer (Transform transform)
    {
        copyFrom (transform);
    }
 
    public void copyFrom (Transform transform)
    {
        position = transform.position;
        rotation = transform.rotation;
        localScale = transform.localScale;
    }
 
    public void copyTo (Transform transform)
    {
        transform.position = position;
        transform.rotation = rotation;
        transform.localScale = localScale;
    }

    public TransformContainer copy()
    {
        TransformContainer result = new TransformContainer();
        result.position = position;
        result.rotation = rotation;
        result.localScale = localScale;
        return result;
    }
 
}