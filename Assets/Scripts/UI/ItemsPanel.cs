using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemsPanel : MonoBehaviour
{
    [System.Serializable]
    public class Item
    {
        public GameObject movableObject;
        public int amount;
    }

    public GameObject button;
    public float step = 55;
    public Item[] items;

    private Dictionary<String, GameObject> _buttonMap;
    // Start is called before the first frame update

    public GameObject GetOrCreateButton(GameObject o)
    {
        if (!_buttonMap.ContainsKey(o.name))
            _buttonMap[o.name] = CreateButton(o, 0, transform.childCount, true);

        return _buttonMap[o.name];
        
    }

    private void DeleteButtonMapEntry(GameObject o)
    {
        _buttonMap.Remove(o.name);
    }

    public GameObject CreateButton(GameObject movableObject, int amount, int position, bool temporal)
    {
        GameObject newButton = Instantiate(button, gameObject.transform, false) as GameObject;
        newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, (position + 1) * -step) ;
        ItemCreator itemCreator = newButton.GetComponent<ItemCreator>();
        itemCreator.defaultAmount = amount;
        itemCreator.currentAmount = amount;
        itemCreator.prefab = movableObject;
        itemCreator.UpdateText();
        itemCreator.OnDelete = DeleteButtonMapEntry;
        
        itemCreator.temporal = temporal;
        if (!temporal) itemCreator.Create();
        return newButton;
    }
    void Start()
    {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
        Item item;
        _buttonMap = new Dictionary<String, GameObject>();
        for (int i = 0; i < items.Length; i++)
        {
            item = items[i];
            
            _buttonMap[item.movableObject.name] = 
                CreateButton(item.movableObject, item.amount, i, false);
        }
    }


    public void Reset()
    {
        foreach (Transform t in transform)
        {
            t.GetComponent<ItemCreator>().Reset();
        }
    }
}
