using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    public void HandlePickupItem()
    {
        InventorySystem.current.Add(referenceItem);
        Destroy(gameObject);
    }
}
