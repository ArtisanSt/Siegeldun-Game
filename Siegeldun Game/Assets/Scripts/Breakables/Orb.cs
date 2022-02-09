using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : Breakables
{
    protected void EntityInitialization()
    {
        entityName = "Orb";

        maxHealth = 1;
        entityHp = maxHealth;

        doDrop = true;
        dropChance = 1;
    }

    void Start()
    {
        BreakablesInitialization();
        EntityInitialization();

        PrefabsInit();
    }

    void Update()
    {
        if (!isAlive)
        {
            DeathInitialization();
            ClearInstance(0);
        }
    }
}
