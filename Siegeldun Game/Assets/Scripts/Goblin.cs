using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : EnemyAIv2
{

    // Enemy NPC Properties Initialization
    protected void GoblinInitialization()
    {
        entityName = "Goblin";
        EntityStatsInitialization(entityName);

        // Battle Initialization
        entityWeapon = 0; // Pseudo Weapon Index
        entityDamage = 20f;
        attackSpeed = .85f;
        attackDelay = 2f;
        lastAttack = 0f;
        attackRange = 0.3f; // Pseudo Weapon Range
        EqWeaponStamCost = 0f;
        weaponDrag = 0f; // Pseudo Weapon Drag
        weaponKbForce = .8f; // Pseudo Weapon Knockback Force
        attacking = false;
        kbDir = 0;
        kTick = 0f;
        kbHorDisplacement = .8f;
        kbVerDisplacement = 0f;

        // HP Initialization
        maxHealth = 100f;
        entityHp = maxHealth;
        hpRegenAllowed = true;
        healthRegen = healthRegenScaling[idxDiff];
        regenDelay = 3f;
        hpRegenTimer = 0f;

        // Movement Initialization
        isGrounded = false;
        mvSpeed = 5f;
        jumpForce = 15.5f;
        rBody.gravityScale = 6;

        // Pathfinding Initialization
        sleepTime = 10f;
        doSleep = (Random.Range(0,2) == 0) ? true: false;
        isAwake = (doSleep) ? ((Random.Range(0, 2) == 0) ? true : false) : true;

        wanderMvSpeed = 3f;

        forgivenessTime = 3f;
        triggerDistance = 5f;
        jumpEnabled = true;

        // Item Drop Parameters
        dropChance = 3;
    }

    // Start is called before the first frame update
    void Start()
    {
        GoblinInitialization();
        EnemyNPCStart();
    }

    // Update is called once per frame
    void Update()
    {
        EnemyNPCUpdate();
        DropItem();
    }
    // Updates Every Physics Frame
    void FixedUpdate()
    {
        EnemyNPCFixedUpdate();
    }
}
