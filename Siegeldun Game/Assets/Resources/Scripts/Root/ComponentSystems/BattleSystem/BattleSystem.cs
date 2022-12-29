using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleSystem : UserComponents
{
    // ============================== UNITY METHODS ==============================
    // Should be initialized by an entity
    public void Init(BattleProp battleProp)
    {
        this.battleProp = battleProp;
        PropertyInit();
    }

    public void Update()
    {
        if (IsRestricted()) return;

    }

    public void FixedUpdate()
    {
        if (IsRestricted()) return;

    }

    public void LateUpdate()
    {
        if (IsRestricted()) return;

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
    private BattleProp battleProp;

    protected override void PropertyInit()
    {
        if (!IsRestricted()) return;
        StatsInit();

        state = new State();
        allow = true;
    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    // ------------------------------ EFFECT RESTRICTIONS ------------------------------
    public struct State
    {
        public bool stunned;
        public bool unarmed;

        public bool canAttack { get { return !(stunned || unarmed); } }
    }
    public State state { get; private set; }


    // ------------------------------ BATTLE PROPERTIES ------------------------------
    public BattleProp.Stats statsBase { get; private set; } // Base Stats
    public BattleProp.Stats statsBstd { get; private set; } // Boosted Stats (equipables, potions)
    public BattleProp.Stats statsWpn { get; private set; } // Weapon Stats

    public float dmg { get { return (statsBase.dmg.x * (1 + statsWpn.dmg.y) + statsWpn.dmg.x) * (1 + statsBstd.dmg.y) + statsBstd.dmg.x; } }

    public float prgDmg { get { return (statsWpn.prgDmg.x + statsWpn.prgDmg.y * statsWpn.prgDmg.x) * (1 + statsBstd.prgDmg.y) + statsBstd.prgDmg.x; } }
    public float trDmg { get { return (statsWpn.trDmg.x + statsWpn.trDmg.y * statsWpn.trDmg.x) * (1 + statsBstd.trDmg.y) + statsBstd.trDmg.x; } }
    public float ftlDmg { get { return (statsWpn.ftlDmg.x + statsWpn.ftlDmg.y * statsWpn.ftlDmg.x) * (1 + statsBstd.ftlDmg.y) + statsBstd.ftlDmg.x; } }

    public float critHit { get { return (statsWpn.critHit + statsBstd.critHit).Positive(); } }
    public float critCnc { get { return (statsWpn.critCnc + statsBstd.critCnc).Positive(); } }

    public void StatsInit()
    {
        statsBase = battleProp.stats[difficulty];
        statsBstd = new BattleProp.Stats();
        statsWpn = new BattleProp.Stats();
    }


    // ------------------------------ BATTLE METHODS ------------------------------
    public bool DealDamage(params IDamageable[] iDamageables)
    {
        if (IsRestricted() || !state.canAttack) return false;

        foreach (IDamageable iDamageable in iDamageables)
        {
            iDamageable.TakeDamage(trDmg, prgDmg, TotalDamage(), ftlDmg);
        }

        return true;
    }


    public float TotalDamage()
    {
        return dmg * (1 + (Random.Range(0f, 1f) / critCnc).Floor() * critHit);
    }
}
