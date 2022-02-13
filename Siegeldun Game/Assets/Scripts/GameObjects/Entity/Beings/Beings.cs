using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBoost
{
    public string boostName { get; private set; }
    public Vector2 speedBoost { get; private set; }

    public SpeedBoost(string boostName, Vector2 speedBoost)
    {
        this.boostName = boostName;
        BoostAdjustment(speedBoost);
    }

    public void BoostAdjustment(Vector2 speedBoost)
    {
        this.speedBoost = speedBoost;
    }
}

public abstract class Beings : Entity, IBoostable, IWeaponizable
{
    // ========================================= BEINGS PROPERTIY SCALING =========================================
    private string _entityType = "Beings";
    public override string entityType { get { return _entityType; } }




    protected virtual void Start()
    {
        InventoryPropInit();
    }

    // ========================================= MOVEMENT PROPERTIES =========================================
    [Header("MOVEMENT SETTINGS", order = 1)]
    [SerializeField] protected bool doMoveX = false;
    [SerializeField] protected float mvSpeed = 0;
    protected float dirXFacing = 0, dirXFinal = 0, runVelocity = 0;

    [SerializeField] protected float slowDownConst = 0.9f;
    public bool isGrounded;// Updates

    [SerializeField] protected LayerMask groundLayer;

    [SerializeField] protected bool doMoveY = false;
    [SerializeField] protected bool allowJump = false;
    [SerializeField] protected float jumpForce = 0;
    protected float dirYFinal = 0, jumpVelocity = 0;

    protected Vector2 totalSpeed; // Updates
    protected Dictionary<string, SpeedBoost> speedBoost = new Dictionary<string, SpeedBoost>();

    protected enum MovementAnim { idle, run, jump, fall };
    protected MovementAnim state;


    protected abstract void Controller();

    protected void Movement()
    {
        totalSpeed.Set(mvSpeed + TotalBoost("MVSpeed"), jumpForce + TotalBoost("JumpHeight"));

        // Horizontal Movement
        float slowDown = dirXFinal * slowDownConst;
        dirXFinal = (!isAttacking && !isHurting) ? dirXFacing : slowDown; // Front movement with a slowdown effect when attacking
        runVelocity = (isAlive) ? dirXFinal * totalSpeed.x + rcvKbDisplacement : slowDown;
        runVelocity = (doMoveX) ? runVelocity : 0;

        // Vertical Movement
        if (capColl.IsTouchingLayers(groundLayer) || capColl.IsTouchingLayers(enemyLayer) && !isGrounded) StartCoroutine(GroundCheckAlpha());
        float freeFalling = (0f < rBody.velocity.y && rBody.velocity.y < 0.001f) ? 0f : rBody.velocity.y;
        dirYFinal = (allowJump && isGrounded) ? jumpForce : freeFalling;
        jumpVelocity = (isAlive) ? dirYFinal : freeFalling;
        jumpVelocity = (doMoveY) ? jumpVelocity : 0;

        rBody.velocity = new Vector2(runVelocity, jumpVelocity);
        deadOnGround = !isAlive && (rBody.velocity == new Vector2(0, 0)) && isGrounded;
    }

    // Grounded Alpha
    private IEnumerator GroundCheckAlpha()
    {
        if (!isGrounded)
        {
            isGrounded = true;
            StartCoroutine(GroundCheckDecay());
            yield return new WaitUntil(() => allowJump);
            yield return null; // Skip 1 frame before turning back to false
            isGrounded = false;
        }
    }

    private IEnumerator GroundCheckDecay()
    {
        yield return new WaitForSeconds(.1f);
        isGrounded = false;
    }




    // ========================================= BATTLING PROPERTIES =========================================
    [Header("BATTLING SETTINGS", order = 3)]
    [SerializeField] protected bool doAttack = false;

    [SerializeField] protected bool hasWeapon = false;
    [SerializeField] protected GameObject weaponGameobject = null;
    [SerializeField] protected Weapon wpnEquipped = null;
    protected WeaponProperties weaponProp = null, atkStatsProp = null;
    [SerializeField] protected WeaponProperties defaultPower = new WeaponProperties();

    protected float totalAtkDamage = 0, totalAtkRange = 0, totalAtkSpeed = 0, totalAtkDelay = 0, totalAtkCrit = 0, totalStamCost = 0, totalKbForce = 0;

    protected int totalAtkCritChance = 0;

    [SerializeField] protected bool doAtkCombo = false;

    protected int curAtkCombo = 1;

    protected int atkFacing; // Updates

    protected enum EntityStateToPlayer { Enemy, Ally, NeutralEvil, NeutralGood }
    [SerializeField] protected EntityStateToPlayer stateToPlayer = EntityStateToPlayer.NeutralGood; // "Enemy", "Ally", "Neutral"

    [SerializeField]
    protected LayerMask enemyLayer, allyLayer;
    [SerializeField] protected Transform attackPoint;

    protected Dictionary<string, Dictionary<string, float>> statsBoost = new Dictionary<string, Dictionary<string, float>>() // statsName: {sourceName: effectParam}
    {
        ["Damage"] = new Dictionary<string, float>(),
        ["AttackRange"] = new Dictionary<string, float>(),
        ["AttackSpeed"] = new Dictionary<string, float>(),
        ["AttackDelay"] = new Dictionary<string, float>(),

        ["CritHit"] = new Dictionary<string, float>(),
        ["CritChance"] = new Dictionary<string, float>(),

        ["WpnStamCost"] = new Dictionary<string, float>(),

        ["KbForce"] = new Dictionary<string, float>(),

        ["MVSpeed"] = new Dictionary<string, float>(),
        ["JumpHeight"] = new Dictionary<string, float>(),

        ["KbReduction"] = new Dictionary<string, float>(),
        ["DmgReduction"] = new Dictionary<string, float>(),
        ["CritReduction"] = new Dictionary<string, float>(),
    };

    protected float TotalBoost(string statsName)
    {
        float totalBoost = 0;
        foreach (KeyValuePair<string, float> eachBoost in statsBoost[statsName])
        {
            totalBoost += eachBoost.Value;
            if (statsName == "DamageReduction" && totalBoost >= 1)
            {
                totalBoost = 1f;
                break;
            }
        }
        return totalBoost;
    }

    public void AddBoost(string statsName, string sourceName, SelfEffectProperties effectProp)
    {
        StartCoroutine(AddStatsBoost(statsName, sourceName, effectProp));
    }

    protected IEnumerator AddStatsBoost(string statsName, string sourceName, SelfEffectProperties effectProp)
    {
        if (statsBoost[statsName].ContainsKey(sourceName)) statsBoost[statsName][sourceName] = effectProp.effectParam; // Overwrites the old same Effect
        else statsBoost[statsName].Add(sourceName, effectProp.effectParam);

        if (effectProp.effectSpeed == "Overtime")
        {
            yield return new WaitForSeconds(effectProp.effectTimer);
            statsBoost[statsName].Remove(sourceName);
        }
    }

    public void SetWeapon(Weapon wpnEquipped = null)
    {
        this.wpnEquipped = wpnEquipped;
        hasWeapon = wpnEquipped != null;
        if (hasWeapon) { this.weaponProp = wpnEquipped.uniqueProp; }
    }

    protected void UpdateStats()
    {
        weaponProp = (wpnEquipped == null) ? defaultPower : wpnEquipped.uniqueProp;

        totalAtkDamage = weaponProp.wpnDamage + TotalBoost("Damage");
        totalAtkRange = weaponProp.wpnAtkRange + TotalBoost("AttackRange");
        totalAtkSpeed = weaponProp.wpnAtkSpeed + TotalBoost("AttackSpeed");
        totalAtkDelay = weaponProp.wpnAtkDelay + TotalBoost("AttackDelay");

        totalAtkCritChance = weaponProp.wpnCritChance + (int)TotalBoost("CritChance");
        totalAtkCrit = weaponProp.wpnAtkCrit + TotalBoost("CritHit");

        totalStamCost = weaponProp.wpnStamCost + TotalBoost("WpnStamCost");

        totalKbForce = weaponProp.wpnKbForce + TotalBoost("KbForce");

        totalKbReduction = TotalBoost("KbReduction");
        totalDmgReduction = TotalBoost("DmgReduction");
        totalCritReduction = TotalBoost("CritReduction");

        int totalWpnDurability = weaponProp.durability;

        atkStatsProp = new WeaponProperties(weaponProp.weaponType, weaponProp.tier, weaponProp.doBreak);
        atkStatsProp.SetValues(totalAtkDamage, totalAtkRange, totalAtkSpeed, totalAtkDelay, totalAtkCritChance, totalAtkCrit, totalStamCost, totalKbForce, totalWpnDurability);
    }

    protected abstract void Attack();




    // ========================================= ANIMATION METHODS =========================================
    protected override void AnimationState()
    {
        base.AnimationState();

        if (!isAlive) return;

        if (!isHurting && !isAttacking && !isDying)
        {
            // Vertical Movement Animation
            //if (jumpVelocity > .99f)
            if (jumpVelocity >= 1f)
            {
                state = MovementAnim.jump;
            }
            //else if (jumpVelocity < -1f)
            else if (jumpVelocity <= -1f)
            {
                state = MovementAnim.fall;
            }
            else if (dirXFacing == 0)
            {
                state = MovementAnim.idle;
            }
            else
            {
                state = MovementAnim.run;
            }
        }

        //spriteFacing = ((dirXFacing > 0f) ? 1 : -1) * spriteDefaultFacing;
        spriteFacing = (dirXFacing == 0f) ? spriteFacing : (int)(dirXFacing / Mathf.Abs(dirXFacing)) * spriteDefaultFacing;
        transform.localScale = new Vector3(spriteFacing, 1, 1);
        runAnimationSpeed = (totalSpeed.x / mvSpeed) * animationSpeed;
        anim.speed = (isAttacking) ? totalAtkSpeed : ((state == MovementAnim.idle) ? runAnimationSpeed : animationSpeed);
        anim.SetInteger("state", (int)state);
    }




    // ========================================= INVENTORY PROPERTIES =========================================
    [Header("INVENTORY SETTINGS", order = 4)]
    [SerializeField] protected bool hasInventory = false;
    [SerializeField] protected Inventory inventory = null;

    protected GameObject[] eqpSlots = new GameObject[] { null, null }; // Weapon, Consumable


    // Initialize at Start (Not Awake)
    protected void InventoryPropInit()
    {
        if (hasInventory)
        {
            inventory = GetComponent<Inventory>();
            eqpSlots[0] = inventory.eqpSlotsCol["Weapon"].eqpSlot;
            eqpSlots[1] = inventory.eqpSlotsCol["Consumable"].eqpSlot;
        }
    }
}