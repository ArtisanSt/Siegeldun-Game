using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponProperties
{
    // ========================================= Weapon Properties =========================================
    public string weaponType; // Melee or Range

    public int tier;

    public bool isBroken;
    public bool doBreak;
    public int durability;
    private int _maxDur;

    public float wpnDamage;
    public float wpnAtkRange;
    public float wpnAtkSpeed;
    public float wpnAtkDelay;

    public int wpnCritChance;
    public float wpnAtkCrit;

    public float wpnStamCost;

    public float wpnKbForce;

    // Default values for Hand
    public WeaponProperties(string weaponType = "Melee", int tier = 0, bool doBreak = false)
    {
        this.weaponType = weaponType;
        this.tier = tier;
        this.doBreak = doBreak;
        isBroken = false;
    }

    public void SetValues(float wpnDamage, float wpnAtkRange, float wpnAtkSpeed, float wpnAtkDelay, int wpnCritChance, float wpnAtkCrit, float wpnStamCost, float wpnKbForce, int durability = 0)
    {
        this.wpnDamage = wpnDamage;
        this.wpnAtkRange = wpnAtkRange;
        this.wpnAtkSpeed = wpnAtkSpeed;
        this.wpnAtkDelay = wpnAtkDelay;

        this.wpnCritChance = wpnCritChance;
        this.wpnAtkCrit = wpnAtkCrit;

        this.wpnStamCost = wpnStamCost;

        this.wpnKbForce = wpnKbForce;

        this.durability = durability;
        this._maxDur = durability;
    }

    public void ChangeValues(float wpnDamage = 0, float wpnAtkRange = 0, float wpnAtkSpeed = 0, float wpnAtkDelay = 0, int wpnCritChance = 0, float wpnAtkCrit = 0, float wpnStamCost = 0, float wpnKbForce = 0, int _maxDur = 0)
    {
        this.wpnDamage = (wpnDamage != 0) ? wpnDamage : this.wpnDamage ;
        this.wpnAtkRange = (wpnAtkRange != 0) ? wpnAtkRange : this.wpnAtkRange;
        this.wpnAtkSpeed = (wpnAtkSpeed != 0) ? wpnAtkSpeed : this.wpnAtkSpeed;
        this.wpnAtkDelay = (wpnAtkDelay != 0) ? wpnAtkDelay : this.wpnAtkDelay;

        this.wpnCritChance = (wpnCritChance != 0) ? wpnCritChance : this.wpnCritChance;
        this.wpnAtkCrit = (wpnAtkCrit != 0) ? wpnAtkCrit : this.wpnAtkCrit;

        this.wpnStamCost = (wpnStamCost != 0) ? wpnStamCost : this.wpnStamCost;

        this.wpnKbForce = (wpnKbForce != 0) ? wpnKbForce : this.wpnKbForce;

        this._maxDur = (_maxDur != 0) ? _maxDur : this._maxDur;
    }

    public void OnUse(bool isCrit)
    {
        if (doBreak && !isBroken)
        {
            // Decrease the durability when used
            if (durability > 0) durability -= (isCrit) ? 2 : 1;

            // Check if broken
            if (durability <= 0) isBroken = true;
        }
    }

    public bool OnFix(int healAmount = 0) // success or fail
    {
        bool isFixed = false;
        if (doBreak && !isBroken)
        {
            if (durability < _maxDur)
            {
                durability += healAmount;
                isFixed = true;
            }

            if (durability >= _maxDur) durability = _maxDur;
        }
        return isFixed;
    }
}

public abstract class Weapon : Item
{
    // ========================================= Weapon Properties =========================================
    private string _itemType = "Weapon";
    public override string itemType { get { return _itemType; } }

    public WeaponProperties uniqueProp { get; protected set; }

    public override bool OnUse(bool isCrit)
    {
        bool outcome = base.OnUse(isCrit);
        if (outcome) uniqueProp.OnUse(isCrit);
        return outcome;
    }

    // ========================================= OVERWRITE METHODS =========================================
    public void OverwriteStats(Weapon originalItem)
    {
        curQuantity = originalItem.curQuantity;
        effectDict = originalItem.effectDict;
        uniqueProp = originalItem.uniqueProp;
    }
}
