using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityProp : ScriptableObject
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    [SerializeField] public string entityName;
    [SerializeField] public string entityTitle;
    [SerializeField] public string entityId;

    public virtual string parentPath
    {
        get
        {
            return "Entities";
        }
    }
}
