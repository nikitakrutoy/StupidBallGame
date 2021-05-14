using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public delegate void CompletedDelegate();
    public static CompletedDelegate Completed;
    

    public float defeatDelay = 2;

    public ResultPanel victoryPanel;
    public ResultPanel defeatPanel;

    private CameraController[] _cameraControllers;

    public MovableObjectManager movableObjectManager;
    public SimulationManager simulationManager;    
    public StupidBall stupidBall;
    public FinishPoint finishPoint;

    private bool _isFinished = false;
    private bool _isFailed = false;
    // Start is called before the first frame update
    private void Start()
    {
        stupidBall.Defeat = Defeat;
        stupidBall.Victory = Victory;
        _cameraControllers = Camera.main.GetComponents<CameraController>();
        _cameraControllers = _cameraControllers.Where(c => c.enabled).ToArray();
    }
    
    public void Victory()
    {
        if (!_isFailed)
        {
            movableObjectManager.RestrictModifications = true;
            victoryPanel.Play();
            Completed?.Invoke();
            _isFinished = true;
        }
    }

    public void Defeat()
    {
        if (Time.time - simulationManager.simulationStartTime > defeatDelay && !_isFinished)
        {
            movableObjectManager.RestrictModifications = true;
            defeatPanel.Play();
            _isFailed = true;
        }
    }

    public void ResetPosition()
    {
        movableObjectManager.ResetObjectsState();
    }

    public void ResetSimulation()
    {
        simulationManager.ResetObjectsState();
        stupidBall.ResetState();
        _isFailed = false;
        _isFinished = false;
    }

    private void DisableCameraController()
    {
        foreach (var controller in _cameraControllers)
        {
            controller.enabled = false;
        }
    }

    private void EnableCameraController()
    {
        foreach (var controller in _cameraControllers)
        {
            controller.enabled = true;
        }
    }

    void OnEnable()
    {
        SimulationManager.OnActivate += movableObjectManager.DisableInput;
        SimulationManager.OnReset += movableObjectManager.EnableInput;
        MovableObjectController.OnActivate += DisableCameraController;
        MovableObjectController.OnDeactivate += EnableCameraController;
    }


    void OnDisable()
    {
        SimulationManager.OnActivate += movableObjectManager.DisableInput;
        SimulationManager.OnReset += movableObjectManager.EnableInput;
        MovableObjectController.OnActivate -= DisableCameraController;
        MovableObjectController.OnDeactivate -= EnableCameraController;
    }

    // Update is called once per frame
}