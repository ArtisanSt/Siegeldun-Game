using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beings : Entity
{
    // ========================================= ENTITY PROPERTIY SCALING =========================================
    // HP Mechanics
    protected float[] maxHpScaling;
    protected float[] healthRegenScaling;

    //Stamina Mechanics
    protected float[] maxStamScaling;
    protected float[] stamRegenScaling;


    // ========================================= BEINGS PROPERTIES =========================================
    // Function Parameters
    protected bool isHurting;
    protected bool isAttacking;
    protected bool isDying;

    // Animation Properties
    protected int defaultFacing; // 1 is right, -1 is left, 0 when not attacking
    protected string animationCurrentState;
    protected string currentSprite;

    [Header("ENTITY PROPERTIES", order = 0)]
    [Header("Battle Mechanics", order = 1)]
    protected bool attacking;
    protected int attackFacing; // 1 is right and -1 is left
    [SerializeField] protected int entityWeapon;
    [SerializeField] protected float weaponDrag;
    [SerializeField] protected float weaponKbForce;
    [SerializeField] protected float EqWeaponStamCost;
    [SerializeField] protected float entityDamage;
    [SerializeField] protected float attackDelay;
    protected bool isCrit;
    protected float critHit;
    protected int critChance;
    protected int attackCombo;
    protected float comboTime;
    [SerializeField] protected float attackRange;
    protected int knockbackFacing;
    [SerializeField] public float kbHorDisplacement;
    [SerializeField] protected float kbVerDisplacement;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask enemyLayers;

    [Header("HP Mechanics", order = 1)]
    [SerializeField] protected bool hpRegenAllowed;
    [SerializeField] protected float healthRegen;
    [SerializeField] protected float regenDelay;
    private float hpTick = 0f;
    private float hpHideTime = 4f;
    protected GameObject EntityStatusBar;

    [Header("Stamina Mechanics", order = 1)]
    [SerializeField] protected float maxStam;
    [SerializeField] protected float entityStam;
    [SerializeField] protected bool stamRegenAllowed;
    [SerializeField] protected float stamRegen;

    [Header("Movement Mechanics", order = 1)]
    protected const float slowDownConst = 0.90f;
    [SerializeField] protected float mvSpeed;
    [SerializeField] protected float mvSpeedBoost = 0f;
    [SerializeField] protected float totalMvSpeed;
    [SerializeField] protected float dirFacing;
    [SerializeField] protected float dirX;
    [SerializeField] protected float runVelocity;
    [SerializeField] protected bool allowJump;
    [SerializeField] public bool isGrounded;
    [SerializeField] protected float jumpForce;
    [SerializeField] protected float dirY;
    [SerializeField] protected float jumpVelocity;
    protected float runAnimationSpeed;
    [SerializeField] protected LayerMask groundLayers;


    protected void BeingsInitialization()
    {
        entityType = "Beings";
        ComponentInitialization();
        entityID = rBody.gameObject.GetInstanceID();
        isAlive = true;
        willBeDestroyed = false;
        EntityStatusBar = GameObject.Find($"/Enemies/{rBody.name}/EntityStatusBar");
    }


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
                EntityStatusBar.SetActive(true);
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
                    EntityStatusBar.SetActive(false);
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
        if (hpRegenAllowed)
        {
            // HP Natural Healing
            if (entityHp < maxHealth && hpParam)
            {
                if (hpRegenTimer >= regenDelay)
                {
                    bool didEffect = HPRegen(healthRegen, "Passive");
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

        if (stamRegenAllowed)
        {
            // Stamina Natural Healing
            if (entityStam < maxStam)
            {
                bool didEffect = StamRegen(stamRegen, "Passive");
            }
        }
    }

    // Regen
    public bool HPRegen(float healAmount, string healSpeed = "instant", float timeHeal = 0f)
    {
        if (entityHp != maxHealth)
        {
            if (healSpeed == "instant" || healSpeed == "Passive")
            {
                entityHp += healAmount;
                if (healSpeed == "instant")
                {
                    HealthbarF.fillAmount = entityHp / maxHealth;
                }
                entityHp = Mathf.Clamp(entityHp, 0, maxHealth);
            }
            else if (healSpeed == "overtime")
            {
                HealOvertime(healAmount, timeHeal, true);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool StamRegen(float stamAmount, string healSpeed = "instant", float timeHeal = 0f)
    {
        if (entityStam != maxStam)
        {
            if (healSpeed == "instant" || healSpeed == "Passive")
            {
                entityStam += stamAmount;
                if (healSpeed == "instant")
                {
                    StamBar.fillAmount = entityStam / maxStam;
                }
                entityStam = Mathf.Clamp(entityStam, 0, maxStam);
            }
            else if (healSpeed == "overtime")
            {
                HealOvertime(stamAmount, timeHeal, false);
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator HealOvertime(float healAmount, float timeHeal, bool isHealth)
    {
        float startTime = Time.time;
        while (Time.time - timeHeal <= startTime)
        {
            if (isHealth)
            {
                entityHp += healAmount;
            }
            else
            {
                entityStam += healAmount;
            }
        }
        yield return 0;
    }

    // ========================================= ENTITY STATS INITIALIZATION =========================================
    protected void EntityStatsInitialization(string entityName)
    {
        switch (entityName)
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
            case "Crawler":
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
        dirY = allowJump ? jumpForce : ((0f < rBody.velocity.y && rBody.velocity.y < 0.001f) ? 0f : rBody.velocity.y);
        jumpVelocity = (isGrounded) ? dirY : rBody.velocity.y;

        rBody.velocity = new Vector2(runVelocity, jumpVelocity);
        deadBeings = new Vector2(runVelocity, jumpVelocity) == new Vector2(0, 0);
    }


    // ========================================= ANIMATION METHODS =========================================
    protected void EntityAnimationState()
    {
        currentSprite = sprite.sprite.name;
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
