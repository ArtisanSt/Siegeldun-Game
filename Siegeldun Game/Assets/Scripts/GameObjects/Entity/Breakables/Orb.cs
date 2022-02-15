using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : Breakables
{
    // ========================================= Entity Initialization =========================================
    /*private bool _isInstanceLimited = false;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 0;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _entityName = "Orb";
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

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && col.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(col.otherCollider, col.collider);
        }
    }

    // ========================================= ANIMATION METHODS =========================================
    protected override void AnimationState()
    {

    }
}