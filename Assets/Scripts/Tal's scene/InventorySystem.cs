using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    private Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;
    public List<InventoryItem> inventory;
    public static InventorySystem current;

    private void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
    }

    public InventoryItem Get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    public void Add(InventoryItemData referenceData)
    {        
        InventoryItem newItem = new InventoryItem(referenceData);
        inventory.Add(newItem);
        m_itemDictionary.Add(referenceData, newItem);        
    }

    public void Remove(InventoryItemData referenceData, InventoryItem value)
    {        
        inventory.Remove(value);
        m_itemDictionary.Remove(referenceData);                   
    }
}

[System.Serializable]
public class InventoryItem
{
    public InventoryItemData data;

    public InventoryItem(InventoryItemData source)
    {
        data = source;
    }
}
