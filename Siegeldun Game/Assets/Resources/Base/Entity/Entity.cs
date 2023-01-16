using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : Base
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        ComponentInit();
        Debug.Log(DefaultsToJson());
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


    // ============================== JSON ==============================
    public string DefaultsToJson()
    {
        string componentsJson = "";
        IInitializeable[] iInit = GetComponents<IInitializeable>();
        for (int i=0; i < iInit.Length; i++)
        {
            string componentJson = iInit[i].DefaultsToJson();
            int start = 1;
            componentsJson += componentJson.Substring(start, componentJson.Length - start - 1);
            if (i + 1 != iInit.Length - 1)
                componentsJson += ", ";
        }
        return $"{{ \"{dataProp.name}\" : {{ {componentsJson} }} }}";
    }
}
