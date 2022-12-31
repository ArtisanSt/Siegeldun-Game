using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureProp : EntityProp
{
    public StructureProp()
    {
        entityType = EntityType.Structure;
    }

    public string structureName { get { return entityName; } }
    public string structureTitle { get { return entityTitle; } }
    public string structureID { get { return entityID; } }

    // ============================== SHARED PROPERTIES ==============================
    public override string dirPath { get { return $"{base.dirPath}/Structures"; } }


    // ============================== MAIN PROPERTIES ==============================
}
