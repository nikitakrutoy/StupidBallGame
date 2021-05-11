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

    public float deathLevel = -10;
    public float defeatDelay = 1;
    // Start is called before the first frame update
    private void Start()
    {
        _rg = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < deathLevel || 
            (_rg.velocity.magnitude < 1e-3 && _rg.useGravity)) 
        {
            Defeat();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
            Victory();
    }
}
