using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item<TItemProp> : Entity<TItemProp>
    where TItemProp: ItemProp
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
    protected override void ComponentInit()
    {
        base.ComponentInit();
    }


    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public override void PropertyInit()
    {
        if (entityProp == null) return;

        amount = new Vector2Int(entityProp.maxAmount, entityProp.maxAmount);
    }

    public Vector2Int amount; // curAmount, maxAmount



    // ============================== INSTANTIATION ==============================
    public void Duplicate(TItemProp entityProp, Vector2Int amount)
    {
        this.entityProp = entityProp;
        this.amount = amount;
    }


    // ============================== INSTANTIATION ==============================
    public override void Duplicate(GameObject gameObject)
    {
        base.Duplicate(gameObject);
        Item<TItemProp> item = gameObject.GetComponent<Item<TItemProp>>();
        Duplicate(item.entityProp, item.amount);
    }
}
