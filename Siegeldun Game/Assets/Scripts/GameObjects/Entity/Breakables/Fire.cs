using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Breakables
{
    // ========================================= Entity Properties =========================================
    /*private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _entityName = "Fire";
    public override string entityName { get { return _entityName; } }
    public override string objectName { get { return _entityName; } }*/




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

    protected override void OnEntityDestroy()
    {
        base.OnEntityDestroy();
    }

    // ========================================= ANIMATION METHODS =========================================
    protected override void AnimationState()
    {

    }
}