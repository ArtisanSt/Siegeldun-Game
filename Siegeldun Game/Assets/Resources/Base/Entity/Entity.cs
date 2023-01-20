using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public abstract class Entity : Base, IMoveable, IJsonable
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        ComponentInit();
        IInitializeables();
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
    public override System.Type baseType => typeof(Entity);


    // ============================== ENTITY PROPERTIES ==============================
    public EntityProp baseProp;

    public string entityNickname; 

    public void ComponentInit()
    {

    }

    public IInitializeable[] iInits => GetComponents<IInitializeable>();
    public void IInitializeables()
    {
        for (int i=0; i<iInits.Length; i++)
        {
            iInits[i].Init();
        }
    }


    // ============================== IMOVEABLE ==============================
    public Rigidbody2D rbody => GetComponent<Rigidbody2D>();


    // ============================== JSON ==============================
    public virtual JsonData BasePropToJson()
    {
        baseProp.name = instanceName;
        return new JsonData(baseProp.GetType().ToString(), baseProp.ToJson());
    }

    public void SetBaseProp(string baseProp)
    {
        this.baseProp = JsonUtility.FromJson<EntityProp>(baseProp);
    }
}
