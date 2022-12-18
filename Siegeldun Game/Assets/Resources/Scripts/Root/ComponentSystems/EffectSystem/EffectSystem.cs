using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectSystem : MonoBehaviour, IEffectable
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        PropertyInit();
    }

    protected virtual void Update()
    {

    }

    protected virtual void FixedUpdate()
    {

    }

    protected virtual void LateUpdate()
    {

    }

    // When turned disabled
    protected virtual void OnDisable()
    {

    }

    // When turned enabled
    protected virtual void OnEnable()
    {

    }

    // When scene ends
    protected virtual void OnDestroy()
    {

    }


    // ============================== COMPONENTS ==============================
    protected virtual void ComponentChecker()
    {
        /*movementSystem = GetComponent<MovementSystem>();*/

    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    protected IStatusable iStatusable;
    protected IMoveable iMoveable;
    protected IBattleable iBattleable;

    protected StatusProp statusProp;
    protected MovementProp movementProp;
    protected BattleProp battleProp;

    private Dictionary<string, List<Effect>> effectsContainer;

    public Effect Evaluate(Effect.EffectTargets targetEffect)
    {
        List<Effect> effects = effectsContainer[targetEffect.ToString()];

        Effect temp = new Effect(effects[0].effectTarget);

        return temp;
    }

    protected virtual void PropertyInit()
    {
        // Components Initiation
        iStatusable = GetComponent<IStatusable>();
        iMoveable = GetComponent<IMoveable>();
        iBattleable = GetComponent<IBattleable>();

        if (iStatusable != null)
            statusProp = iStatusable.statusProp;
        if (iMoveable != null)
            movementProp = iMoveable.movementProp;
        if (iBattleable != null)
            battleProp = iBattleable.battleProp;

        // Effects Initiation
        effectsContainer = new Dictionary<string, List<Effect>>();

        List<Effect> temp = new List<Effect>().AddRange(movementProp.passiveAbilities.Get, statusProp.passiveAbilities.Get, battleProp.passiveAbilities.Get);
        foreach (Effect eff in temp)
        {
            AddEffect(eff);
        }
    }

    public void AddEffect(Effect effect)
    {
        List<Effect> target = FindEffect(effect);
        if (IsDuplicate(effect, target)) return;

        target.Add(effect);

        if (effect.permanent) return;
        StartCoroutine(EffectTimer(effect, effect.time));

        if (effect.count > 0) return;
        StartCoroutine(EffectCounter(effect));
    }

    private List<Effect> FindEffect(Effect effect)
    {
        string effectTarget = effect.target;
        if (!effectsContainer.ContainsKey(effectTarget)) effectsContainer.Add(effectTarget, new List<Effect>());

        return effectsContainer[effectTarget];
    }

    private bool IsDuplicate(Effect effect, List<Effect> target)
    {
        foreach (Effect eff in target)
        {
            if (eff.id == effect.id) return true;
        }
        return false;
    }

    private bool EffectEvaluation(Effect effect, List<Effect> target)
    {
        bool output = false;

        target.Evaluate();

        return output;
    }

    public void RemoveEffect(Effect effect)
    {
        FindEffect(effect).Remove(effect);
    }

    private IEnumerator EffectTimer(Effect effect, float time)
    {
        yield return new WaitForSeconds(time);
        RemoveEffect(effect);
    }

    private IEnumerator EffectCounter(Effect effect)
    {
        yield return new WaitUntil(() => effect.count == 0);
        RemoveEffect(effect);
    }
}
