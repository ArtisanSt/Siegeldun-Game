using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    // ========================================= UNITY PROPERTIES =========================================
    // Component Declaration
    private enum MovementAnim { idle, run, jump, fall };
    private MovementAnim state;


    // ========================================= ENTITY PROPERTIY SCALING =========================================
    // Battle Mechanics
    protected float[] weaponStaminaCost = new float[] { 20f, 25f, 33f }; // Pseudo Stamina cost
    protected float[] weaponAttackSpeed = new float[] { 1.17f, .95f, .5f }; // Pseudo Attack Speed

    // Entity Properties Initialization
    private void EntityInitialization()
    {
        entityName = rBody.name;
        EntityStatsInitialization(entityName);
        isAlive = true;
        isBreakable = false;

        // Battle Initialization
        entityWeapon = 0; // Pseudo Weapon Index
        entityDamage = 30f; // Pseudo Damage
        attackSpeed = 1 / weaponAttackSpeed[idxDiff];
        attackDelay = 0.3f; //(1 / attackSpeed) + .1f;
        lastAttack = 0f;
        attackCombo = 1;
        comboTime = 0f;
        attackRange = 0.3f; // Pseudo Weapon Range
        EqWeaponStamCost = weaponStaminaCost[entityWeapon];
        weaponDrag = 0f; // Pseudo Weapon Drag
        weaponKbForce = .8f; // Pseudo Weapon Knockback Force
        attacking = false;
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
            Controller();
        }

        // Pseudo Revive
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                HpRegen(maxHealth, "instant");
                isAlive = true;
                capColl.enabled = true;
                cirColl.enabled = true;
                boxColl.enabled = true;
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            }
        }

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
        //attacking = true;
        comboTime = 0f;
        entityStam -= EqWeaponStamCost;
        lastAttack = Time.time;
        attackCombo = (attackCombo == 3) ? 1 : attackCombo + 1;

        // Collision Sensing
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            int kbDir = (enemy.GetComponent<Entity>().rBody.position.x > rBody.position.x) ? 1 : -1;
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
    }


    // ========================================= CONTROLLER METHODS =========================================
    private void Controller()
    {
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

        // Horizontal Movement
        attackFacing = (attacking) ? ((sprite.flipX) ? -1 : 1) : 0;
        knockbackFacing = (isKnockbacked) ? kbDir : 0; // Knockback Effect
        dirX = ((attacking) ? dirX * slowDownConst : Input.GetAxisRaw("Horizontal")); // Front movement with a slowdown effect when attacking
        totalMvSpeed = mvSpeed + mvSpeedBoost;
        runVelocity = (dirX * totalMvSpeed) + (attackFacing * weaponDrag) + (knockbackFacing * knockbackedForce);

        // Vertical Movement
        isGrounded = capColl.IsTouchingLayers(groundLayers);
        dirY = (Input.GetButtonDown("Jump")) ? jumpForce : ((0f < rBody.velocity.y && rBody.velocity.y < 0.001f) ? 0f : rBody.velocity.y);
        jumpVelocity = (isGrounded) ? dirY : rBody.velocity.y;

        rBody.velocity = new Vector2(runVelocity, jumpVelocity);

        // Attack Code
        attacking = anim.GetCurrentAnimatorStateInfo(0).IsName("Noob_Attack_" + attackCombo.ToString()); // Anti-spamming code
        if (Input.GetKeyDown(KeyCode.Mouse1) && attacking == false && Time.time - lastAttack > attackDelay && EqWeaponStamCost <= entityStam)
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


    // ========================================= ANIMATION METHODS =========================================
    private void AnimationState()
    {
        if (isAlive)
        {
            // Horizontal Movement Animation
            runAnimationSpeed = Mathf.Abs(runVelocity);
            if (dirX == 0)
            {
                state = MovementAnim.idle;
                anim.speed = animationSpeed;
            }
            else
            {
                state = MovementAnim.run;
                anim.speed = runAnimationSpeed;
                transform.localScale = new Vector3((dirX > 0f) ? 1 : -1, 1, 1);
            }

            // Vertical Movement Animation
            if (jumpVelocity > .99f)
            {
                state = MovementAnim.jump;
            }
            else if (jumpVelocity < -1f)
            {
                state = MovementAnim.fall;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Noob_Attack_" + attackCombo.ToString()))
            {
                anim.speed = attackSpeed;
            }
            else
            {
                anim.speed = totalMvSpeed / mvSpeed;
            }

            if (!attacking)
            {
                anim.SetInteger("state", (int)state);
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