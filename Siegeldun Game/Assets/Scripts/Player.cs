using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    // ========================================= UNITY PROPERTIES =========================================
    private float[] animationTime = new float[9];



    // ========================================= ENTITY PROPERTIY SCALING =========================================
    // Battle Mechanics
    protected float[] weaponStaminaCost = new float[] { 5f, 8f, 10f }; // Pseudo Stamina cost
    protected float[] weaponAttackSpeed = new float[] { .5f, .95f, .5f }; // Pseudo Attack Speed


    // Entity Properties Initialization
    private void EntityInitialization()
    {
        entityName = "Player";
        defaultFacing = 1;
        EntityStatsInitialization(entityName);
        isAlive = true;
        isBreakable = false;

        // Battle Initialization
        entityWeapon = 0; // Pseudo Weapon Index
        entityDamage = 30f; // Pseudo Damage
        attackSpeed = 1 / weaponAttackSpeed[idxDiff];
        attackDelay = (1 / attackSpeed) + .1f;
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
        stamRegen = stamRegenScaling[idxDiff];

        // Movement Initialization
        mvSpeed = 7.4f;
        jumpForce = 19.5f;
        rBody.gravityScale = 6;
    }

    protected void AnimClipTime()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name.Substring(entityName.Length + 1))
            {
                case "Idle":
                    animationTime[0] = clip.length;
                    break;
                case "Run":
                    animationTime[1] = clip.length;
                    break;
                case "Jump":
                    animationTime[2] = clip.length;
                    break;
                case "Fall":
                    animationTime[3] = clip.length;
                    break;
                case "Hurt":
                    animationTime[4] = clip.length;
                    break;
                case "Death":
                    animationTime[5] = clip.length;
                    break;
                case "Attack_1":
                    animationTime[6] = clip.length;
                    break;
                case "Attack_2":
                    animationTime[7] = clip.length;
                    break;
                case "Attack_3":
                    animationTime[8] = clip.length;
                    break;
            }
        }
    }


    // ========================================= UNITY MAIN METHODS =========================================
    // Initializes when the Player Script is called
    void Start()
    {
        ComponentInitialization();
        EntityInitialization();
    }

    // Updates Every Frame
    void Update()
    {
        if (isAlive)
        {
            PassiveSkills(hpRegenAllowed, stamRegenAllowed, regenDelay);
            Timer();
        }
        else
        {
            ClearInstance();
        }

        Controller();
        Movement();
        AnimationState();
        HealthBarUpdate();
        StaminaBarUpdate();
    }

    // ========================================= PLAYER METHODS =========================================
    // Damage Give
    private void Attack()
    {
        // Attack Animation
        anim.SetTrigger("attack" + attackCombo.ToString());
        comboTime = 0f;
        entityStam -= EqWeaponStamCost;
        lastAttack = Time.time;
        attackCombo = (attackCombo == 3) ? 1 : attackCombo + 1;

        // Collision Sensing
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            int kbDir = (enemy.GetComponent<Rigidbody2D>().position.x > rBody.position.x) ? 1 : -1;
            enemy.GetComponent<Entity>().TakeDamage(entityDamage / 3, kbDir, weaponKbForce);
        }
    }

    private void Consume()
    {

    }


    // ========================================= CONTROLLER METHODS =========================================
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
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking && !isHurting && Time.time - lastAttack > attackDelay && EqWeaponStamCost <= entityStam)
            {
                Attack();
            }

            // Pseudo Attack Speed Changer
            if (Input.GetKeyDown(KeyCode.P))
            {
                attackSpeed += 0.1f;
            }

            if (Input.GetKeyDown(KeyCode.O))
            {
                attackSpeed -= 0.1f;
            }

            // Pseudo Damage Taken 
            if (Input.GetKeyDown(KeyCode.Q))
            {
                TakeDamage(50, (sprite.flipX) ? 1 : -1, kbHorDisplacement);
            }

            // Pseudo Heal
            if (Input.GetKeyDown(KeyCode.E))
            {
                HpRegen(20f, "instant");
                StamRegen(20f, "instant");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}