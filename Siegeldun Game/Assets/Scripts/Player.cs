using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Entity
{
    // ========================================= ENTITY PROPERTIY SCALING =========================================
    // Battle Mechanics
    private float[] weaponStaminaCost = new float[] { 5f, 8f, 10f }; // Pseudo Stamina cost
    private float[] weaponAttackSpeed = new float[] { 1f, 2f, 5f }; // Pseudo Attack Speed

    // HP Mechanics
    private float[] maxHpScaling = new float[] { 100f, 200f, 400f };
    private float[] healthRegenScaling = new float[] { .01f, .005f, .001f };

    //Stamina Mechanics
    private float[] maxStamScaling = new float[] { 100f, 200f, 400f };
    private float[] stamRegenScaling = new float[] { .01f, .005f, .001f };

    // Entity Properties Initialization
    private void EntityInitialization()
    {
        entityName = "player";
        isAlive= true;

        // Battle Initialization
        entityWeapon = 0; // Pseudo Weapon Index
        entityDamage = 30f; // Pseudo Damage
        attackSpeed = weaponAttackSpeed[idxDiff];
        attackRange = 0.3f; // Pseudo Weapon Range
        EqWeaponStamCost = weaponStaminaCost[entityWeapon];
        weaponDrag = 3f; // Pseudo Weapon Drag
        attacking = false;
        kbDir = 0;
        kTick = 0f;
        kbHorDisplacement = .8f;
        kbVerDisplacement = .4f;

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
        mvSpeed = 5f;
        jumpForce = 15.5f;
    }


    // ========================================= UNITY PROPERTIES =========================================
    // Component Declaration
    private Rigidbody2D body;
    private CapsuleCollider2D capColl;
    private Animator animator;
    private SpriteRenderer sprite;


    // ========================================= UNITY MAIN METHODS =========================================
    // Initializes when the Player Script is called
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        capColl = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        EntityInitialization();
    }

    // Updates Every Frame
    void Update()
    {
        Debug.Log(runAnimationSpeed);

        if (isAlive)
        {
            PassiveSkills();

            Controller();
        }

        // Pseudo Revive
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                HpRegen(maxHealth, "instant");
                isAlive = true;
            }
        }

        AnimationState();
        HealthBarUpdate();
        StaminaBarUpdate();
    }


    // ========================================= PLAYER METHODS =========================================
    private void PassiveSkills()
    {
        // HP Natural Healing
        if (entityHp < maxHealth)
        {
            if (hpRegenTimer >= regenDelay)
            {
                HpRegen(healthRegen);
            }
            else
            {
                hpRegenTimer += Time.deltaTime; // Heal Delay after not receiving damage
            }
        }
        else
        {
            hpRegenTimer = 0f;
        }

        // Stamina Natural Healing
        if (entityStam < maxStam)
        {
            StamRegen(stamRegen);
        }
    }

    // Regen
    public void HpRegen(float healAmount, string HealSpeed = "overtime")
    {
        if (hpRegenAllowed)
        {
            entityHp += healAmount;
            if (HealSpeed == "instant")
            {
                HealthbarF.fillAmount = entityHp / maxHealth;
            }
            entityHp = Mathf.Clamp(entityHp, 0, maxHealth);
        }
    }

    public void StamRegen(float StamAmount, string HealSpeed = "overtime")
    {
        if (stamRegenAllowed)
        {
            entityStam += StamAmount;
            if (HealSpeed == "instant")
            {
                StamBar.fillAmount = entityStam / maxStam;
            }
            entityStam = Mathf.Clamp(entityStam, 0, maxStam);
        }
    }

    // ========================================= ENTITY METHODS =========================================
    // Damage Receive
    public void TakeDamage(float damageTaken, float knockbackedForce, int kbDir)
    {
        hpRegenTimer = 0f;
        if (damageTaken >= entityHp)
        {
            entityHp -= entityHp;
            Death();
        }
        else
        {
            entityHp -= damageTaken;
            Knockback(knockbackedForce, kbDir);
        }
    }

    public void Knockback(float kbHorDisplacement, int kbDir)
    {
        isKnockbacked = true;
        knockbackedForce = kbHorDisplacement * 5f;
        this.kbDir = kbDir;
        kTick = Time.deltaTime;
    }

    private void Death()
    {
        isAlive = false;
        Debug.Log("Player Dead!");
    }

    // Damage Give
    private void Attack()
    {
        entityStam -= EqWeaponStamCost;

        // Attack Animation
        animator.SetTrigger("attack");

        // Collision Sensing
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage
        /*foreach (Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }*/
    }

    private void Consume()
    {

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
        attackFacing = (attacking) ? ((sprite.flipX) ? -1 : 1) : 0; // Weapon drag when attacking
        knockbackFacing = (isKnockbacked) ? kbDir : 0;
        dirX = ((attacking) ? dirX * slideDivisor : Input.GetAxisRaw("Horizontal"));
        runVelocity = (dirX * mvSpeed) + (attackFacing * weaponDrag) + (knockbackFacing * knockbackedForce);

        // Vertical Movement
        isGrounded = capColl.IsTouchingLayers(groundLayers);
        dirY = ((isGrounded && Input.GetButtonDown("Jump")) ? jumpForce : body.velocity.y);
        jumpVelocity = (dirY) + ((isKnockbacked) ? kbVerDisplacement : 0f);

        body.velocity = new Vector2(runVelocity, jumpVelocity);

        // Attack Code
        attacking = GetComponent<SpriteRenderer>().sprite.ToString().Substring(0, 11) == "Noob_Attack"; // Anti-spamming code
        if (Input.GetKeyDown(KeyCode.Mouse0) && attacking == false && EqWeaponStamCost <= entityStam)
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
            TakeDamage(Random.Range(5, 10), kbHorDisplacement, (sprite.flipX) ? 1 : -1);
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
                animator.speed = animationSpeed;
            }
            else
            {
                state = MovementAnim.run;
                animator.speed = runAnimationSpeed;
                if (dirX > 0f)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
            }

            // Vertical Movement Animation
            if (body.velocity.y > .99f)
            {
                state = MovementAnim.jump;
            }
            else if (body.velocity.y < -1f)
            {
                state = MovementAnim.fall;
            }

            if (attacking)
            {
                animator.speed = attackSpeed;
            }
            else
            {
                animator.speed = animationSpeed;
            }
        }
        else
        {
            state = MovementAnim.idle;
        }

        animator.SetInteger("state", (int)state);
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}