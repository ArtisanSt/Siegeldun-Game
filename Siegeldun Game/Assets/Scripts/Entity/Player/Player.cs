using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Inventory
{
    // ========================================= UNITY PROPERTIES =========================================
    private float[] animationTime = new float[9];

    protected GameObject weaponSpriteObject;
    protected Animator weaponAnim;


    // ========================================= ENTITY PROPERTIY SCALING =========================================
    // Battle Mechanics
    protected float[] weaponStaminaCost = new float[] { 20f, 25f, 33f }; // Pseudo Stamina cost
    protected float[] weaponAttackSpeed = new float[] { .5f, 1.17f, .95f }; // Pseudo Attack Speed

    // Entity Properties Initialization
    private void UniquePropertyInit()
    {
        entityName = "Player";
        defaultFacing = 1;
        EntityStatsInitialization(entityName);

        // Battle Initialization
        weaponGameObject = (weaponSlot.transform.childCount > 0) ? weaponSlot.transform.GetChild(0).gameObject.GetComponent<Weapon>() : null;
        entityWeapon = 0; // Pseudo Weapon Index
        entityDamage = 30f; // Pseudo Damage
        attackSpeed = weaponAttackSpeed[idxDiff];
        attackDelay = 0.3f; //(1 / attackSpeed) + .1f;
        critHit = .3f; // Pseudo critHit
        critChance = 100; // Pseudo critChance
        lastAttack = 0f;
        attackCombo = 1;
        comboTime = 0f;
        attackRange = 0.3f; // Pseudo Weapon Range
        EqWeaponStamCost = weaponStaminaCost[entityWeapon];
        weaponDrag = 0f; // Pseudo Weapon Drag
        weaponKbForce = .8f; // Pseudo Weapon Knockback Force
        isAttacking = false;
        kbDir = 0;
        kTick = 0f;
        kbHorDisplacement = .8f;
        kbVerDisplacement = 0f;

        // HP Initialization
        maxHealth = maxHpScaling[idxDiff];
        entityHp = maxHealth;
        hpRegenAllowed = true;
        healthRegen = healthRegenScaling[idxDiff];
        regenDelay = 3f;
        hpRegenTimer = 0f;

        // Stamina Initialization
        maxStam = maxStamScaling[idxDiff];
        entityStam = maxStam;
        stamRegenAllowed = true;
        stamRegen = 0.13f; //stamRegenScaling[idxDiff];

        // Movement Initialization
        mvSpeed = 7.4f;
        jumpForce = 19.5f;
        rBody.gravityScale = 6;

        doDrop = true;
    }


    // ========================================= UNITY MAIN METHODS =========================================
    // Initializes when the Player Script is called
    void Awake()
    {
        BeingsInit();
        UniquePropertyInit();
        InventoryInit();
        StatusBarInit();
        weaponSpriteObject = entityGameObject.transform.GetChild(1).gameObject;
        weaponAnim = weaponSpriteObject.GetComponent<Animator>();

        PrefabsInit();
    }

    void Start()
    {
        StatsUpdate();
    }

    // Updates Every Frame
    void Update()
    {
        if (isAlive)
        {
            StatsUpdate();
            PassiveSkills(hpRegenAllowed, stamRegenAllowed, regenDelay);
            Timer();
        }
        else
        {
            DeathFinalizer();
            ClearInstance(5);
        }

        Controller();
        isGrounded = capColl.IsTouchingLayers(groundLayers) || capColl.IsTouchingLayers(enemyLayers);
        Movement();
        AnimationState();

        HealthBarUpdate();
        StaminaBarUpdate();
    }


    // ========================================= PLAYER METHODS =========================================
    // Damage Give
    private void Attack()
    {
        int attackID = Random.Range(-9999, 10000);
        // Attack Animation
        anim.SetTrigger("sword" + attackCombo.ToString());
        comboTime = 0f;
        entityStam -= EqWeaponStamCost;
        lastAttack = Time.time;
        attackCombo = (attackCombo == 3) ? 1 : attackCombo + 1;
        isCrit = Random.Range(1, critChance + 1) == 1;
        float totalDamage = entityDamage * (1 + critHit);

        // Collision Sensing
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            int kbDir = (enemy.GetComponent<Rigidbody2D>().position.x > rBody.position.x) ? 1 : -1;
            enemy.GetComponent<Entity>().TakeDamage(totalDamage, attackID, kbDir, weaponKbForce, isCrit);
        }
    }

    protected void DeathFinalizer()
    {
        // Inventory Reset
        ClearItem();
    }


    // ========================================= TIMER METHODS =========================================
    private void Timer()
    {
        if (attackCombo != 1)
        {
            if (comboTime >= attackSpeed * 2)
            {
                comboTime = 0f;
                attackCombo = 1;
            }
            else
            {
                comboTime += Time.deltaTime;
            }
        }
        else
        {
            comboTime = 0f;
        }

        // Pseudo Knockback Timer
        if (isKnockbacked)
        {
            if (kTick > .1f)
            {
                isKnockbacked = false;
                knockbackedForce = kbHorDisplacement;
                kTick = 0f;
            }
            else
            {
                kTick += Time.deltaTime;
            }
        }
    }


    // ========================================= CONTROLLER METHODS =========================================
    private void Controller()
    {
        // Vertical Movement
        allowJump = (isAlive) ? Input.GetButtonDown("Jump") : false;

        // Horizontal Movement
        dirFacing = (isAlive) ? Input.GetAxisRaw("Horizontal") : 0;

        if (isAlive)
        {
            // Attack Code
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isAttacking && !isHurting && Time.time - lastAttack > attackDelay && EqWeaponStamCost <= entityStam)
            {
                Attack();
            }

            ConsumeControls();
        }
    }
    

    // ========================================= ANIMATION METHODS =========================================
    protected void AnimationState()
    {
        animationCurrentState = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Substring(entityName.Length + 1);
        EntityAnimationState();
    }


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}