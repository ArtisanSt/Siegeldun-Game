using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : Entity
{
    protected void BreakablesInitialization()
    {
        entityType = "Breakables";
        deadBeings = true;
        ComponentInitialization();
    }

    protected void DeathInitialization()
    {

    }
}
