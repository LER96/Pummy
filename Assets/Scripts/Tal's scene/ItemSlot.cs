using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;

    public void SetItem(InventoryItem item)
    {
        itemIcon.sprite = item.data.icon;
        itemName.text = item.data.itemName;
    }
}
