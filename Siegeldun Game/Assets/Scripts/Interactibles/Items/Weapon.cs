using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    // ========================================= Weapon Properties =========================================
    public float damage { get; protected set; }
    public float attackRange { get; protected set; }
    public float attackSpeed { get; protected set; }
    public float attackDelay { get; protected set; }
    public float staminaCost { get; protected set; }
    public float durability { get; protected set; }
    public int critChance { get; protected set; }
    public float critHit { get; protected set; }
    public int tier { get; protected set; }
    public bool doBreak { get; protected set; }

    public string weaponType { get; protected set; } // Melee or Range

    protected void WeaponInitialization()
    {
        ItemInit();
        itemType = "Weapon";
    }
}
