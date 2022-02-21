using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Breakables
{
    // ========================================= ITEM DROPS INITIALIZATION =========================================
    protected override void itemDropsInit()
    {

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

    // ========================================= ENTITY DEATH =========================================
    // Executes after death animation and instance clearing on memory
    protected override void Die()
    {
        base.Die();
        // Clear Inventory
    }

    // Executes right before entity to be destroyed
    protected override void OnEntityDestroy()
    {
        base.OnEntityDestroy();
    }

    // ========================================= ANIMATION METHODS =========================================
    protected override void AnimationState()
    {

    }
}