using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="Item/Create New Item")]
public class item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}
