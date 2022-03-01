using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Katana : Weapon
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
    }

    public override bool ActiveEffectsCondition()
    {
        return true;
    }
}