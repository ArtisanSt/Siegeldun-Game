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

public class Consumable : Item
{
    // ========================================= Weapon Properties =========================================
    public ConsumableProperties uniqueProp;

    protected void ConsumableInit()
    {
        itemType = "Consumable";
    }

    // ========================================= OVERWRITE METHODS =========================================
    public void OverwriteStats(int curQuantity, Dictionary<string, SelfEffectProperties> effectDict, ConsumableProperties uniqueProp)
    {
        this.curQuantity = curQuantity;
        this.effectDict = effectDict;
        this.uniqueProp = uniqueProp;
    }
}
