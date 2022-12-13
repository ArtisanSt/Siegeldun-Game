using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Entity/Item")]
public class ItemProp : EntityProp
{
    public override string parentPath
    {
        get
        {
            return $"{base.parentPath}/Items";
        }
    }
}
