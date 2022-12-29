using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StatusSystem : UserComponents, IDamageable, IRegenable
{
    // ============================== UNITY METHODS ==============================
    // Should be initialized by an entity
    public void Init(StatusProp statusProp)
    {
        this.statusProp = statusProp;
        PropertyInit();
    }

    public void Update()
    {
        if (IsRestricted()) return;
        StatsUpdate();
    }

    public void FixedUpdate()
    {
        if (IsRestricted()) return;

    }

    public void LateUpdate()
    {
        if (IsRestricted()) return;
        if (curHP <= 0f) Death();
    }

    // When turned disabled
    public void OnDisable()
    {

    }

    // When turned enabled
    public void OnEnable()
    {

    }

    // When scene ends
    public void OnDestroy()
    {

    }

    // ============================== INITIALIZATION ==============================
    private StatusProp statusProp;

    public override bool IsRestricted() { return base.IsRestricted() || !alive; }

    protected override void PropertyInit()
    {
        if (!IsRestricted()) return;
        StatsInit();

        alive = true;
        state = new State();
        allow = true;
    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    // ------------------------------ EFFECT RESTRICTIONS ------------------------------
    public struct State
    {
        public bool stunned; // Cannot regen

        // HP
        public bool wounded; // Unable to heal

        public bool invulnerable; // HP will not receive damage
        public bool healable { get { return !wounded; } }

        // SP
        public bool fatigue;

        public bool canHeal { get { return !fatigue; } }

        // DR
        public bool soften; // Ignores Damage Reduction
        public bool harden; // Ignores Raw Damage

        // Shield
        public bool invincible; // Shield absorbs all types of damage but breaks if damage is greater than shield threshold
        public bool unbreakable; // Shield absorbs all types of damage and will not break
    }
    public State state { get; private set; }


    // ------------------------------ STATUS PROPERTIES ------------------------------
    public StatusProp.Stats statsBase { get; private set; } // Base Stats
    public StatusProp.Stats statsBstd { get; private set; } // Boosted Stats (equipables, potions)
    public StatusProp.Stats statsArmor { get; private set; } // Armor Stats (only for damage reduction)

    public float curHP { get; private set; }
    public float maxHP { get { return statsBase.HP.x * (1 + statsBstd.HP.y) + statsBstd.HP.x; } }
    public float HPRegen { get { return statsBase.HPRegen.x * (1 + statsBstd.HPRegen.y) + statsBstd.HPRegen.x; } }

    public float curSP { get; private set; }
    public float maxSP { get { return statsBase.SP.x * (1 + statsBstd.SP.y) + statsBstd.SP.x; } }
    public float SPRegen { get { return statsBase.SPRegen.x * (1 + statsBstd.SPRegen.y) + statsBstd.SPRegen.x; } }

    private Vector2 lastHealTime; // HP, SP

    public float DR { get { return (statsBase.DR.x + statsBstd.DR.x) * (1 + statsBstd.DR.y); } }

    public float shd { get; private set; }
    public float ResetShield() { return (statsBase.shd.x + statsBstd.shd.x) * (1 + statsBstd.shd.y); }

    public void StatsInit()
    {
        statsBase = statusProp.stats[difficulty];
        statsBstd = new StatusProp.Stats();

        curHP = maxHP;
        curSP = maxSP;
        shd = ResetShield();
        lastHealTime = new Vector2(-99f, -99f);
    }

    public void StatsUpdate()
    {
        Regenerate();
    }


    // ------------------------------ STATUS METHODS ------------------------------
    public bool HealHP(float amount)
    {
        curHP = (curHP + amount).Clamp(0, maxHP);
        return true;
    }

    public bool HealSP(float amount)
    {
        curSP = (curSP + amount).Clamp(0, maxSP);
        return true;
    }

    public void Regenerate()
    {
        if (lastHealTime.x + statusProp.regenTimeIncrement.x.Positive() <= Time.time)
        {
            if (HealHP(HPRegen))
                lastHealTime.Set(Time.time, lastHealTime.y);
        }
        if (lastHealTime.y + statusProp.regenTimeIncrement.y.Positive() <= Time.time)
        {
            if (HealSP(SPRegen))
                lastHealTime.Set(lastHealTime.x, Time.time);
        }
    }


    // ------------------------------ IDamageable ------------------------------
    public bool TakeDamage(float trDmg, float prgDmg, float ttlDmg, float ftlDmg)
    {
        if (IsRestricted()) return false;

        float dmg = (ttlDmg - DR).Positive();
        float dmgToShd = trDmg + prgDmg + dmg;
        float leftShd = (shd - dmgToShd).Positive(); // Left Shield when damage is received

        if (state.invulnerable || state.invincible || state.unbreakable)
        {
            if (state.invulnerable)
            {
                shd = leftShd;
                EffectUsed("invulnerable");
            }
            else if (state.invincible)
            {
                if (leftShd <= 0) ShieldBroken();
                EffectUsed("invincible");
            }
            else if (state.unbreakable)
            {
                EffectUsed("unbreakable");
            }
            return false;
        }

        float shdOverflow = ((prgDmg + dmg) - (shd - trDmg).Positive()).Positive(); // damage that are not absorbed by the shield
        float leftHP = (curHP - (trDmg + shdOverflow)).Positive(); // True damage damages both shield and HP
        shd = leftShd;
        curHP = (leftHP.Floor() - ftlDmg <= 0) ? 0f : leftHP.Positive();

        return true;
    }

    public void Death()
    {
        alive = false;
    }

    // Resurrection but in different body (using stones)
    public void Reshape()
    {

    }

    // Resurrection but in the same body
    public void Resurrect()
    {

    }


    // ------------------------------ Methods ------------------------------
    public void EffectUsed(string effectName)
    {
        // Temporary
        List<string> effectNames = new List<string>() { "unbreakable", "invincible" };
        if (!effectNames.Contains(effectName)) return;
    }

    public void ShieldBroken()
    {
        shd = 0;
    }
}
