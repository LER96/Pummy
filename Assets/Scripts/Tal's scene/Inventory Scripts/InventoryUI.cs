using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform itemContent;
    public GameObject inventoryMenu;
    private bool _inventoryOpen = false;

    private void Start()
    {
       _inventoryOpen = false;
       inventoryMenu.SetActive(false);
    }

    void Update()
    {
       if (Input.GetKeyDown(KeyCode.I) && !_inventoryOpen)
       {
           inventoryMenu.SetActive(true);
           OnUpdateInventory();
           Cursor.lockState = CursorLockMode.Confined;
           _inventoryOpen = true;
       }
       else if (Input.GetKeyDown(KeyCode.I) && _inventoryOpen)
       {
           inventoryMenu.SetActive(false);
           Cursor.lockState = CursorLockMode.Locked;
           _inventoryOpen = false;
       }
    }

    public void OnUpdateInventory()
    {
        foreach (Transform item in itemContent)
        {
            Destroy(item.gameObject);
        }

        DrawInventory();
    }

    public void DrawInventory()
    {
        foreach (InventoryItem item in InventorySystem.current.inventory)
        {
            AddInventorySlot(item);
        }
    }

    public void AddInventorySlot(InventoryItem item)
    {
        GameObject obj = Instantiate(slotPrefab, itemContent);

        ItemSlot slot = obj.GetComponent<ItemSlot>();
        slot.SetItem(item);
    }
}
