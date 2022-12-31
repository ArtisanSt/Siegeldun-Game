using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityProp : ScriptableObject
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [SerializeField] public string entityName;
    [SerializeField] public string entityTitle;
    [SerializeField] public string entityId;
    public int instanceID { get { return GetInstanceID(); } }

    public enum EntityType { Unit, Structure, Item }
    public abstract EntityType entityType { get; }

    public virtual string parentPath
    {
        get
        {
            return "Entities";
        }
    }
}
