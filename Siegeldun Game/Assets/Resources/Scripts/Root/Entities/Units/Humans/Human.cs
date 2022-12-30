using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Unit<HumanProp>
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
    public override void PropertyInit()
    {
        PropertyReconfig();
        base.PropertyInit();
        if (entityProp == null) return;
    }

    protected void PropertyReconfig()
    {
        if (entityProp != null) return;
        if (!EntityContainer.instance.Contains(gameObject.name, out int index, EntityContainer.instance.collections.Get("humans"))) return;

        this.entityProp = EntityContainer.instance.collections.humans[index].entityProp;
    }


    // ============================== INSTANTIATION ==============================
    public override void Duplicate(GameObject gameObject)
    {
        base.Duplicate(gameObject);
        Human human = gameObject.GetComponent<Human>();
    }
}
