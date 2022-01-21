using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword1 : Weapon
{
    // ========================================= Item Initialization =========================================
    protected void ItemInit()
    {
        objectName = "Weapon_Sword1";
        itemName = "Sword1";

        uniqueProp = new WeaponProperties("Melee", 0);

        maxQuantity = 1;
        curQuantity = 1;
    }

    protected void UniqueStatsInit()
    {
        uniqueProp.SetValues(true, 100f, 30f, 0.3f, 1, 0.3f, 100, .3f, 10f);
    }

    // ========================================= UNITY MAIN METHODS =========================================
    void Awake()
    {
        InteractInit();
        WeaponInit();
        ItemInit();
        UniqueStatsInit();
    }

    void Start()
    {
        PrefabsInit();
    }
}