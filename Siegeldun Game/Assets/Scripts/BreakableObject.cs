using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObject : Entity
{
    protected void BreakableObjectInit()
    {
        isBreakable = true;
        entityName = "Object";

        maxHealth = 1;
        entityHp = maxHealth;
    }

    void Start()
    {
        rBody = transform.GetComponent<Rigidbody2D>();
        BreakableObjectInit();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}