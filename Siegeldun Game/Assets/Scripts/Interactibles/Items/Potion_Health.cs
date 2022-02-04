using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Health : Consumable
{
    // ========================================= Item Initialization =========================================
    protected void ItemInit()
    {
        objectName = "Potion_Health";
        itemName = "Health Potion";

        uniqueProp = new ConsumableProperties("Potion");

        maxQuantity = 64;
        curQuantity = 1;
    }

    protected void UniqueStatsInit()
    {
        effectDict["HP"].SetValues(true, 30f, "instant", 0f);
    }

    // ========================================= UNITY MAIN METHODS =========================================
    void Awake()
    {
        InteractInit();
        ConsumableInit();
        ItemInit();
        UniqueStatsInit();
    }

    void Start()
    {
        PrefabsInit();
    }
}