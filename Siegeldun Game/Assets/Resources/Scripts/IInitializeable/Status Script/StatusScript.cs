using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatusScript : MonoBehaviour, IRestrictable, IInitializeable, IJsonable, IRegenable, IDamageable
{
    // ============================== MAIN RESTRICTION ==============================
    public bool paused { get { return GameSystem.paused; } }
    [SerializeField] private bool _allowed;
    public bool allowed => _allowed;
    public bool alive { get; set; }
    public bool initialized { get; private set; }

    public bool IsRestricted() { return paused || !allowed || !initialized || !alive; }


    // ============================== IINITIALIZEABLE ==============================
    public void Init()
    {
        SetAliveToAll(true);
        initialized = true;
        if (IsRestricted()) return;
        PropertyInit();
        ComponentInit();
        StatsInit();

        UIInit();
    }

    private void SetAliveToAll(bool value)
    {
        IInitializeable[] iInits = GetComponent<Entity>().iInits;
        for (int i = 0; i < iInits.Length; i++)
        {
            iInits[i].alive = value;
        }
    }


    // ------------------------------ PROPERTIES ------------------------------
    public int difficulty { get { return GameSystem.difficulty; } }
    protected void PropertyInit()
    {
        state = new State();
        _allowed = true;
    }


    // ------------------------------ COMPONENTS ------------------------------
    private void ComponentInit()
    {

    }


    // ============================== UNITY METHODS ==============================
    public void Update()
    {
        if (IsRestricted()) return;
        StatsUpdate();

        if (Input.GetKeyDown(KeyCode.P))
        {
            TakeDamage(0, 0, 20, 0);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            curSP -= 10;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            HealHP(10);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            HealSP(5);
        }
    }

    public void FixedUpdate()
    {
        if (IsRestricted()) return;

    }

    public void LateUpdate()
    {
        UIUpdate();

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
    public StatusProp baseProp;

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
        statsBase = baseProp.stats[difficulty];
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
        if (curHP != maxHP && lastHealTime.x + baseProp.regenTimeIncrement.x.Positive() <= Time.time)
        {
            if (HealHP(HPRegen))
                lastHealTime.Set(Time.time, lastHealTime.y);
        }
        if (curSP != maxSP && lastHealTime.y + baseProp.regenTimeIncrement.y.Positive() <= Time.time)
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
        SetAliveToAll(false);
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


    // ============================== UI ==============================
    [SerializeField] private Image spBar, hpBarB, hpBarF, shdBar;
    [SerializeField] private TMP_Text hpText, spText;

    private float HPUIChangeTickCount, SPUIChangeTickCount;
    [SerializeField] private float UIPercentChangeSpeed, UIPercentChangeAcceleration;

    private void UIInit()
    {
        hpBarF.fillAmount = (curHP / maxHP).Positive();
        hpBarB.fillAmount = hpBarF.fillAmount;
        shdBar.fillAmount = (shd / maxHP).Positive();

        HPUIChangeTickCount = 0;
        SPUIChangeTickCount = 0;
    }

    private void UIUpdate()
    {
        HPBarUI();
        SPBarUI();
    }

    private void HPBarUI()
    {
        if (hpBarF == null || hpBarB == null || shdBar == null) return;

        float fillFractionShd = (shd / maxHP).Clamp(0, 1);
        float fillFractionHP = (curHP / maxHP).Clamp(0, 1);

        float fillHPBarF = hpBarF.fillAmount;
        float fillHPBarB = hpBarB.fillAmount;
        float fillShdBar = shdBar.fillAmount;

        // SHDBAR Changes
        if (fillShdBar != fillFractionShd)
        {
            shdBar.fillAmount = fillFractionShd;
        }

        // HPBARF Changes
        if (fillHPBarF != fillFractionHP)
        {
            hpBarF.fillAmount = fillFractionHP;
        }

        // HPBARB Changes
        if (fillHPBarB != fillHPBarF)
        {
            if (fillHPBarF < fillHPBarB)
                hpBarB.fillAmount -= (UIPercentChangeSpeed * Mathf.Pow(UIPercentChangeAcceleration, HPUIChangeTickCount)).Positive();
            if (fillHPBarF > fillHPBarB)
                hpBarB.fillAmount = fillHPBarF;

            HPUIChangeTickCount++;
        }
        else if (HPUIChangeTickCount != 0)
        {
            HPUIChangeTickCount = 0;
        }

        if (hpText == null) return;
        hpText.text = $"{Mathf.FloorToInt(curHP)} / {Mathf.FloorToInt(maxHP)}";
        if (shd > 0)
            hpText.text = $"({hpText.text}) + {Mathf.FloorToInt(shd)}";
    }

    private void SPBarUI()
    {
        if (spBar == null) return;

        float fillFractionSP = (curSP / maxSP).Positive();
        float fillSPBar = spBar.fillAmount;

        // HPBARB Changes
        if (fillSPBar != fillFractionSP)
        {
            if (fillFractionSP < fillSPBar)
                spBar.fillAmount -= (UIPercentChangeSpeed * Mathf.Pow(UIPercentChangeAcceleration, SPUIChangeTickCount)).Positive();
            if (fillFractionSP > fillSPBar)
                spBar.fillAmount = fillFractionSP;

            SPUIChangeTickCount++;
        }
        else if (SPUIChangeTickCount != 0)
        {
            SPUIChangeTickCount = 0;
        }

        if (spText == null) return;
        spText.text = $"{Mathf.FloorToInt(curSP)} / {Mathf.FloorToInt(maxSP)}";
    }





    // ============================== JSON ==============================
    public JsonData BasePropToJson() => new JsonData(baseProp.GetType().ToString(), baseProp.ToJson());

    public void SetBaseProp(string baseProp)
    {
        this.baseProp = JsonUtility.FromJson<StatusProp>(baseProp);
    }
}
