using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour, IInitializer
{
    // ============================== UNITY METHODS ==============================
    protected virtual void Start()
    {
        InitInitializeables();
    }

    // ============================== BASE PROPERTIES ==============================
    public virtual string instanceID => $"{instanceName}({GetInstanceID()})";
    public abstract System.Type baseType { get; }
    public System.Type objectType => this.GetType();
    public virtual string instanceName => objectType.ToString();

    public virtual string dirPath { get; }


    // ============================== IINITIALIZER ==============================
    public IInitializeable[] iInits => GetComponents<IInitializeable>();
    public virtual void InitInitializeables()
    {
        for (int i = 0; i < iInits.Length; i++)
        {
            iInits[i].Init();
        }
    }
}
