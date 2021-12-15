using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_Fire : Entity
{
    protected void BreakableObjectInit()
    {
        entityName = "Fire";
        isBreakable = true;

        maxHealth = 1;
        entityHp = maxHealth;
    }

    void Start()
    {
        ComponentInitialization();
        BreakableObjectInit();
    }
}