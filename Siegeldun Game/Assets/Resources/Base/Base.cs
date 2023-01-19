using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base : MonoBehaviour
{
    // ============================== BASE PROPERTIES ==============================
    public abstract string instanceName { get; }
    public virtual string instanceID => $"{instanceName}({GetInstanceID()})";
    public abstract System.Type objectType { get; }

    public virtual string dirPath { get; }
}
