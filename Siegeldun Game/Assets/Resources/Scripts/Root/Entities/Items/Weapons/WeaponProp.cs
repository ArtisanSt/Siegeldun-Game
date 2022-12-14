using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Entity/Item/Weapon")]
public class WeaponProp : ItemProp
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public override EntitySubType entitySubType { get { return EntitySubType.Weapon; } }


    public override string parentPath
    {
        get
        {
            return $"{base.parentPath}/Humans";
        }
    }
}
