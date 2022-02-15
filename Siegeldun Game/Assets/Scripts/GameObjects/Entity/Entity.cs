using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


public abstract class Entity : BaseObject, IDamageable, IRegeneration, IFaceScaling
{
    // ========================================= UNITY PROPERTIES =========================================
    // Component Declaration
    [Header("COMPONENT SETTINGS", order = 0)]
    [SerializeField] protected Rigidbody2D rBody = null;
    [SerializeField] protected SpriteRenderer sprite = null;
    [SerializeField] protected BoxCollider2D boxColl = null;
    [SerializeField] protected CircleCollider2D cirColl = null;
    [SerializeField] protected CapsuleCollider2D capColl = null;
    [SerializeField] protected Animator anim = null;


    protected void ComponentInit()
    {
        rBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        boxColl = GetComponent<BoxCollider2D>();
        cirColl = GetComponent<CircleCollider2D>();
        capColl = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }

    protected virtual void Awake()
    {
        InstanceInit();
        GameMechanicsPropInit();
        EntityPropInit();
        StatusBarPropInit();
    }

    protected virtual void LateUpdate()
    {
        HpBarUIUpdate();
        StamBarUIUpdate();
    }




    // =========================================  ANIMATION PROPERTIES =========================================
    [Header("ANIMATION SETTINGS", order = 1)]
    [SerializeField] protected float animationSpeed = 1f;
    protected float runAnimationSpeed = 1f;
    [SerializeField] protected int spriteDefaultFacing = 1;
    private int _spriteFacing = 1;
    public int spriteFacing { get { return _spriteFacing; } protected set { _spriteFacing = value; } } // Updates
    protected string curAnimStateName; // Updates
    protected string curSpriteName; // Updates

    protected bool isHurting = false;
    protected bool isAttacking = false;
    protected bool isDying = false;


    protected virtual void AnimationState()
    {
        curSpriteName = sprite.sprite.name;
        curAnimStateName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Substring(entityName.Length + 1);
        curAnimStateName = (curAnimStateName.Length >= 6 && curAnimStateName.Substring(0, 6) == "Attack") ? "Attack" : curAnimStateName;
        isHurting = curAnimStateName == "Hurt";
        isAttacking = curAnimStateName == "Attack";
    }




    // ========================================= ENTITY PROPERTIES =========================================
    [Header("ENTITY SETTINGS", order = 2)]
    [SerializeField] public string entityName;
    public enum EntityType { Beings, Breakables }
    [SerializeField] public EntityType entityType;
    public int entityID { get; private set; }

    [SerializeField] public GameObject entityPrefab = null;

    protected bool isPlayer ;

    [SerializeField] public bool hasSpawner = false;
    [SerializeField] protected GameObject spawner = null;
    [SerializeField] protected Transform[] activePoints = new Transform[2] { null, null };

    protected void EntityPropInit()
    {
        ComponentInit();
        entityID = gameObject.GetInstanceID();
        isPlayer = entityName == "Player";
    }

    protected override void PrefabsInit()
    {
        entityPrefab = AssetDatabase.LoadAssetAtPath<UnityEngine.GameObject>($"Assets/Prefabs/EntityPrefabs/{entityName}.prefab");

        if (entityName != "Player")
        {
            gameObject.name = $"{entityName} ({entityID})";
        }
    }

    public void CreatedBySpawner(bool hasSpawner, GameObject spawner, Transform[] activePoints)
    {
        this.hasSpawner = hasSpawner;
        this.spawner = spawner;
        this.activePoints = activePoints;
    }



    // ========================================= STATUS BAR PROPERTIES =========================================
    [Header("STATUS BAR SETTINGS", order = 1)]
    public bool isAlive = true;
    protected bool deadOnGround = true; // Updates

    [SerializeField] protected GameObject entityStatusBar = null;

    [SerializeField] protected bool hasHp = false;
    [SerializeField] protected bool isInvulnerable = false;
    [SerializeField] protected float[] maxHpScaling = new float[3] { 0, 0, 0 };
    [SerializeField] public float curHp = 0;
    [SerializeField] public float maxHp = 0;

    [SerializeField] protected Image hpBarB = null;
    [SerializeField] protected Image hpBarF = null;
    [SerializeField] protected Text hpText = null;

    [SerializeField] protected bool doHpRegen = false;
    [SerializeField] protected float[] hpRegenScaling = new float[3] { 0, 0, 0 };
    [SerializeField] protected float hpRegen = 0;
    [SerializeField] protected float hpRegenDelay = 3;
    protected float _hpRegenStart = 0f;
    protected float _hpTick = 0f;
    protected float _hpHideTime = 0f;


    [SerializeField] protected bool hasStam = false;
    [SerializeField] protected bool isInfStamina = false;
    [SerializeField] protected float[] maxStamScaling = new float[3] { 0, 0, 0 };
    [SerializeField] public float curStam = 0;
    [SerializeField] public float maxStam = 0;

    [SerializeField] protected Image stamBar = null;
    [SerializeField] protected Text stamText = null;

    [SerializeField] protected bool doStamRegen = false;
    [SerializeField] protected float[] stamRegenScaling = new float[3] { 0, 0, 0 };
    [SerializeField] protected float stamRegen = 0;
    [SerializeField] protected float stamRegenDelay = 3;

    // Updates
    protected bool[] isPassiveHealing = new bool[2] { false, false };
    protected float[] passiveHealingIncrement = new float[2] { .1f, .1f };

    protected float curHpHealAmount = 0.1f;
    protected float curStamHealAmount = 0.1f;

    protected float totalDmgReduction = 0, totalCritReduction = 0, totalKbReduction = 0; // In percentage
    protected float _lastAttack; // Updates
    protected float rcvKbDisplacement = 0f; // Updates

    protected Dictionary<string, Dictionary<string, float>> healEffect = new Dictionary<string, Dictionary<string, float>>() // statsName: {sourceName: effectParam}
    {
        ["HP"] = new Dictionary<string, float>(),
        ["Stamina"] = new Dictionary<string, float>(),
    };

    protected void StatusBarPropInit()
    {
        if (hasHp)
        {
            maxHp = maxHpScaling[idxDiff];
            curHp = maxHp;
            if (doHpRegen) { hpRegen = hpRegenScaling[idxDiff]; }
        }

        if (hasStam)
        {
            maxStam = maxStamScaling[idxDiff];
            curStam = maxStam;
            if (doStamRegen) { stamRegen = stamRegenScaling[idxDiff]; }
        }
    }


    protected void HpBarUIUpdate()
    {
        if (!hasHp || !entityStatusBar) return;

        // HP UI Parameters
        float fillF = hpBarF.fillAmount;
        float fillB = hpBarB.fillAmount;
        float hpFraction = curHp / maxHp;

        // Health Bar UI Updater
        if (fillB != fillF || fillF != hpFraction || fillB != hpFraction)
        {
            _hpHideTime = Time.time;
            if (!isPlayer) entityStatusBar.SetActive(true);

            float netRegenF = hpFraction - fillF;
            float netRegenB = fillF - fillB;
            float percentChangeB, percentChangeF;

            if (fillF > hpFraction) // When Front Health Bar is greater than the real health
            {
                hpBarF.color = Color.cyan;
                hpBarF.fillAmount = hpFraction;
            }
            else if (fillF < hpFraction) // When Front Health Bar is lower than the real health
            {
                hpBarF.color = Color.green;
                percentChangeF = netRegenF / 2;
                hpBarF.fillAmount += Mathf.Lerp(0, netRegenF, percentChangeF);
            }

            if (fillB > fillF) // When Back Health Bar is greater than Front Health Bar
            {
                _hpTick += 0.75f;
                hpBarB.color = Color.red;
                percentChangeB = (-0.001f / netRegenB) * _hpTick;
                hpBarB.fillAmount += Mathf.Lerp(0, netRegenB, percentChangeB);
            }
            else if (fillB < fillF)// When Back Health Bar is lower than Front Health Bar
            {
                _hpTick = 0f;
                percentChangeB = curHpHealAmount / (maxHp * netRegenB);
                hpBarB.fillAmount = fillF;
            }
        }
        else
        {
            _hpTick = 0f;
            hpBarF.color = Color.cyan;
            if (!isPlayer && TimerIncrement(_hpHideTime, 5f))
            {
                entityStatusBar.SetActive(false);
            }
        }

        // HP UI Text
        hpText.text = $"{Mathf.FloorToInt(curHp)} / {Mathf.FloorToInt(maxHp)}";
    }

    protected void StamBarUIUpdate()
    {
        if (!hasStam || !entityStatusBar) return;

        // Stamina UI Parameters
        float fillS = stamBar.fillAmount;
        float stamFraction = curStam / maxHp;

        // Stamina Bar UI Updater
        if (fillS != stamFraction)
        {
            if (fillS > stamFraction)
            {
                stamBar.fillAmount = stamFraction;
            }
            else if (fillS < stamFraction)
            {
                float netRegenS = stamFraction - fillS;
                float percentChangeS = curStamHealAmount / (maxHp * netRegenS);
                stamBar.fillAmount += Mathf.Lerp(0, netRegenS, percentChangeS);
            }
        }
        else
        {
            stamBar.color = Color.yellow;
        }

        // Stamina UI Text
        stamText.text = $"{Mathf.FloorToInt(curStam)} / {Mathf.FloorToInt(maxHp)}";
    }

    protected void PassiveSkills()
    {
        if (isAlive)
        {
            if (doHpRegen)
            {
                if (curHp < maxHp && !isHurting && !isPassiveHealing[0])
                {
                    if (TimerIncrement(_hpRegenStart, hpRegenDelay))
                    {
                        PassiveHealer("HP", hpRegen, "Instant");
                    }
                }
                else
                {
                    _hpRegenStart = Time.time;
                }
            }

            if (doStamRegen)
            {
                if (curStam < maxStam && !isPassiveHealing[1])
                {
                    PassiveHealer("Stamina", hpRegen, "Instant");
                }
            }
        }
    }

    private void PassiveHealer(string hp_stam, float healAmount, string healSpeed)
    {
        Regenerate(hp_stam, healAmount, healSpeed);
        PassiveHealing(hp_stam);
    }

    protected IEnumerator PassiveHealing(string hp_stam)
    {
        int idx = (hp_stam == "HP") ? 0 : 1;
        isPassiveHealing[idx] = true;
        yield return new WaitForSeconds(passiveHealingIncrement[idx]);
        isPassiveHealing[idx] = false;
    }

    public void Regenerate(string hp_stam, float healAmount, string healSpeed, float timeHeal = 0f, float timeSkip = 0.1f)
    {
        bool canHeal = (hp_stam == "HP") ? hasHp && doHpRegen : ((hp_stam == "Stamina") ? hasStam && doStamRegen : false);

        if (canHeal && isAlive)
        {
            if (healSpeed == "Overtime")
            {
                StartCoroutine(HealOvertime(hp_stam, healAmount, timeHeal, timeSkip));
            }
            else if (healSpeed == "Instant")
            {
                if (hp_stam == "HP" && curHp < maxHp)
                {
                    curHp += healAmount;
                    curHp = Mathf.Clamp(curHp, 0, maxHp);
                }
                else if (hp_stam == "Stamina" && curStam < maxStam)
                {
                    curStam += healAmount;
                    curStam = Mathf.Clamp(curStam, 0, maxStam);
                }
            }
        }
    }

    protected IEnumerator HealOvertime(string hp_stam, float healAmount, float timeHeal, float timeSkip)
    {
        float curTime = 0f;
        bool isHealing = true;
        while (isHealing)
        {
            if (hp_stam == "HP")
            {
                isHealing = curHp < maxHp && !isHurting && curTime < timeHeal;
            }
            else if (hp_stam == "Stamina")
            {
                isHealing = curStam < maxStam && !isHurting && curTime < timeHeal;
            }
            Regenerate(hp_stam, healAmount, "Instant");

            yield return new WaitForSeconds(timeSkip);
            curTime += timeSkip;
        }
    }

    // Attack Receiving Decreaser
    private void DamageEvaluator(ref WeaponProperties rcvStatsProp)
    {
        float newKb = rcvStatsProp.wpnKbForce * (1f - totalKbReduction);
        float newDmg = rcvStatsProp.wpnDamage * (1f - totalDmgReduction);
        float newCrit = rcvStatsProp.wpnAtkCrit * (1f - totalCritReduction);

        rcvStatsProp.ChangeValues(wpnDamage: newDmg, wpnKbForce: newKb, wpnAtkCrit: newCrit);
    }

    public void TakeDamage(float attackID, int rcvKbDir, WeaponProperties rcvStatsProp)
    {

        DamageEvaluator(ref rcvStatsProp);
        bool doesDamage = isAlive && !isInvulnerable && rcvStatsProp.wpnDamage > 0 && ProcessEvaluator((float)attackID, rcvStatsProp.wpnAtkSpeed);

        if (!doesDamage) return;
        bool isCrit = ChanceRandomizer(rcvStatsProp.wpnCritChance);

        float damage = ((rcvStatsProp.wpnDamage >= Mathf.Floor(curHp)) ? curHp : rcvStatsProp.wpnDamage) + ((isCrit) ? rcvStatsProp.wpnAtkCrit : 0);
        curHp -= damage;

        _hpRegenStart = Time.time;
        if (curHp == 0f)
        {
            Die();
        }
        else
        {
            if (entityType.ToString() == "Beings")
            {
                if (isCrit)
                {
                    _lastAttack = Time.time;
                    anim.SetTrigger("hurt");
                    Knockback(rcvStatsProp.wpnKbForce * rcvKbDir);
                }
            }
        }
    }

    protected void Knockback(float rcvKbDisplacement)
    {
        this.rcvKbDisplacement = rcvKbDisplacement;
        StartCoroutine(KbTimer());
    }

    protected IEnumerator KbTimer()
    {
        yield return new WaitForSeconds(.1f);
        rcvKbDisplacement = 0f;
    }

    protected virtual void Die()
    {
        isAlive = false;
        anim.SetBool("death", true);
        InstanceDestroyed(objectName, gameObject);

        StartCoroutine(ClearInstance());
    }

    protected IEnumerator ClearInstance(int time = 1)
    {
        yield return new WaitUntil(() => deadOnGround);
        if (boxColl.enabled)
        {
            if (entityType.ToString() == "Beings")
            {
                GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionY;
            }
            boxColl.enabled = false;
            if (capColl != null) capColl.enabled = false;
            if (cirColl != null) cirColl.enabled = false;

            StartCoroutine(DestroyInstance(time));
        }
    }

    private IEnumerator DestroyInstance(int time = 1)
    {
        StartCoroutine(OnDestroyDrop());
        yield return new WaitForSeconds(time);
        OnEntityDestroy();
        Destroy(gameObject);
    }

    private IEnumerator OnDestroyDrop()
    {
        yield return new WaitForSeconds(onDestroyDropDelay);
        GameObject curItem = Drop(dropChance, new Vector2(0, 0));
    }

    protected virtual void OnEntityDestroy() { }
}
