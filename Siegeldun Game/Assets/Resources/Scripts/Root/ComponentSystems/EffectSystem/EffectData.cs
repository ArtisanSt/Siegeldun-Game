using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace EffectData
{
    /*
    [System.Serializable]
    public struct Stats
    {
        public bool boolean;
        public float percent;
        public float amountFloat;
        public int amountInt;

        public Stats(bool boolean = false, float percent = 0f, float amountFloat = 0f, int amountInt = 0)
        {
            this.boolean = boolean;
            this.percent = percent;
            this.amountFloat = amountFloat;
            this.amountInt = amountInt;
        }
    }

    [System.Serializable]
    public struct Action
    {
        public enum Target
        {
            healthPoints, staminaPoints, healthRegen, staminaRegen,
            jumpCount, jumpHeight, movementSpeed,
            attackDamage, attackSpeed
        }
        public Target target;

        public Stats stats;

        public enum InvocationType { OnInvoke, TimeIncrements }
        public InvocationType invocationType;
        public float increments; // seconds to wait before next invoke

        public IEnumerator caller;
    }

    [System.Serializable]
    public struct Effect
    {
        public enum Name
        {
            HealthUp, StaminaUp, HealthRegen, StaminaRegen,
            DoubleJump
        }
        public Name name;

        public string id { get; private set; }
        public bool enabled { get; private set; }

        // CharacterPassive > EquipablePassive > ConsumablePassive
        public enum SourceType { CharacterPassive = 2, EquipablePassive = 1, ConsumablePassive = 0 }
        public SourceType sourceType;
        public GameObject sourceG;
        public bool showOnUI;
        public Sprite icon;

        public enum ExpirationType { Permanent, Timed, Count, TimedCount }
        public ExpirationType expirationType;
        public float time;
        public int count;
        public bool isPermanent { get { return expirationType == ExpirationType.Permanent; } }

        public string description;
        public enum Classification { Buff, Debuff }
        public Classification classification;

        public List<Effect.Name> immunities;
        public List<Action> actions;

        public List<IEnumerator> coroutines { get; private set; }

        public Effect(Name _name, List<Action> _actions)
        {
            name = _name;
            actions = _actions;
        }

        public void Init(int gameObjectID)
        {
            CreateID(gameObjectID);

            showOnUI = icon == null ? false : true; // If icon is null then false

            this.coroutines = new List<IEnumerator>();
            enabled = true;
        }

        public void AddCoroutine(params IEnumerator[] coroutines)
        {
            this.coroutines.AddRange(coroutines);
        }

        public bool CreateID(int gameObjectID)
        {
            if (id.Trim() == "")
                id = $"{name}({UniqueID()})[{gameObjectID}]";
            return id != "";
        }

        private string UniqueID()
        {
            return System.DateTime.Now.ToString("G");
        }

        public Action GetAction(Action.Target target)
        {
            foreach (Action action in actions)
            {
                if (action.target == target) return action;
            }
            throw new System.ArgumentException("Target not found!", nameof(target));
        }
    }

    [System.Serializable]
    public struct EffectsTransmitter
    {
        public List<Effect> effects;

        public void Transmit(params GameObject[] targetsG)
        {
            foreach (GameObject targetG in targetsG)
            {
                if (targetG == null || targetG.GetComponent<IEffectReceiver>() == null) continue;
                targetG.GetComponent<IEffectReceiver>().effectsReceiver.Receive(effects);
            }
        }

        public void Transmit(params IEffectReceiver[] targets)
        {
            foreach (IEffectReceiver target in targets)
            {
                target.effectsReceiver.Receive(effects);
            }
        }
    }

    public class EffectsContainer
    {
        private List<Effect> _effects;
        public Dictionary<Action.Target, List<string>> effects { get; private set; } // Action.Target , List of effect ID
        public Dictionary<Action.Target, List<string>> immunities { get; private set; } // Action.Target , List of effect ID

        public EffectsContainer()
        {
            _effects = new List<Effect>();
            effects = new Dictionary<Action.Target, List<string>>();
            immunities = new Dictionary<Action.Target, List<string>>();
        }

        public void AddEffect(List<Effect> newEffects)
        {
            foreach (Effect effect in newEffects)
            {
                if (_effects.ContainsKey(effect.name))
                {
                    if ((int)effects[effect.name].sourceType > (int)effect.sourceType) continue;
                    if (this.immunities.ContainsKey(effect.name)) continue;
                    Interrupt(effect.name);
                }

                Invoke(effect);
            }
        }

        public void AddEffect(Effect effect)
        {

        }

        public void ExchangeEffect(Effect oldEffect, Effect newEffect)
        {

        }

        public void RemoveEffect(Effect effect)
        {

        }

        public bool Evaluate(Action.Target target, out Stats stats)
        {
            if (!effects.ContainsKey(target)) return false;

            FindEffect(effects[target][0], out Effect effect);
            stats = effect.GetAction(target);
            if (effects[target].Count == 1) return true;

            for (int i = 1; i < effects[target].Count; i++)
            {


                if (_effects[i].id == effectID)
                {
                    effect = _effects[i];
                    return i;
                }
            }
        }

        public bool FindEffect(string effectID, out Effect effect)
        {
            foreach (Effect eff in _effects)
            {
                if (eff.id == effectID)
                {
                    effect = eff;
                    return true;
                }
            }
            return false;
        }

        public bool FindEffect(Effect.Name effectName, out Effect effect)
        {
            foreach (Effect eff in _effects)
            {
                if (eff.name == effectName)
                {
                    effect = eff;
                    return true;
                }
            }
            return false;
        }
    }

    public interface IEffectTransmitter
    {
        public void Transmit(params GameObject[] targetsG);
        public void Transmit(params IEffectReceiver[] targets);
    }

    public interface IEffectReceiver
    {

    }

    public interface IEffectRunner
    {

    }
    
    public static class Collections
    {
        public static List<string> Methods
        {
            get
            {
                return (from methodInfo in typeof(Collections).GetMethods()
                        where methodInfo.Name != "Get"
                        select methodInfo.Name).ToList();
            }
        }

        public static Effect Get(string methodName, object[] parameters)
        {
            if (!Methods.Contains(methodName)) throw new System.ArgumentException($"Method name not found: {methodName}", nameof(methodName));

            return (Effect)typeof(Collections).GetMethod(methodName).Invoke(null, parameters);
        }
    }

    public static class EffectExtensions
    {
        public static Effect.ExpirationType ConfigureExpirationType(this Effect effect)
        {
            if (effect.time <= 0f)
            {
                if (effect.count > 0) return EffectData.Effect.ExpirationType.Count;
                return Effect.ExpirationType.Permanent;
            }
            if (effect.count > 0) return EffectData.Effect.ExpirationType.TimedCount;
            return Effect.ExpirationType.Timed;
        }

        public static int FindIndex(this List<Effect> effects, string effectName)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].name.ToString() == effectName) return i;
            }
            return -1;
        }
    }*/
}