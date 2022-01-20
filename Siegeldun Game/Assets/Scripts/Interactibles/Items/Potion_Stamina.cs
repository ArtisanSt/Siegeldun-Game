using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Stamina : Consumable
{
    // Called when instance is created
    void Awake()
    {
        ConsumableInitialization();
        itemName = "Potion_Stamina";
        consumableType = "Potion";
    }

    // Start is called before the first frame update
    void Start()
    {
        // ========================================= Potion Initialization =========================================
        maxQuantity = 64;
        curQuantity = 1;

        effectDict["Stamina"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 0f, // 0: false, 1: true
            ["effectParam"] = 0f, // How much it heals
            ["effectSpeed"] = 0f, // 0: instant, 1: overtime
            ["effectTimer"] = 0f, // Time of effect
        };

        PrefabsInit();
    }

    void Update()
    {
        InteractibleUpdate();
    }
}
