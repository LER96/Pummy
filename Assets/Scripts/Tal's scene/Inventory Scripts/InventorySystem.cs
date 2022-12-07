using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField] GameObject smartPhone;
    [SerializeField] GameObject playerReference;
    public List<InventoryItem> inventory;
    public static InventorySystem current;
    private ScreenShot picturesScript;

    private void Start()
    {
        picturesScript = playerReference.GetComponent<ScreenShot>();
        picturesScript.enabled = false;
    }

    private void Awake()
    {
        current = this;
        inventory = new List<InventoryItem>();
    }

    public void Add(InventoryItemData referenceData)
    {
        InventoryItem newItem = new InventoryItem(referenceData);
        inventory.Add(newItem);
        if (referenceData.itemName == "Phone")
        {
            EnablePhone();
        }
    }

    public void Remove(InventoryItem value)
    {
        inventory.Remove(value);
    }

    public void EnablePhone()
    {
        smartPhone.SetActive(true);
        picturesScript.enabled = true;
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
