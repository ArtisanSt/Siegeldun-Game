using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Entity : Base, IMoveable
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
    public override string instanceName => dataProp.name;
    public override string dirPath => dataProp.dirPath;


    // ============================== ENTITY PROPERTIES ==============================
    public EntityProp dataProp;

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
    public string DefaultsToJson()
    {
        string componentsJson = "";
        for (int i=0; i < iInits.Length; i++)
        {
            string componentJson = iInits[i].DefaultsToJson();
            int start = 1;
            componentsJson += componentJson.Substring(start, componentJson.Length - start - 1);
            if (i + 1 != iInits.Length - 1)
                componentsJson += ", ";
        }
        return $"{{ \"{dataProp.name}\" : {{ {componentsJson} }} }}";
    }
}
