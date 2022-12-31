using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitProp : EntityProp
{
    public UnitProp()
    {
        entityType = EntityType.Unit;
    }

    public string unitName { get { return entityName; } }
    public string unitTitle { get { return entityTitle; } }
    public string unitID { get { return entityID; } }

    // ============================== SHARED PROPERTIES ==============================
    public override string dirPath { get { return $"{base.dirPath}/Units/{unitType.ToString()}"; } }


    // ============================== MAIN PROPERTIES ==============================
    public enum UnitType { Human }
    public UnitType unitType;

    public EffectProp effectProp;
    public MovementProp movementProp;
    public BattleProp battleProp;
    public StatusProp statusProp;
}
