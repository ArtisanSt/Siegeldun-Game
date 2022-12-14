using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemProp : EntityProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public override EntityType entityType { get { return EntityType.Item; } }

    public new enum EntitySubType { Weapon }
    public abstract EntitySubType entitySubType { get; }


    public override string parentPath
    {
        get
        {
            return $"{base.parentPath}/Items";
        }
    }
}
