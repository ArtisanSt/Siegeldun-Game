using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : NPC
{
    // ========================================= Entity Properties =========================================
    private bool _isInstanceLimited = true;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 100;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _entityName = "Goblin";
    public override string entityName { get { return _entityName; } }
    public override string objectName { get { return _entityName; } }




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
        defaultPower.SetValues(15f, .3f, 1f, 1f, 1, 0, 0, 0);
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
}
