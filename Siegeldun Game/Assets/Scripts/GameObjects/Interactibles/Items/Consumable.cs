using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConsumableProperties
{
    // ========================================= Consumable Properties =========================================
    public enum ConsumableType { Potion, Food }
    public ConsumableType consumableType; // Food or Potion

    public ConsumableProperties(ConsumableType consumableType)
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
    /*private string _itemType = "Consumable";
    public override string itemType { get { return _itemType; } }*/

    [SerializeField] public ConsumableProperties uniqueProp;

    public override bool OnUse(bool isCrit)
    {
        bool outcome = base.OnUse(isCrit);
        return outcome;
    }

    // ========================================= OVERWRITE METHODS =========================================
    public void OverwriteStats(Consumable originalItem)
    {
        OverwriteStats(originalItem.curQuantity, originalItem.effects);
        uniqueProp = originalItem.uniqueProp;
    }
}
