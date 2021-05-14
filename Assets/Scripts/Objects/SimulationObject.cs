using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationObject : MonoBehaviour
{
    private Rigidbody _rg;
    private Vector3 _velocity = Vector3.zero;
    private TransformContainer _restTransform;
    void Start()
    {
        _rg = GetComponent<Rigidbody>();
        _restTransform = new TransformContainer();
        UpdateRest();
    }

    public void UpdateRest()
    {
        _restTransform = new TransformContainer();
        _restTransform.copyFrom(transform);
    }

    public void ResetState()
    {
        _rg.velocity = Vector3.zero;
        _velocity = Vector3.zero;
        _rg.useGravity = false;
        _rg.isKinematic = true;
        _restTransform.copyTo(transform);
    }
    
    public void PauseGravity()
    {
        _velocity = _rg.velocity;
        _rg.velocity = Vector3.zero;
        _rg.useGravity = false;
        _rg.isKinematic = true;
    }

    public void ResumeGravity()
    {
        _rg.velocity = _velocity;
        _rg.useGravity = true;
        _rg.isKinematic = false;
    }
    
    void OnEnable()
    {
        SimulationManager.OnActivate += ResumeGravity;
        SimulationManager.OnPause += PauseGravity;
        SimulationManager.OnReset += ResetState;
    }


    void OnDisable()
    {
        SimulationManager.OnActivate -= ResumeGravity;
        SimulationManager.OnPause -= PauseGravity;
        SimulationManager.OnReset -= ResetState;
    }

}
