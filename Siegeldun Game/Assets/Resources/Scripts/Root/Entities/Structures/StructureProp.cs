using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure", menuName = "Entity/Structure")]
public class StructureProp : EntityProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public override EntityType entityType { get { return EntityType.Structure; } }

    public new enum EntitySubType { Structure }
    public EntitySubType entitySubType = EntitySubType.Structure;


    public override string parentPath
    {
        get
        {
            return $"{base.parentPath}/Structures";
        }
    }
}
