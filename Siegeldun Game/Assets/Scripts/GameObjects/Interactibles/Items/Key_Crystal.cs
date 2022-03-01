using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_Crystal : Key
{


    protected override void UniqueStatsInit()
    {

    }

    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
        if (disableInteract && isInteractible) ToggleInteractible();
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
