using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : NPC
{
    // ========================================= ITEM DROPS INITIALIZATION =========================================
    protected override void itemDropsInit()
    {

    }

    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
        NPCInit();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    // Updates Every Physics Frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate(); // Updates the Animation of the Entity
    }

    // ========================================= NPC DEATH =========================================
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
}
