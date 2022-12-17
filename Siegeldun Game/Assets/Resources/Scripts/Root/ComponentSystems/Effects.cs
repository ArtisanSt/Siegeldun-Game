using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Effects
{
    public enum EffectNames
    {
        Healing, Bouncy
    }
    public EffectNames effectName;

    public string name { get { return effectName.ToString(); } }
    public string id { get; private set; }
    public bool allow = false;
    public float value = 0f;
    // public Sprite icon;

    public int count = 0;
    public float time = 0f;
    public bool infinite { get { return time <= 0; } }

    public string description;

    public enum ExpirationType { Infinite, Timed, Count, TimedCount }
    public ExpirationType expirationType
    {
        get
        {
            if (infinite) return ExpirationType.Infinite;
            else if (count > 1 && time > 0f) return ExpirationType.TimedCount;
            else if (count > 1) return ExpirationType.Count;
            else return ExpirationType.Timed;
        }
    }
    public enum ValueType { Boolean, Percent, Amount }
    public ValueType valueType;

    public enum Classification { Buff, Debuff }
    public Classification classification
    {
        get
        {
            bool isBuff = (value == 0) ? allow : value > 0;
            return isBuff ? Classification.Buff : Classification.Debuff ;
        }
    }

    public List<Effects> subEffects;


    public Effects(EffectNames effectName, bool allow = false, float time = 0f, float value = 0f, int count = 0)
    {
        this.effectName = effectName;
        this.allow = allow;
        this.time = time;
        this.value = value;
        this.count = count;
        id = "";
    }

    private void DescriptionConfigure()
    {
        foreach ( System.Reflection.FieldInfo fieldInfo in typeof(Effects).GetFields())
        {
            description.Replace($"${fieldInfo.Name}$", fieldInfo.GetValue(this).ToString());
        }
    }

    private void SubEffectConfigure()
    {
        if (subEffects == null)
            subEffects = new List<Effects>();

        foreach (Effects effect in subEffects)
        {
            effect.allow = allow;
            effect.time = time;
            effect.count = count;
        }
    }

    public void Use(IEffectable target)
    {
        DescriptionConfigure();
        if (id.Trim().Length == 0)
            id = System.DateTime.Now.ToString("G");

        target.AddEffect(this);
    }

    public void Use(GameObject target)
    {
        if (target == null) return;
        Use(target.GetComponent<IEffectable>());
    }


    /*
     * EFFECTS:
     * DEBUFFS
     *  
     *  heavyweight : jump height lowered
     *  weaken : basic attack down
     *  slowed : movement speed down
     *  lazy : attack speed up
     *  suffer : weaken, slowed, lazy
     *  
     *  
     *  
     *  
     *  stucked : movement is disabled
     *  silenced : skill is disabled
     *  unarmed : basic attack is disabled
     *  stunned : stucked, silenced, unarmed
     *  slept : slowed, stunned (sleep animation)
     *  frozen : slowed, stunned (freeze animation)
     *  
     *  concussed : lowered attack speed (concussed animation)
     *  chilled : attack speed down, slowed (chilled animation)
     *  
     *  poisoned : continuous damage for n seconds (poisoned animation)
     *  burned : continuous damage for n seconds (burned animation)
     *  
     *  soften : Armor down
     *  
     *  Blinded : Black cover layer on the screen
     *  
     *  condemn : clear all buffs
     *  
     *  
     *  
     *  
     *  BUFFS
     *  
     *  lightweight : jump higher
     *  strengthen : basic attack up
     *  haste : movement speed up, 
     *  agile : attack speed
     *  raged : strength, haste, agile
     *  
     *  healing : hp increasing
     *  refreshing : sp increasing
     *  
     *  healthy : hp max up
     *  endurance : sp regen up
     *  
     *  Toughen : Armor Up
     *  
     *  Illuminate : Light
     *  
     *  
     *  cleanse : clear all debuffs
     *  
     *  
     *  bouncy : multiple jump
     *  
     *  Fatal Damage : Deals an amount of damage that will kill the enemy
     *  True Damage : Amount of damage that ignores Armor
     *  
    */
}

