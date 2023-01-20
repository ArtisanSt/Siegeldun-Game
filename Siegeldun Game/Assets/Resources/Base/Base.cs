using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
    // ============================== BASE PROPERTIES ==============================
    public virtual string instanceID => $"{instanceName}({GetInstanceID()})";
    public abstract System.Type baseType { get; }
    public System.Type objectType => this.GetType();
    public virtual string instanceName => objectType.ToString();

    public virtual string dirPath { get; }
}
