using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable_Crate : Entity
{
    [SerializeField] GameObject itemPrefab;
    private int dropChance;

    protected void BreakableObjectInit()
    {
        entityName = "Crate";
        isBreakable = true;

        dropChance = 1;

        maxHealth = 1;
        entityHp = maxHealth;
    }

    void Start()
    {
        ComponentInitialization();
        BreakableObjectInit();
    }

    void Update()
    {
        if(drop == true)
        {
            Drop(itemPrefab, dropChance, 0);
            drop = false;
        }
    }
}