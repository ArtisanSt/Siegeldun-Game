using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]

[RequireComponent(typeof(MovementSystem))]
[RequireComponent(typeof(BattleSystem))]
[RequireComponent(typeof(StatusSystem))]
public class Unit : Entity, IMoveable, IBattleable, IStatusable
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
    // No need to call
    protected override void ComponentInit()
    {
        base.ComponentInit();
        capColl = GetComponent<CapsuleCollider2D>();
    }


    // ============================== DATAPROP ==============================
    public new UnitProp dataProp { get; protected set; }
    public override void DataPropInit()
    {
        this.dataProp = DataProp.instance.Get<UnitProp>(entityName);
        if (this.dataProp == null) return;
        base.dataProp = this.dataProp;
    }


    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public CapsuleCollider2D capColl { get; private set; }

    public MovementProp movementProp { get; private set; }
    public BattleProp battleProp { get; private set; }
    public StatusProp statusProp { get; private set; }

    public MovementSystem movementSystem { get; private set; }
    public BattleSystem battleSystem { get; private set; }
    public StatusSystem statusSystem { get; private set; }

    public override void PropertyInit()
    {
        if (dataProp == null) return;

        Debug.Log(dataProp.entityName);
        Debug.Log(dataProp.entityID);

        movementProp = dataProp.movementProp;
        battleProp = dataProp.battleProp;
        statusProp = dataProp.statusProp;

        movementSystem = GetComponent<MovementSystem>();
        battleSystem = GetComponent<BattleSystem>();
        statusSystem = GetComponent<StatusSystem>();

        movementSystem.Init(movementProp);
        battleSystem.Init(battleProp);
        statusSystem.Init(statusProp);
    }


    /*// ============================== INSTANTIATION ==============================
    public override void Duplicate(GameObject gameObject)
    {
        base.Duplicate(gameObject);
        Unit<TUnitProp> unit = gameObject.GetComponent<Unit<TUnitProp>>();
    }*/
}
