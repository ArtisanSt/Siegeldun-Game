using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // ========================================= Status Bar Properties =========================================
    // HP Component Declaration
    [SerializeField] Image HealthbarF;
    [SerializeField] Image HealthbarB;

    private int statHp;
    [SerializeField] Text statHpText;

    // HP Parameter Declaration
    private float hpTick;
    private float hpRegenTimer;

    // Stamina Component Declaration
    [SerializeField] Image StamBar;

    private int statStam;
    [SerializeField] Text statStamText;

    // Called when Start
    public void StatusInitialization()
    {
        // HP Initialization
        maxHealth = maxHpScaling[idxDiff];
        healthRegen = healthRegenScaling[idxDiff];
        entityHp = maxHealth;
        hpRegenTimer = 0f;

        // Stamina Initialization
        maxStam = 100f;
        stamRegen = .01f;
        entityStam = maxStam;
        EqWeaponStamCost = weaponStaminaCost[entityWeapon];
        attacking = false;

        // Battle Initialization
        attackSpeed = 1f;
    }


    // ========================================= STATUS BAR METHODS =========================================
    // Health Bar UI Update
    private void HealthBarUpdate()
    {
        // HP UI Parameters
        float fillF = HealthbarF.fillAmount;
        float fillB = HealthbarB.fillAmount;
        float hpFraction = entityHp / maxHealth;

        // Health Bar UI Updater
        if (fillB != fillF || fillF != hpFraction || fillB != hpFraction)
        {
            float netRegenF = hpFraction - fillF;
            float netRegenB = fillF - fillB;
            float percentChangeB, percentChangeF;

            if (fillF > hpFraction) // When Front Health Bar is greater than the real health
            {
                HealthbarF.color = Color.cyan;
                HealthbarF.fillAmount = hpFraction;
            }
            else if (fillF < hpFraction) // When Front Health Bar is lower than the real health
            {
                HealthbarF.color = Color.green;
                percentChangeF = healthRegen / (maxHealth * netRegenF);
                HealthbarF.fillAmount += Mathf.Lerp(0, netRegenF, percentChangeF);
            }

            if (fillB > fillF) // When Back Health Bar is greater than Front Health Bar
            {
                hpTick += 0.1f;
                HealthbarB.color = Color.red;
                percentChangeB = (-0.001f / netRegenB) * hpTick;
                HealthbarB.fillAmount += Mathf.Lerp(0, netRegenB, percentChangeB);
            }
            else if (fillB < fillF)// When Back Health Bar is lower than Front Health Bar
            {
                hpTick = 0f;
                percentChangeB = healthRegen / (maxHealth * netRegenB);
                HealthbarB.fillAmount = fillF;
            }
        }
        else
        {
            hpTick = 0f;
            HealthbarF.color = Color.cyan;
        }

        // HP UI Text
        statHp = (int)entityHp;
        statHpText.text = statHp.ToString() + " / " + maxHealth.ToString();
    }

    // Status Bar UI Update
    private void StaminaBarUpdate()
    {
        // Stamina UI Parameters
        float fillS = StamBar.fillAmount;
        float stamFraction = entityStam / maxStam;

        // Stamina Bar UI Updater
        if (fillS != stamFraction)
        {
            if (fillS > stamFraction)
            {
                StamBar.fillAmount = stamFraction;
            }
            else if (fillS < stamFraction)
            {
                float netRegenS = stamFraction - fillS;
                float percentChangeS = stamRegen / (maxStam * netRegenS);
                StamBar.fillAmount += Mathf.Lerp(0, netRegenS, percentChangeS);
            }
        }

        // Stamina UI Text
        statStam = (int)entityStam;
        statStamText.text = statStam.ToString() + " / " + maxStam.ToString();
    }


    // ========================================= Unity Properties =========================================
    // Component Declaration
    private Rigidbody2D body;
    private CapsuleCollider2D capColl;
    private Animator animator;
    private SpriteRenderer sprite;

    // Movement Parameters
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] private LayerMask groundLayers;
    private float dirX;
    private enum MovementAnim { idle, run, jump, fall };
    private MovementAnim state;


    // ========================================= UNITY MAIN METHODS =========================================
    // Initializes when the Player Script is called
    void Start()
    {
        // Component Variable Definition
        body = GetComponent<Rigidbody2D>();
        capColl = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        StatusInitialization();
    }

    // Updates Every Frame
    void Update()
    {
        if(isAlive)
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


    // ========================================= Game Properties =========================================
    public static int difficulty = 1; // Pseudo Difficulty
    private int idxDiff = difficulty - 1;
    private float animationSpeed = 1f;


    // ========================================= Entity Properties =========================================
    private string entityName = "player";
    private bool isAlive = true;

    // Property Scaling
    private float[] maxHpScaling = new float[] { 100f, 200f, 400f };
    private float[] healthRegenScaling = new float[] { .01f, .005f, .001f };
    private float[] weaponStaminaCost = new float[] { 2f, 5f, 10f }; // Pseudo Stamina cost
    private float[] weaponAttackSpeed = new float[] { 1f, 2f, 5f }; // Pseudo Attack Speed

    // Battle Mechanics
    public int entityWeapon = 0; // Pseudoweapon
    public float entityDamage;
    public float attackSpeed;
    public float attackRange = 0.3f;
    private const float slideDivisor = 0.96f;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage = 10;

    // HP Mechanics
    private float entityHp;
    private float healthRegen;
    private float regenDelay = 3f;
    private float maxHealth;

    // Stamina Mechanics
    private float entityStam;
    private float stamRegen;
    private float maxStam;
    private bool attacking;
    private float EqWeaponStamCost;

    // ========================================= ENTITY METHODS =========================================
    private void PassiveSkills()
    {
        // HP Natural Healing
        if (entityHp < maxHealth)
        {
            hpRegenTimer += Time.deltaTime; // Heal Delay after not receiving damage
            if (hpRegenTimer >= regenDelay)
            {
                HpRegen(healthRegen);
            }
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
        entityHp += healAmount;
        if (HealSpeed == "instant")
        {
            HealthbarF.fillAmount = entityHp / maxHealth;
        }
        entityHp = Mathf.Clamp(entityHp, 0, maxHealth);
    }

    public void StamRegen(float StamAmount, string HealSpeed = "overtime")
    {
        entityStam += StamAmount;
        if (HealSpeed == "instant")
        {
            StamBar.fillAmount = entityStam / maxStam;
        }
        entityStam = Mathf.Clamp(entityStam, 0, maxStam);
    }

    // Damage Receive
    public void TakeDamage(float damageTaken)
    {
        if (damageTaken >= entityHp)
        {
            entityHp -= entityHp;
            Death();
        }
        else
        {
            entityHp -= damageTaken;
        }

        hpRegenTimer = 0f;
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
        attacking = true;
        animator.SetTrigger("attack");
        if (attacking)
        {
            animator.speed = attackSpeed;
        }
        else
        {
            animator.speed = animationSpeed;
        }
        attacking = false;

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
        // Movement Animation
        // Horizontal Movement
        if (attacking)
        {
            dirX *= slideDivisor; // Slide Attack
        }
        else
        {
            dirX = Input.GetAxisRaw("Horizontal");
        }
        body.velocity = new Vector2(dirX * speed, body.velocity.y);

        // Vertical Movement
        if (Input.GetButtonDown("Jump") && capColl.IsTouchingLayers(groundLayers))
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }

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
            TakeDamage(Random.Range(5, 10));
        }

        // Pseudo Heal
        if (Input.GetKeyDown(KeyCode.E))
        {
            HpRegen(20f, "instant");
        }
    }


    // ========================================= ANIMATION METHODS =========================================
    private void AnimationState()
    {
        if (isAlive)
        {
            // Lateral Movement Animation
            if (dirX == 0)
            {
                state = MovementAnim.idle;
            }
            else
            {
                state = MovementAnim.run;
                if (dirX > 0f)
                    sprite.flipX = false;
                else
                    sprite.flipX = true;
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

