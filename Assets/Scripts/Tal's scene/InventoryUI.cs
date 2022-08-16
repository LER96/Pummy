using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public GameObject slotPrefab;
    public GameObject inventoryMenu;
    private bool inventoryOpen = false;

    private void Start()
    {
        inventoryOpen = false;
        inventoryMenu.SetActive(false);
    }
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I) && !inventoryOpen)
        {
            inventoryMenu.SetActive(true);
            OnUpdateInventory();
            inventoryOpen = true;
        }

        if (Input.GetKeyDown(KeyCode.I) && inventoryOpen)
        {
            inventoryMenu.SetActive(false);
            inventoryOpen = false;
        }
    }

    private void OnUpdateInventory()
    {
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
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
        GameObject obj = Instantiate(slotPrefab);
        obj.transform.SetParent(transform, false);

        ItemSlot slot = obj.GetComponent<ItemSlot>();
        slot.SetItem(item);
    }
}
