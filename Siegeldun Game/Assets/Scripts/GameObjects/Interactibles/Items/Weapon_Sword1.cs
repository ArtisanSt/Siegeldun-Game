using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Sword1 : Weapon
{
    // ========================================= Item Initialization =========================================
    private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "Weapon_Sword1";
    public override string objectName { get { return _objectName; } }

    private string _itemName = "Sword1";
    public override string itemName { get { return _itemName; } }

    protected override void UniqueStatsInit()
    {
        maxQuantity = 1;
        curQuantity = 1;

        uniqueProp = new WeaponProperties("Melee", 0, true);
        uniqueProp.SetValues(30f, .3f, .5f, .3f, 100, 10f, 10f, .3f, 100);
    }

    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
    }

    protected override void PassiveEffects(string state)
    {
        if (state == "Select")
        {

        }
        else if (state == "Inventory")
        {

        }
        else if (state == "Equipped")
        {

        }
    }

    protected override void ActiveEffects()
    {
    }

    public override bool ActiveEffectsCondition()
    {
        return true;
    }
}