using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class StupidBall : MonoBehaviour
{
    public delegate void DefeatDelegate();
    public DefeatDelegate Defeat;
    public delegate void VictoryDelegate();
    public VictoryDelegate Victory;
    

    private Rigidbody _rg;
    private Material _defaultMaterial;
    private float _victoryTime = -1;

    public Material dissolveMaterial;
    public float dissolveAnimTime = 2;
    public float deathLevel = -10;
    // Start is called before the first frame update
    private void Start()
    {
        _rg = GetComponent<Rigidbody>();
        _defaultMaterial = GetComponent<Renderer>().sharedMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < deathLevel || 
            (_rg.velocity.magnitude < 1e-3 && _rg.useGravity)) 
        {
            Defeat();
        }

        if (_victoryTime > 0 && dissolveMaterial)
        {
            float n = (Time.time - _victoryTime) / dissolveAnimTime;
            dissolveMaterial.SetFloat(
                "_DissolveAmount", Mathf.Clamp(n, 0, 1)
                );
            if (n > 1) _victoryTime = 1;
        }
    }

    public void ResetState()
    {
        GetComponent<Renderer>().sharedMaterial = _defaultMaterial;
        _victoryTime = -1;
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
            _victoryTime = Time.time;
            if (dissolveMaterial) GetComponent<Renderer>().sharedMaterial = dissolveMaterial;
            Victory();
        }
    }
}
