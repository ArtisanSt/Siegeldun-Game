using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item<N> : Entity<N> where N: ItemProp
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

    }

    // When turned disabled
    protected override void OnDisable()
    {
        base.OnDisable();

    }

    // When turned enabled
    protected override void OnEnable()
    {
        base.OnEnable();

    }

    // When scene ends
    protected override void OnDestroy()
    {
        base.OnDestroy();

    }


    // ============================== COMPONENTS ==============================
    protected override void ComponentInit()
    {
        base.ComponentInit();
    }


    // ============================== MAIN PROPERTIES AND METHODS ==============================
    //public MovementProp movementProp { get; private set; }
    public override void PropertyInit()
    {
        if (entityProp == null) return;

        //movementProp = entityProp.movementProp;
    }
}
