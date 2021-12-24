using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Stamina : Consumable
{
    // Start is called before the first frame update
    void Start()
    {
        ConsumableInitialization();
        // ========================================= Potion Initialization =========================================
        itemName = "Stamina_Potion";
        maxQuantity = 64;

        consumableType = "Potion";
        effectDict["Stamina"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
            ["effectParam"] = 0f, // How much it heals
            ["effectSpeed"] = 0f, // 0: instant, 1: overtime
            ["effectTimer"] = 0f, // Time of effect
        };
    }
}
