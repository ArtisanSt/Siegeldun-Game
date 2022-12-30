using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Human", menuName = "Entity/Unit/Human")]
public class HumanProp : UnitProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public override UnitType unitType { get { return UnitType.Human; } }


    public override string parentPath
    {
        get
        {
            return $"{base.parentPath}/Humans";
        }
    }
}
