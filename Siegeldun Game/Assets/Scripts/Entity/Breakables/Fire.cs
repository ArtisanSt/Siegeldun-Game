using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : Breakables
{
    protected void EntityInitialization()
    {
        entityName = "Fire";

        maxHealth = 1;
        entityHp = maxHealth;

        doDrop = false;
        dropChance = 0;
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