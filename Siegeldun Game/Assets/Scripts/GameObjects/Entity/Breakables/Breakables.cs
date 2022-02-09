using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Breakables: Entity
{
    // ========================================= BEINGS PROPERTIY SCALING =========================================
    private string _entityType = "Breakables";
    public override string entityType { get { return _entityType; } }




    // ========================================= BATTLING METHODS =========================================
    protected override void Attack()
    {

    }
}
