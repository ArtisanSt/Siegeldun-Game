using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitProp : EntityProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public override EntityType entityType { get { return EntityType.Unit; } }

    public new enum EntitySubType { Human }
    public abstract EntitySubType entitySubType { get; }


    public override string parentPath
    {
        get
        {
            return $"{base.parentPath}/Units";
        }
    }

    public EffectProp effectProp;
    public MovementProp movementProp;
    public BattleProp battleProp;
    public StatusProp statusProp;
}
