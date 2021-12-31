using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword1 : Weapon
{
    void Awake()
    {
        WeaponInitialization();
        itemName = "Weapon_Sword1";
        weaponType = "Melee";
    }

    void Start()
    {
        // ========================================= Sword Initialization =========================================
        maxQuantity = 1;
        curQuantity = 1;

        damage = 30f; // Pseudo Damage
        attackRange = 0.3f; // Pseudo Range
        attackSpeed = 1;
        staminaCost = 10f;
        durability = 100f;
        critChance = 100; // 1 out of 100
        critHit = .3f;
        tier = 0; // Pseudo Tier

        ItemFinalization();
    }
}