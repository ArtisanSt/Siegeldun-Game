using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Stamina : Consumable
{
    // ========================================= Item Initialization =========================================
    /*private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "Potion_Stamina";
    public override string objectName { get { return _objectName; } }

    private string _itemName = "Stamina Potion";
    public override string itemName { get { return _itemName; } }
*/
    protected override void UniqueStatsInit()
    {

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
    }

    protected override void ActiveEffects()
    {
        UseEffects();
    }

    public override bool ActiveEffectsCondition()
    {
        return entityHolder.GetComponent<Entity>().curStam < entityHolder.GetComponent<Entity>().maxStam && base.ActiveEffectsCondition();
    }
}
