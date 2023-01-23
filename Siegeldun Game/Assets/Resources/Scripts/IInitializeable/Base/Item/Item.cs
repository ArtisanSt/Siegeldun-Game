using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Base, IJsonable
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {

    }

    protected override void Start()
    {
        base.Start();
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }

    // When turned disabled
    protected virtual void OnDisable()
    {

    }

    // When turned enabled
    protected virtual void OnEnable()
    {

    }

    // When scene ends
    protected virtual void OnDestroy()
    {

    }


    // ============================== BASE INHERITED PROPERTIES ==============================
    public override string dirPath => baseProp.dirPath;
    public override System.Type baseType => typeof(Item);


    // ============================== ENTITY PROPERTIES ==============================
    public ItemProp baseProp;

    public string itemNickname;

    public virtual void ComponentInit()
    {

    }


    /*// ============================== IMOVEABLE ==============================
    public Rigidbody2D rbody => GetComponent<Rigidbody2D>();*/


    // ============================== JSON ==============================
    public string componentName => baseProp.GetType().ToString();
    public virtual JsonData BasePropToBasePropJD()
    {
        baseProp.name = instanceName;
        return new JsonData(componentName, baseProp.ToJson());
    }

    public void SetBaseProp(string baseProp)
    {
        this.baseProp = JsonUtility.FromJson<ItemProp>(baseProp);
    }
}
