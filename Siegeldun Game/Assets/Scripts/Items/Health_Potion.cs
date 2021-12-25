using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Potion : Consumable
{
    void Start()
    {
        ConsumableInitialization();
        // ========================================= Potion Initialization =========================================
        itemName = "Health_Potion";
        maxQuantity = 64;

        consumableType = "Potion";
        effectDict["HP"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 1f, // 0: false, 1: true
            ["effectParam"] = 30f, // How much it heals
            ["effectSpeed"] = 0f, // 0: instant, 1: overtime
            ["effectTimer"] = 0f, // Time of effect
        };
    }
}