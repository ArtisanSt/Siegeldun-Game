using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    // ========================================= Game Properties =========================================
    [SerializeField] protected static int difficulty = 1; // Pseudo Difficulty
    protected int idxDiff = difficulty - 1;
    protected float animationSpeed = 1f;


    // ========================================= ENTITY PROPERTIY SCALING =========================================
    // HP Mechanics
    protected float[] maxHpScaling;
    protected float[] healthRegenScaling;

    //Stamina Mechanics
    protected float[] maxStamScaling;
    protected float[] stamRegenScaling;


    // ========================================= UNITY PROPERTIES =========================================
    // Component Declaration
    public Rigidbody2D rBody;
    public SpriteRenderer sprite;
    protected BoxCollider2D boxColl;
    protected CapsuleCollider2D capColl;
    protected Animator anim;

    private enum MovementAnim { idle, run, jump, fall };
    private MovementAnim state;

    protected void ComponentInitialization()
    {
        rBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxColl = GetComponent<BoxCollider2D>();
        capColl = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }


    // ========================================= Entity Properties =========================================
    protected string entityName;
    public bool isAlive = true;
    public bool isBreakable;
    protected bool isHurting;
    protected bool isAttacking;
    protected bool isDying;
    protected string animationCurrentState;
    protected string currentSprite;
    protected int defaultFacing; // 1 is right, -1 is left, 0 when not attacking

    [Header("ENTITY PROPERTIES", order = 0)]
    [Header("Battle Mechanics", order = 1)]
    [SerializeField] protected int entityWeapon;
    [SerializeField] protected float entityDamage;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected float attackDelay;
    protected float lastAttack;
    protected int attackCombo;
    protected float comboTime;
    [SerializeField] protected float attackRange;
    [SerializeField] protected float EqWeaponStamCost;
    [SerializeField] protected float weaponDrag;
    [SerializeField] protected float weaponKbForce;
    protected int attackFacing; // 1 is right and -1 is left
    protected bool attacking;
    protected bool isKnockbacked;
    protected int knockbackFacing;
    protected int kbDir; // 1 is right, -1 is left, 0 when not attacking
    protected float kTick;
    protected float knockbackedForce;
    [SerializeField] public float kbHorDisplacement;
    [SerializeField] protected float kbVerDisplacement;

    protected const float slowDownConst = 0.95f;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask enemyLayers;

    [Header("HP Mechanics", order = 1)]
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float entityHp;
    [SerializeField] protected bool hpRegenAllowed;
    [SerializeField] protected float healthRegen;
    [SerializeField] protected float regenDelay;
    [SerializeField] protected float hpRegenTimer;
    private float hpTick = 0f;
    private float hpHideTime = 4f;

    [Header("Stamina Mechanics", order = 1)]
    [SerializeField] protected float maxStam;
    [SerializeField] protected float entityStam;
    [SerializeField] protected bool stamRegenAllowed;
    [SerializeField] protected float stamRegen;

    [Header("Movement Mechanics", order = 1)]
    [SerializeField] protected float mvSpeed;
    [SerializeField] protected float mvSpeedBoost = 0f;
    [SerializeField] protected float totalMvSpeed;
    [SerializeField] protected float dirX;
    [SerializeField] protected float dirFacing;
    [SerializeField] protected float dirY;
    [SerializeField] protected bool allowJump;
    [SerializeField] protected float runVelocity;
    [SerializeField] public bool isGrounded;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float jumpVelocity;
    protected float runAnimationSpeed;
    [SerializeField] protected LayerMask groundLayers;


    // ========================================= Status Bar Properties =========================================
    [Header("STATUS BAR PROPERTIES", order = 0)]
    [Header("HP Component", order = 1)]
    [SerializeField] protected Image HealthbarF;
    [SerializeField] protected Image HealthbarB;
    [SerializeField] Text statHpText;

    [Header("Stamina Component", order = 1)]
    [SerializeField] protected Image StamBar;
    [SerializeField] Text statStamText;


    // ========================================= STATUS BAR METHODS =========================================
    // Health Bar UI Update
    protected void HealthBarUpdate()
    {
        // HP UI Parameters
        float fillF = HealthbarF.fillAmount;
        float fillB = HealthbarB.fillAmount;
        float hpFraction = entityHp / maxHealth;

        // Health Bar UI Updater
        if (fillB != fillF || fillF != hpFraction || fillB != hpFraction)
        {
            if (entityName != "Player")
            {
                HealthbarF.enabled = true;
                HealthbarB.enabled = true;
                statHpText.enabled = true;
                hpHideTime = 0f;
            }
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
            if (entityName != "Player")
            {
                if (hpHideTime > 3f)
                {
                    HealthbarF.enabled = false;
                    HealthbarB.enabled = false;
                    statHpText.enabled = false;
                }
                else
                {
                    hpHideTime += Time.deltaTime + ((attackDelay * 4) / 5);
                }
            }
        }

        // HP UI Text
        statHpText.text = Mathf.FloorToInt(entityHp).ToString() + " / " + Mathf.FloorToInt(maxHealth).ToString();
    }

    // Status Bar UI Update
    protected void StaminaBarUpdate()
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
        else
        {
            StamBar.color = Color.yellow;
        }

        // Stamina UI Text
        statStamText.text = Mathf.FloorToInt(entityStam).ToString() + " / " + Mathf.FloorToInt(maxStam).ToString();
    }


    // ========================================= HEALING METHODS =========================================
    protected void PassiveSkills(bool hpRegenAllowed, bool stamRegenAllowed, float regenDelay, bool hpParam = true)
    {
        if(hpRegenAllowed)
        {
            // HP Natural Healing
            if (entityHp < maxHealth && hpParam)
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
        }

        if(stamRegenAllowed)
        {
            // Stamina Natural Healing
            if (entityStam < maxStam)
            {
                StamRegen(stamRegen);
            }
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


    // ========================================= ENTITY METHODS =========================================
    // Damage Receive
    public void TakeDamage(float damageTaken, int kbDir, float knockbackedForce)
    {
        if (isAlive && damageTaken > 0f)
        {
            hpRegenTimer = 0f;
            if (damageTaken >= Mathf.Floor(entityHp))
            {
                entityHp -= entityHp;
                Die();
            }
            else
            {
                entityHp -= damageTaken;
                if (!isBreakable)
                {
                    if (Random.Range(1, 101) == 1)
                    {
                        lastAttack = Time.time;
                        anim.SetTrigger("hurt");
                        Knockback(kbDir, knockbackedForce);
                    }
                }
            }
        }
    }

    public void Knockback(int kbDir, float kbHorDisplacement)
    {
        isKnockbacked = true;
        knockbackedForce = kbHorDisplacement * 5f;
        this.kbDir = kbDir;
        kTick = Time.deltaTime;
    }

    private void Die()
    {
        Debug.Log(entityName + " Dead!");
        if (!isBreakable)
        {
            anim.SetBool("death", true);
            isAlive = false;
        }
    }

    protected void ClearInstance()
    {
        if (capColl.enabled && rBody.velocity == new Vector2(0, 0))
        {
            capColl.enabled = false;
            boxColl.enabled = false;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            
            StartCoroutine(DestroyInstance());
        }

    }

    private IEnumerator DestroyInstance()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }


    // ========================================= ENTITY STATS INITIALIZATION =========================================
    protected void EntityStatsInitialization(string entityName)
    {
        switch(entityName)
        {
            case "Player":
                this.maxHpScaling = new float[] { 100f, 200f, 400f };
                this.healthRegenScaling = new float[] { .01f, .005f, .001f };

                this.maxStamScaling = new float[] { 100f, 200f, 400f };
                this.stamRegenScaling = new float[] { .01f, .005f, .001f };
                break;
            case "Goblin":
                this.maxHpScaling = new float[] { 100f, 200f, 400f };
                this.healthRegenScaling = new float[] { .01f, .005f, .001f };
                break;
        }
    }


    // ========================================= MOVEMENT METHODS =========================================
    protected void Movement()
    {
        // Horizontal Movement
        knockbackFacing = (isKnockbacked) ? kbDir : 0; // Knockback Effect
        dirX = (isAttacking) ? dirX * slowDownConst : dirFacing; // Front movement with a slowdown effect when attacking
        totalMvSpeed = mvSpeed + mvSpeedBoost;
        runVelocity = (isAlive) ? ((dirX * totalMvSpeed) + (knockbackFacing * knockbackedForce)) : dirX * slowDownConst;

        // Vertical Movement
        isGrounded = capColl.IsTouchingLayers(groundLayers) || capColl.IsTouchingLayers(enemyLayers);
        dirY = allowJump ? jumpForce : ((0f < rBody.velocity.y && rBody.velocity.y < 0.001f) ? 0f : rBody.velocity.y);
        jumpVelocity = (isGrounded) ? dirY : rBody.velocity.y;

        rBody.velocity = new Vector2(runVelocity, jumpVelocity);
    }


    // ========================================= ANIMATION METHODS =========================================
    protected void AnimationState()
    {
        currentSprite = sprite.sprite.name;
        animationCurrentState = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Substring(entityName.Length + 1);
        if (animationCurrentState == "Hurt")
        {
            isHurting = true;
        }
        else
        {
            isHurting = false;
        }

        if (animationCurrentState == "Attack")
        {
            isAttacking = true;
            anim.speed = attackSpeed;
        }
        else
        {
            isAttacking = false;
        }

        if (isAlive)
        {
            if (!isHurting && !isAttacking && !isDying)
            {
                // Vertical Movement Animation
                if (jumpVelocity > .99f)
                {
                    state = MovementAnim.jump;
                }
                else if (jumpVelocity < -1f)
                {
                    state = MovementAnim.fall;
                }
                else
                {
                    // Horizontal Movement Animation
                    runAnimationSpeed = totalMvSpeed / mvSpeed;
                    if (dirX == 0)
                    {
                        state = MovementAnim.idle;
                        anim.speed = animationSpeed;
                    }
                    else
                    {
                        state = MovementAnim.run;
                        anim.speed = runAnimationSpeed;
                        transform.localScale = new Vector3(((dirX > 0f) ? 1 : -1) * defaultFacing, 1, 1);
                    }
                }
            }
            anim.SetInteger("state", (int)state);
        }
    }
}
