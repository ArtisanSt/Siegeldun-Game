using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Key : Item
{
    // ========================================= Item Initialization =========================================
    /*private string _itemType = "Key";
    public override string itemType { get { return _itemType; } }*/

    // ========================================= OVERWRITE METHODS =========================================
    public void OverwriteStats(Key originalItem)
    {
        OverwriteStats(originalItem.curQuantity, originalItem.effects);
    }
}