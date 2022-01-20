using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Health : Consumable
{
    void Awake()
    {
        ConsumableInitialization();
        itemName = "Potion_Health";
        consumableType = "Potion";
    }

    void Start()
    {
        // ========================================= Potion Initialization =========================================
        maxQuantity = 64;
        curQuantity = 1;
        effectDict["HP"] = new Dictionary<string, float>()
        {
            ["hasEffect"] = 1f, // 0: false, 1: true
            ["effectParam"] = 30f, // How much it heals
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