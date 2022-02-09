using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableProperties
{
    // ========================================= Consumable Properties =========================================
    public string consumableType { get; protected set; } // Food or Potion

    public ConsumableProperties(string consumableType)
    {
        this.consumableType = consumableType;
    }

    public void SetValues()
    {
    }
}

public abstract class Consumable : Item
{
    // ========================================= Weapon Properties =========================================
    private string _itemType = "Consumable";
    public override string itemType { get { return _itemType; } }

    public ConsumableProperties uniqueProp { get; protected set; }

    public override bool OnUse(bool isCrit)
    {
        bool outcome = base.OnUse(isCrit);
        return outcome;
    }

    // ========================================= OVERWRITE METHODS =========================================
    public void OverwriteStats(Consumable originalItem)
    {
        curQuantity = originalItem.curQuantity;
        effectDict = originalItem.effectDict;
        uniqueProp = originalItem.uniqueProp;
    }
}
