using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    void Start()
    {
        // ========================================= James's Intialization =========================================
        itemName = "Sword";
        itemType = itemTypes[1];


        // ========================================= Sword Initialization =========================================
        itemName = "Sword";
        damage = 30f; // Pseudo Damage
        attackRange = 0.3f; // Pseudo Range
        attackSpeed = 1;
        staminaCost = 10f;
        durability = 100f;
        critChance = 100; // 1 out of 100
        critHit = .3f;
        tier = 0; // Pseudo Tier
        type = "Melee";
    }
}