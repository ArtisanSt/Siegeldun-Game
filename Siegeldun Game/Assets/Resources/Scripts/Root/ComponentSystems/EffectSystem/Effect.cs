using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct EffectsData
{
    public Effect DoubleJump(float time)
    {
        Effect output = new Effect(Effect.EffectTargets.Bouncy);
        output.count = 2;
        output.time = time;
        output.description = ""; // Put Description
        return output;
    }
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

public abstract class BaseEffect
{
    public string name;
    public GameObject source;
    public bool showOnUI;
    public Sprite icon;
    private string _id = "";
    public string id { get { return _id; } protected set { _id = value; } }

    public enum ValueType { Boolean, Percent, Value, Count }
    public ValueType valueType;
    public bool allow = false;
    public float value = 0f;

    public int count = 0;
    public float time = 0f;

    public string description;

    public abstract string target { get; }
    public string title { get { return target.NameToTitle(); } }
    public bool permanent { get { return time <= 0; } }

    public enum ExpirationType { Permanent, Timed, Count, TimedCount }
    public ExpirationType expirationType
    {
        get
        {
            if (permanent) return ExpirationType.Permanent;
            else if (count > 1 && time > 0f) return ExpirationType.TimedCount;
            else if (count > 1) return ExpirationType.Count;
            else return ExpirationType.Timed;
        }
    }

    public enum Classification { Buff, Debuff }
    public Classification classification
    {
        get
        {
            bool isBuff = allow;
            if (valueType == ValueType.Value || valueType == ValueType.Percent)
                isBuff = value >= 0f;
            else if (valueType == ValueType.Count)
                isBuff = count > 0;
            return isBuff ? Classification.Buff : Classification.Debuff;
        }
    }

    public enum PriorityLevel { Worthless = 0, Passive = 1, Essential = 2, Worthy = 3}
    public PriorityLevel priorityLevel;

    public enum CancellationType { Priority, Value }
    public CancellationType cancellationType;

    public enum ActionOnCancel { Destroy, Disable }
    public ActionOnCancel actionOnCancel;

    protected void Init(bool allow = false, float time = 0f, float value = 0f, int count = 0)
    {
        this.allow = allow;
        this.time = time;
        this.value = value;
        this.count = count;
        id = "";
    }

    protected void DescriptionConfiguration()
    {
        foreach (System.Reflection.FieldInfo fieldInfo in typeof(Effect).GetFields())
        {
            description.Replace($"${fieldInfo.Name}$", fieldInfo.GetValue(this).ToString());
        }
    }

    public abstract void Apply(IEffectable target, string targetID);

    public void Apply(GameObject target)
    {
        if (target == null || target.GetComponent<IEffectable>() == null) return;
        Apply(target.GetComponent<IEffectable>(), target.GetInstanceID().ToString());
    }

    protected virtual void CreateID()
    {
        if (id.Trim().Length == 0)
            id = $"{name}({UniqueID()})";
    }

    protected string UniqueID()
    {
        return System.DateTime.Now.ToString("G");
    }
}

[System.Serializable]
public class ParentEffect : BaseEffect
{
    public enum EffectTargets
    {
        Suffer
    }
    public EffectTargets effectTarget;
    public override string target { get { return effectTarget.ToString(); } }

    public List<Effect> subEffects;

    public ParentEffect(EffectTargets effectTarget, bool allow = false, float time = 0f, float value = 0f, int count = 0)
    {
        Init(allow, time, value, count);
        this.effectTarget = effectTarget;
    }

    public override void Apply(IEffectable target, string targetID)
    {
        if (target == null || targetID.Trim() == "") return;

        CreateID();
        DescriptionConfiguration();
        foreach (Effect child in subEffects)
        {
            child.AddParent(this);
            child.Apply(target, targetID);
        }
    }

}

[System.Serializable]
public class Effect : BaseEffect
{
    public enum EffectTargets
    {
        Healing, Bouncy, Float
    }
    public EffectTargets effectTarget;
    public override string target { get { return effectTarget.ToString(); } }
    private string _parentID;
    public string parentID { get { return _parentID; } }

    public Effect(EffectTargets effectTarget, bool allow = false, float time = 0f, float value = 0f, int count = 0)
    {
        Init(allow, time, value, count);
        this.effectTarget = effectTarget;
    }

    public void AddParent(ParentEffect parent)
    {
        if (parent == null) return;

        foreach(Effect child in parent.subEffects)
        {
            if (child != this) continue;

            this.time = parent.time;
            this.count = parent.count;
            this.priorityLevel = parent.priorityLevel;

            if (_parentID.Trim() == "")
                this._parentID = parent.id;
        }
    }

    public override void Apply(IEffectable target, string targetID)
    {
        if (target == null || targetID.Trim() == "") return;

        CreateID();
        DescriptionConfiguration();
        target.AddEffect(this);
    }

    protected override void CreateID()
    {
        if (id.Trim().Length == 0)
            id = $"{name}-{parentID}";
    }
}

public static class EffectExtensions
{
    private static Dictionary<string, bool> NumberRestrictions(this Effect effect, params string[] target)
    {
        Dictionary<string, bool> output = new Dictionary<string, bool>();

        if (target.Length == 0) target = new string[] { "value", "percent", "time", "count", "boolean" };

        foreach (string x in target)
        {
            if (x.ToLower() == "value")
                output.Add(x.ToLower(), effect.valueType == Effect.ValueType.Value);
            else if (x.ToLower() == "percent")
                output.Add(x.ToLower(), effect.valueType == Effect.ValueType.Percent);
            else if (x.ToLower() == "time")
                output.Add(x.ToLower(), effect.expirationType == Effect.ExpirationType.Timed || effect.expirationType == Effect.ExpirationType.TimedCount);
            else if (x.ToLower() == "count")
                output.Add(x.ToLower(), effect.expirationType == Effect.ExpirationType.Count || effect.expirationType == Effect.ExpirationType.TimedCount);
            else if (x.ToLower() == "boolean")
                output.Add(x.ToLower(), effect.valueType == Effect.ValueType.Boolean);
        }

        return output;
    }

    public static float Total(this List<Effect> effectList, string target)
    {
        float total = 0;
        foreach (Effect effect in effectList)
        {
            Dictionary<string, bool> output = effect.NumberRestrictions(target.ToLower());

            if (output["value"] || output["percent"])
                total += effect.value;
            if (output["time"])
                total += effect.time;
            if (output["count"])
                total += effect.count;
        }
        return total;
    }

    public static bool Boolean(this List<Effect> effectList, bool isAnd = true)
    {
        bool total = false;
        foreach (Effect effect in effectList)
        {
            if (effect.valueType != Effect.ValueType.Boolean) continue;
            if (isAnd)
                total &= effect.allow;
            else
                total |= effect.allow;
        }
        return total;
    }

    public static bool HasInfinite(this List<Effect> effectList)
    {
        foreach (Effect effect in effectList)
        {
            if (effect.permanent) return true;
        }
        return false;
    }

    public static float Max(this List<Effect> effectList, string target)
    {
        float total = 0;
        foreach (Effect effect in effectList)
        {
            Dictionary<string, bool> output = effect.NumberRestrictions(target.ToLower());

            if (output["value"] || output["percent"])
                total.Max(effect.value);
            if (output["time"])
                total.Max(effect.time);
            if (output["count"])
                total.Max(effect.count);
        }
        return total;
    }

    public static float Min(this List<Effect> effectList, string target)
    {
        float total = 0;
        foreach (Effect effect in effectList)
        {
            Dictionary<string, bool> output = effect.NumberRestrictions(target.ToLower());

            if (output["value"] || output["percent"])
                total.Min(effect.value);
            if (output["time"])
                total.Min(effect.time);
            if (output["count"])
                total.Min(effect.count);
        }
        return total;
    }
}

public static class EffectNamesExtensions
{
    public static bool IsEmpty(this List<Effect> effects, out Effect effect)
    {
        bool isEmpty = effects.Count == 0;
        effect = isEmpty ? null : new Effect(effects[0].effectTarget);
        return isEmpty;
    }

    public static bool IsEmpty(this List<Effect> effects, out List<Effect> effect)
    {
        bool isEmpty = effects.Count == 0;
        effect = isEmpty ? null : new List<Effect>();
        return isEmpty;
    }

    public static List<Effect> Classify(this List<Effect> effects, bool isBuff = true)
    {
        if (!effects.IsEmpty(out List<Effect> temp))
        {
            foreach (Effect effect in effects)
            {
                if (isBuff && effect.classification == Effect.Classification.Debuff) continue;
                if (!isBuff && effect.classification == Effect.Classification.Buff) continue;
                temp.Add(effect);
            }
        }
        return temp;
    }

    public static Effect Evaluate(this List<Effect> effects, params Effect[] adds)
    {
        effects.AddRange(adds);
        if (effects.IsEmpty(out Effect temp)) return temp;

        if (effects[0].effectTarget == Effect.EffectTargets.Bouncy) EvalBouncy(effects);

        return temp;
    }

    public static Effect EvalBouncy(List<Effect> effects)
    {
        if (effects.IsEmpty(out Effect temp)) return temp;

        temp.value = effects.Max("value");
        bool permanent = effects.HasInfinite();

        if (!permanent)
        {
            temp.time = effects.Max("time");
        }
        temp.time = (permanent) ? temp.time : effects.Max("time");

        return temp;
    }
}