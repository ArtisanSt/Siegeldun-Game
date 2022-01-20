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

        PrefabsInit();
    }

    void Update()
    {
        if (!isAlive)
        {
            DeathInitialization();
            ClearInstance(2);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy" && col.gameObject.tag == "Player")
        {
            Physics2D.IgnoreCollision(col.otherCollider, col.collider);
        }
    }
}