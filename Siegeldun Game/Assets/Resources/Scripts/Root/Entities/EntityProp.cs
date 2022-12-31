using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EntityProp
{
    // ============================== MAIN PROPERTIES ==============================
    public string entityName;
    public string entityTitle;
    public string entityID;
    public int instanceCap;

    public enum EntityType { Unit, Structure, Item }
    public EntityType entityType { get; protected set; }

    public virtual string dirPath { get { return "Entities"; } }

}
