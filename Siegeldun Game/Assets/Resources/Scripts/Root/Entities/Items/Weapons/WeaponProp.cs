using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Entity/Item/Weapon")]
public class WeaponProp : ItemProp, IInventoryable
{
    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public override ItemType itemType { get { return ItemType.Weapon; } }


    public override string parentPath
    {
        get
        {
            return $"{base.parentPath}/Humans";
        }
    }
}
