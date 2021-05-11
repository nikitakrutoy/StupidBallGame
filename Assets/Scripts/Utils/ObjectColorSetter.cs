using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ObjectColorSetter : MonoBehaviour
{
    public Material movableMaterial;
    public Material gravityMaterial;
    public Material noCollisionMaterial;
    public Material fixedMaterial;
    public Material errorMaterial;

    private MovableObject _movableObject;
    // Start is called before the first frame update
    void Start()
    {
        Renderer render = GetComponent<Renderer>();
        MovableObject movableObject = GetComponent<MovableObject>();
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        SimulationObject simulationObject = GetComponent<SimulationObject>();
        if (movableObject == null && rigidbody == null) render.sharedMaterial = fixedMaterial;
        else if (!rigidbody.isKinematic && simulationObject) render.sharedMaterial = gravityMaterial;
        else if (!movableObject.detectCollisions && !rigidbody.isKinematic) 
            render.sharedMaterial = errorMaterial;
        else if (!movableObject.detectCollisions) render.sharedMaterial = noCollisionMaterial;
        else if (movableObject) render.sharedMaterial = movableMaterial;

    }

    // Update is called once per frame
}
