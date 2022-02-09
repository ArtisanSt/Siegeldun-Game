using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Health : Consumable
{
    // ========================================= Item Initialization =========================================
    private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _objectName = "Potion_Health";
    public override string objectName { get { return _objectName; } }

    private string _itemName = "Health Potion";
    public override string itemName { get { return _itemName; } }

    protected override void UniqueStatsInit()
    {
        uniqueProp = new ConsumableProperties("Potion");
        effectDict["HP"].SetValues(true, 30f, "Instant", 0f, .1f);
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
        UseEffects("HP");
    }

    public override bool ActiveEffectsCondition()
    {
        return entityHolder.GetComponent<Entity>().curHp < entityHolder.GetComponent<Entity>().maxHp && base.ActiveEffectsCondition();
    }
}