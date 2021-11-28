using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // ========================================= Game Properties =========================================
    public static int difficulty = 1; // Pseudo Difficulty
    private int idxDiff = difficulty - 1;


    // ========================================= Entity Properties =========================================
    private string entityName = "player";

    private float[] maxHpScaling = new float[] { 100f, 200f, 400f };
    private float[] healthRegenScaling = new float[] { .01f, .005f, .001f };

    // HP Mechanics
    private float entityHp;
    private float healthRegen;
    private float regenDelay = 3f;
    private float maxHealth;

    // Battle Mechanics
    public float entityDamage;
    public float attackSpeed;


    // ========================================= Status Bar Properties =========================================
    // HP Component Declaration
    [SerializeField] Image HealthbarF;
    [SerializeField] Image HealthbarB;

    private int statHp;
    [SerializeField] Text statHpText;

    // HP Parameter Declaration
    private bool isDamaged = false;
    private bool regenerate = false;

    private float tick;
    private float hpRegenTimer;

    // Called when Start
    public void HpInitialization()
    {
        maxHealth = maxHpScaling[idxDiff];
        healthRegen = healthRegenScaling[idxDiff];
        entityHp = maxHealth;
        hpRegenTimer = 0f;
    }

    // Update is called once per frame
    public void HpUpdate()
    {
        // Player UI HP Frame Update
        PlayerHealthBarUpdate();
    }

    // Healthbar UI Update
    public void PlayerHealthBarUpdate()
    {
        // Natural Healing starter
        if (entityHp < maxHealth)
        {
            hpRegenTimer += Time.deltaTime;

            if (hpRegenTimer >= regenDelay)
            {
                Regen(healthRegen);
                regenerate = true;
            }
        }
        else
        {
            regenerate = false;
        }

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
                tick += 0.1f;
                HealthbarB.color = Color.red;
                percentChangeB = (-0.001f / netRegenB) * tick;
                HealthbarB.fillAmount += Mathf.Lerp(0, netRegenB, percentChangeB);
            }
            else if (fillB < fillF)// When Back Health Bar is lower than Front Health Bar
            {
                tick = 0f;
                percentChangeB = healthRegen / (maxHealth * netRegenB);
                HealthbarB.fillAmount = fillF;
            }
        }
        else
        {
            tick = 0f;
            HealthbarF.color = Color.cyan;
        }

        // HP UI Text
        statHp = (int)entityHp;
        statHpText.text = statHp.ToString() + " / " + maxHealth.ToString();
    }

    public void TakeDamage(float damageTaken)
    {
        regenerate = false;
        isDamaged = true;
        entityHp -= damageTaken;
        hpRegenTimer = 0f;
    }

    public void Regen(float healAmount, string HealSpeed = "overtime")
    {
        entityHp += healAmount;
        if(HealSpeed == "instant")
        {
            HealthbarF.fillAmount = entityHp / maxHealth;
        }
        entityHp = Mathf.Clamp(entityHp, 0, maxHealth);
    }


    // ========================================= Unity Properties =========================================
    // Component Declaration
    private Rigidbody2D body;
    private CapsuleCollider2D capColl;
    private Animator animator;
    private SpriteRenderer sprite;
    private float dirX;

    // Variable Serialization
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] private LayerMask groundLayers;

    // Attack Parameters
    public bool attacking = false;
    public float attackRange = 0.3f;
    [SerializeField] Transform attackPoint; 
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage = 10;

    private enum MovementAnim { idle, run, jump, fall };
    private MovementAnim state;

    // Initializes when the Player Script is called
    private void Start()
    {
        // Component Variable Definition
        body = GetComponent<Rigidbody2D>();
        capColl = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        HpInitialization();
    }

    // Updates Every Frame
    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * speed, body.velocity.y);

        if (Input.GetButtonDown("Jump") && capColl.IsTouchingLayers(groundLayers))
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }

        AnimationState();


        // Attack Code
        attacking = GetComponent<SpriteRenderer>().sprite.ToString().Substring(0, 11) == "Noob_Attack"; // Anti-spamming code
        if (Input.GetKeyDown(KeyCode.Mouse0) && attacking == false)
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(Random.Range(5, 10));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Regen(20f, "instant");
        }

        HpUpdate();
    }

    private void AnimationState()
    {
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

        if (body.velocity.y > .99f)
        {
            state = MovementAnim.jump;
        }
        else if (body.velocity.y < -1f)
        {
            state = MovementAnim.fall;
        }

        animator.SetInteger("state", (int)state);
    }

    private void Attack()
    {
        attacking = true;
        // Play Attack anim
        animator.SetTrigger("attack");
        // Detect enemy in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        attacking = false;

        // Damage
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    private void Consume()
    {

    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

