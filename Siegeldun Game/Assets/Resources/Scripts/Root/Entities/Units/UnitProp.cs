using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Unit", menuName = "Entity/Unit")]
public class UnitProp : EntityProp
{
    public override string parentPath
    {
        get
        {
            return $"{base.parentPath}/Units";
        }
    }
}
