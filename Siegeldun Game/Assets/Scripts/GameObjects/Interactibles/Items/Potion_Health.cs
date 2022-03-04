using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion_Health : Consumable
{
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
        return entityHolder.GetComponent<Entity>().curHp < entityHolder.GetComponent<Entity>().maxHp && base.ActiveEffectsCondition();
    }
}