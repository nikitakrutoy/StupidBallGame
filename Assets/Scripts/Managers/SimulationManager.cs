using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimulationManager : MonoBehaviour
{
    public delegate void ActivateAction();
    public static event ActivateAction OnActivate;
    
    public delegate void PauseAction();
    public static event PauseAction OnPause;
    
    public delegate void ResetAction();
    public static event ResetAction OnReset;

    public Button ResumeButton;
    public Button PauseButton;
    
    [HideInInspector]
    public float simulationStartTime = -1;
    // Start is called before the first frame update

    public void Activate()
    {
        if (simulationStartTime < 0)
        {
            simulationStartTime = Time.time;
        }

        if (OnActivate != null) OnActivate();
    }
    
    
    public void Pause()
    {
        if (OnPause != null) OnPause();
    }

    public void ResetObjectsState()
    {
        simulationStartTime = -1;
        if (OnReset != null) OnReset();
        PauseButton.gameObject.SetActive(false);
        ResumeButton.gameObject.SetActive(true);
    }
    
    // Update is called once per frame
}
