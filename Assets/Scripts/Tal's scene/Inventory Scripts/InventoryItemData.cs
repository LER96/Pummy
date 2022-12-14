using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
public class InventoryItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject prefab;
    public Button button;
    public ActionSO action;

    public void Init(Action ac)
    {
        button.onClick.AddListener(() => ac.Invoke());
    }
}

public class ActionSO : ScriptableObject
{

}

