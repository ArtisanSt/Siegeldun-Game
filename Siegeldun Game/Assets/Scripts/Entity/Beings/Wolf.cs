using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : EnemyAI
{
    [SerializeField] protected static List<int> GoblinIDs = new List<int>();

    // Enemy NPC Properties Initialization
    protected void EntityInitilization()
    {
        entityName = "Wolf";
        defaultFacing = -1;
        EntityStatsInitialization(entityName);

        // Battle Initialization
        entityWeapon = 0; // Pseudo Weapon Index
        entityDamage = 20f;
        attackSpeed = .3f;
        attackDelay = 2f;
        lastAttack = 0f;
        attackRange = 0.3f; // Pseudo Weapon Range
        EqWeaponStamCost = 0f;
        weaponDrag = 0f; // Pseudo Weapon Drag
        weaponKbForce = .8f; // Pseudo Weapon Knockback Force
        isAttacking = false;
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
        doSleep = (Random.Range(0, 2) == 0) ? true : false;
        isAwake = (doSleep) ? ((Random.Range(0, 2) == 0) ? true : false) : true;
        backOffDistance = Mathf.Abs(rBody.position.x - attackPoint.position.x); // Prevents the enemy to collide entirely with the target
        stayDistance = backOffDistance + attackRange; // Prevents the enemy to collide entirely with the target

        wanderMvSpeed = 3f;

        forgivenessTime = 3f;
        triggerDistance = 5f;
        jumpEnabled = true;

        // Item Drop Parameters
        dropChance = 3;
    }


    // ========================================= UNITY MAIN METHODS =========================================
    // Start is called before the first frame update
    void Awake()
    {
        NPCInit();
        EntityInitilization();
    }

    // Updates Every Physics Frame
    void FixedUpdate()
    {
        EnemyNPCFixedUpdate();

        Controller();
        isGrounded = capColl.IsTouchingLayers(groundLayers) || capColl.IsTouchingLayers(enemyLayers);
        Movement();
        AnimationState(); // Updates the Animation of the Entity
    }

    // Update is called once per frame
    void Update()
    {
        EnemyNPCUpdate();
    }


    // ========================================= ANIMATION METHODS =========================================
    protected void AnimationState()
    {
        animationCurrentState = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Substring(entityName.Length + 1);
        EntityAnimationState();
    }
}
