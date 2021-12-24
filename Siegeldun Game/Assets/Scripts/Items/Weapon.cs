using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    // ========================================= Weapon Properties =========================================
    protected float damage;
    protected float attackRange;
    protected float attackSpeed;
    protected float staminaCost;
    protected float durability;
    protected int critChance;
    protected float critHit;
    protected int tier;
    protected string weaponType; // Melee or Range

    protected void WeaponInitialization()
    {
        ItemEffectInitialization();
        itemType = "Weapon";
    }
}
