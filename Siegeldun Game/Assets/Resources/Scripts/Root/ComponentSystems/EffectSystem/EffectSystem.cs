using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EffectData;


public class EffectSystem : UserComponents//, IEffectReceiver, IEffectRunner
{
    // ============================== UNITY METHODS ==============================
    // Should be initialized by an entity
    public void Init(EffectProp effectProp)
    {
        this.effectProp = effectProp;
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
    private EffectProp effectProp;

    protected override void PropertyInit()
    {
        if (!IsRestricted()) return;

        state = new State();
        allow = true;
    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    // ------------------------------ EFFECT RESTRICTIONS ------------------------------
    public struct State
    {

    }
    public State state { get; private set; }


    // ------------------------------ EFFECT PROPERTIES ------------------------------


    // ------------------------------ EFFECT METHODS ------------------------------












    /*


    // ============================== IEffectReceiver ==============================
    public Dictionary<Effect.Name, Effect> effects { get; private set; }
    public Dictionary<Effect.Name, int> immunities { get; private set; } //Immunity and count

    public void Receive(List<Effect> receivedEffects)
    {
        foreach (Effect effect in receivedEffects)
        {
            if (effects.ContainsKey(effect.name))
            {
                if ((int)effects[effect.name].sourceType > (int)effect.sourceType) continue;
                if (this.immunities.ContainsKey(effect.name)) continue;
                Interrupt(effect.name);
            }

            Invoke(effect);
        }
    }

    private void Interrupt(Effect.Name effectName)
    {
        this.effects[effectName].Interrupt();
        Remove(this.effects[effectName]);
    }

    private void Expire(Effect.Name effectName)
    {
        this.effects[effectName].Expire(); ;
        Remove(this.effects[effectName]);
    }

    private void Remove(Effect effect)
    {
        this.immunities[effect.name] -= 1;
        if (this.immunities[effect.name] == 0) this.immunities.Remove(effect.name);
        this.effects.Remove(effect.name);
    }

    private void Invoke(Effect effect)
    {
        this.effects.Add(effect.name, effect);
        this.effects[effect.name].Init();

        foreach (Effect.Name immunity in effect.immunities)
        {
            if (this.immunities.ContainsKey(immunity))
                this.immunities[immunity] += 1;
            else
                this.immunities[immunity] = 1;
        }

        ActionRunner(effect, Action.InvocationType.OnInvoke);
        IEnumerator effectTimer = StartCoroutine(EffectTimer(effect));
        IEnumerator effectCounter = StartCoroutine(EffectCounter(effect));
        this.effects[effect.name].AddCoroutine(effectTimer, effectCounter);
    }

    private void ActionRunner(Effect effect, Action.InvocationType invocationType)
    {
        foreach (Action action in effect.actions)
        {
            if (action.invocationType != invocationType) continue;

            object value;
            switch (action.valueType)
            {
                case Action.ValueType.Boolean;
                    value = action.boolean;
                    break;

                case Action.ValueType.Percent;
                    value = action.percent;
                    break;

                case Action.ValueType.AmountFloat;
                    value = action.amountFloat;
                    break;

                case Action.ValueType.AmountInt;
                    value = action.amountInt;
                    break;
            }

            Stats temp = new Stats(action.actionType, value);
            this.stats[action.target.ToString()] = temp;
        }
    }

    private IEnumerator EffectTimer(Effect effect)
    {
        yield return new WaitForSeconds(effect.time);
        Expire(effect.name);
    }

    private IEnumerator EffectCounter(Effect effect)
    {
        yield return new WaitUntil(() => effect.count == 0);
        Expire(effect.name);
    }*/
}
