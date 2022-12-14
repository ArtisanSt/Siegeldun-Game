using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]

[RequireComponent(typeof(MovementSystem))]
[RequireComponent(typeof(BattleSystem))]
[RequireComponent(typeof(StatusSystem))]
public abstract class Unit<N> : Entity<N>, IMoveable, IBattleable, IStatusable where N: UnitProp
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
        PropertyInit();
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
    // No need to call
    protected override void ComponentInit()
    {
        base.ComponentInit();
    }


    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public CapsuleCollider2D capColl { get; private set; }

    public MovementProp movementProp { get; private set; }
    public BattleProp battleProp { get; private set; }
    public StatusProp statusProp { get; private set; }

    public override void PropertyInit()
    {
        if (entityProp == null) return;
        capColl = GetComponent<CapsuleCollider2D>();

        movementProp = entityProp.movementProp;
        battleProp = entityProp.battleProp;
        statusProp = entityProp.statusProp;
    }


}