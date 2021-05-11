using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class GameState
{
    private bool[] _completedLevels;
    private bool[] _openedLevels;

    public GameState(int levelsNum)
    {
        _completedLevels = new bool[levelsNum + 1];
        _openedLevels = new bool[levelsNum + 1];
        _openedLevels[1] = true;
        _openedLevels[levelsNum] = true;
    }

    public bool IsCompleted(int l)
    {
        return _completedLevels[l];
    }
    
    public bool IsOpened(int l)
    {
        return _openedLevels[l];
    }
    

    public void Complete(int l)
    {
        _completedLevels[l] = true;
        if (l + 1 < _completedLevels.Length)
            _openedLevels[l + 1] = true;
    }
    
}

public class GameManager : MonoBehaviour
{
    private static int _currentLevel = 0;
    private static GameState _state;
    private static bool _isLoaded = false;

    private void SetupLevel()
    {
        LevelManager.Completed = CompleteLevel;
    }

    private void CompleteLevel()
    {
        _state.Complete(_currentLevel);
        Save();
    }
    public bool IsLevelCompleted(int l)
    {
        return _state.IsCompleted(l);
    }
    
    public bool IsLevelOpened(int l)
    {
        return _state.IsOpened(l);
    }

    
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
        _currentLevel = level;
    }

    public void LoadNextLevel()
    {
        _currentLevel += 1;
        if (_currentLevel >= SceneManager.sceneCountInBuildSettings - 1)
            _currentLevel = 0;
        SceneManager.LoadScene(_currentLevel);
    }

    public void LoadPlayground()
    {
        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    private void Load()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "gamestate";
        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            _state = (GameState)formatter.Deserialize(stream);
            stream.Close();
        }
        else
        {
            _state = new GameState(SceneManager.sceneCountInBuildSettings);
        }    
    }

    private void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "gamestate";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, _state);
        stream.Close();
    }

    public void EraseData()
    {
        _state = new GameState(SceneManager.sceneCountInBuildSettings);
        FileInfo fileInfo = new FileInfo(Application.persistentDataPath + "gamestate");
        if (fileInfo.Exists) fileInfo.Delete();
    } 

    private void Start()
    {
        if (!_isLoaded)
        {
            Load();
            _isLoaded = true;
        }
        SetupLevel();
    }
}
