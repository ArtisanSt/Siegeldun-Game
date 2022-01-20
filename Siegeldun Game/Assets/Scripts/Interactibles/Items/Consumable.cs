using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    // ========================================= Consumable Properties =========================================
    public string consumableType { get; protected set; } // Food or Potion

    protected void ConsumableInitialization()
    {
        ItemInit();
        itemType = "Consumable";
    }

}
