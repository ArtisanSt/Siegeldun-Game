using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : Breakables
{
    protected void EntityInitialization()
    {
        entityName = "Crate";

        maxHealth = 1;
        entityHp = maxHealth;

        dropChance = 1;
    }

    void Start()
    {
        BreakablesInitialization();
        EntityInitialization();
    }

    void Update()
    {
        if (!isAlive)
        {
            DeathInitialization();
            ClearInstance(2);
        }
    }
}