using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] Button itemButton;

    public void SetItem(InventoryItem item)
    {
       itemIcon = GetComponentInChildren<Image>();
       itemName = GetComponentInChildren<TextMeshProUGUI>();
       itemButton = GetComponentInChildren<Button>();

       itemIcon.sprite = item.data.icon;
       itemName.text = item.data.itemName;
       itemButton = item.data.button;
    }
}