using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MovableObjectColorSetter : MonoBehaviour
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
        Rigidbody rg = GetComponent<Rigidbody>();
        SimulationObject simulationObject = GetComponent<SimulationObject>();
        if (movableObject == null && rg == null)
        {
            render.sharedMaterial = fixedMaterial;
            return;
        }

        if (movableObject.movableObjectType == MovableObjectType.NoCollision && !rg.isKinematic)
        {
            render.sharedMaterial = errorMaterial;
            return;
        }

        switch (movableObject.movableObjectType)
        {
            case MovableObjectType.Standard:
                render.sharedMaterial = movableMaterial;
                break;
            case MovableObjectType.NoCollision:
                render.sharedMaterial = noCollisionMaterial;
                break;
            case MovableObjectType.Gravity:
                render.sharedMaterial = gravityMaterial;
                break;
        }
    }
}
