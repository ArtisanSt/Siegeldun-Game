using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Crystal : Key
{
    // ========================================= Item Initialization =========================================
    /*private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "Key_Crystal";
    public override string objectName { get { return _objectName; } }

    private string _itemName = "Crystal";
    public override string itemName { get { return _itemName; } }*/

    protected override void UniqueStatsInit()
    {

    }

    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
    }

    protected override void PassiveEffects(string state)
    {

    }

    protected override void ActiveEffects()
    {

    }

    public override bool ActiveEffectsCondition()
    {
        return base.ActiveEffectsCondition();
    }

}
