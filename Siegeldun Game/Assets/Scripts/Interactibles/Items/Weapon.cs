using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponProperties
{
    // ========================================= Weapon Properties =========================================
    public string weaponType { get; protected set; } // Melee or Range

    public int tier { get; protected set; }

    public bool isBroken { get; protected set; }
    public bool doBreak { get; protected set; }
    public float durability { get; protected set; }

    public float damage { get; protected set; }
    public float attackRange { get; protected set; }
    public float attackSpeed { get; protected set; }
    public float attackDelay { get; protected set; }

    public int critChance { get; protected set; }
    public float critHit { get; protected set; }

    public float staminaCost { get; protected set; }

    public WeaponProperties(string weaponType, int tier)
    {
        this.weaponType = weaponType;
        this.tier = tier;
        isBroken = false;
    }

    public void SetValues(bool doBreak, float durability, float damage, float attackRange, float attackSpeed, float attackDelay, int critChance, float critHit, float staminaCost)
    {
        this.doBreak = doBreak;
        this.durability = durability;

        this.damage = damage;
        this.attackRange = attackRange;
        this.attackSpeed = attackSpeed;
        this.attackDelay = attackDelay;

        this.critChance = critChance;
        this.critHit = critHit;

        this.staminaCost = staminaCost;
    }
}

public class Weapon : Item
{
    // ========================================= Weapon Properties =========================================
    public WeaponProperties uniqueProp { get; protected set; }

    protected void WeaponInit()
    {
        itemType = "Weapon";
    }

    // ========================================= OVERWRITE METHODS =========================================
    public void OverwriteStats(int curQuantity, Dictionary<string, SelfEffectProperties> effectDict, WeaponProperties uniqueProp)
    {
        this.curQuantity = curQuantity;
        this.effectDict = effectDict;
        this.uniqueProp = uniqueProp;
    }
}
