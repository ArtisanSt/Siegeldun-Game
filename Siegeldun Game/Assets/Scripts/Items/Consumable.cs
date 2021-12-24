using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : Item
{
    // ========================================= Consumable Properties =========================================
    protected string consumableType; // Food or Potion

    protected void ConsumableInitialization()
    {
        ItemEffectInitialization();
        itemType = "Consumable";
    }

}
