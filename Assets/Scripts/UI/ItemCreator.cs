using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCreator : MonoBehaviour
{
    public GameObject prefab;
    public int defaultAmount;
    public int currentAmount;
    public bool temporal = false;
    private GameObject lastCreatedObject;


    public delegate void OnDeleteDelegate(GameObject o);

    public OnDeleteDelegate OnDelete;
    public void Create()
    {
        lastCreatedObject = Instantiate(
            prefab, 
            gameObject.transform.position, gameObject.transform.rotation, Camera.main.transform);
        lastCreatedObject.name = prefab.name;
        
        lastCreatedObject.transform.rotation *= Quaternion.Euler(0, 90, 0);
        lastCreatedObject.GetComponent<MovableObject>().createdFromPanel = true;
        lastCreatedObject.GetComponent<SimulationObject>().enabled = false;
        lastCreatedObject.SetActive(true);
        
    }

    public void UpdateText()
    {
        gameObject.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().SetText(currentAmount.ToString());
    }

    public void Reset()
    {
        Destroy(lastCreatedObject);
        if (!temporal)
        {
            Create();
            currentAmount = defaultAmount;
            UpdateText();
        }
        else
        {
            OnDelete?.Invoke(prefab);
            Destroy(gameObject);
        }

    }

    public void Increment()
    {
        if (currentAmount == 0) Create();
        currentAmount += 1;
        UpdateText();
    }
    
    public void DecrementAndCreate()
    {
        if (currentAmount > 0)
        {
            currentAmount -= 1;
            UpdateText();
        }
        if (currentAmount > 0)
        {
            Create();
        }
    }
    
    public void DecrementAndDestroy()
    {
        if (currentAmount > 0)
        {
            currentAmount -= 1;
            UpdateText();
        }
        if (currentAmount == 0)
            Destroy(lastCreatedObject);

    }
    public void Decrement()
    {
        if (currentAmount > 0)
        {
            currentAmount -= 1;
            UpdateText();
        }
    }
}
