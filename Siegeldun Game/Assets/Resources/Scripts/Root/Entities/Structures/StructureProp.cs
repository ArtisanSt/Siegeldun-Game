using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Structure", menuName = "Entity/Structure")]
public class StructureProp : EntityProp
{
    public override string parentPath
    {
        get
        {
            return $"{base.parentPath}/Structures";
        }
    }
}
