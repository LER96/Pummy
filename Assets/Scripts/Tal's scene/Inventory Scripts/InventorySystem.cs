using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public List<InventoryItem> inventory;
    public static InventorySystem current;

    private void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
    }

    public void Add(InventoryItemData referenceData)
    {        
        InventoryItem newItem = new InventoryItem(referenceData);
        inventory.Add(newItem);
    }

    public void Remove(InventoryItem value)
    {        
        inventory.Remove(value);
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
