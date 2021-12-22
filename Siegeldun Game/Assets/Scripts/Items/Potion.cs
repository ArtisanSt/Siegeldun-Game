using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Consumable
{
    void Start()
    {
        // ========================================= James's Intialization =========================================
        itemName = "Health_Potion";
        itemType = itemTypes[0];


        // ========================================= Potion Initialization =========================================
        type = "Potion";
    }
}