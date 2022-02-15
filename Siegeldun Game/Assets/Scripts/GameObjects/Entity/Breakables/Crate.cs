using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Breakables
{
    // ========================================= ITEM DROPS INITIALIZATION =========================================
    protected override void itemDropsInit()
    {
        switch (difficulty)
        {
            case 1:
                // itemDrops.Add();
                break;
            case 2:
                // itemDrops.Add();
                break;
            case 3:
                // itemDrops.Add();
                break;
        }
    }

    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
        itemDropsInit();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void OnEntityDestroy()
    {
        base.OnEntityDestroy();
    }

    // ========================================= ANIMATION METHODS =========================================
    protected override void AnimationState()
    {

    }
}