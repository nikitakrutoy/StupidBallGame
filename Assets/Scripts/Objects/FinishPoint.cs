using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FinishPoint : MonoBehaviour
{
    private static readonly int Direction = Shader.PropertyToID("_Direction");
    private static readonly int Height = Shader.PropertyToID("_Height");
    private Vector3 startScale;
    public float animationScale = 0.2f;
    public float animationSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Renderer render = GetComponent<Renderer>(); 
        Material mat = render.sharedMaterial;
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        Vector3 _up = transform.up;
        Vector3.Dot(_up, _up);
        mat.SetVector(Direction, _up);
        mat.SetFloat(Height, mesh.bounds.max.y - mesh.bounds.min.y);
        startScale = transform.localScale;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 s = new Vector3(      
            startScale.x + animationScale * Mathf.Sin(Time.time * animationSpeed),
            startScale.y,
            startScale.z + animationScale * Mathf.Sin(Time.time * animationSpeed));
        transform.localScale = s;
    }
}
