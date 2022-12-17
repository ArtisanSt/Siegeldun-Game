using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IStatusable))]
public class StatusSystem : MonoBehaviour, IEffectable
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
    public bool isAlive = true;
    protected virtual void PropertyInit()
    {
        iStatusable = GetComponent<IStatusable>();
        iMoveable = GetComponent<IMoveable>();
        iBattleable = GetComponent<IBattleable>();
        if (iStatusable == null) return;
        statusProp = iStatusable.statusProp;

        if (iMoveable != null)
            movementProp = iMoveable.movementProp;
        if (iBattleable != null)
            battleProp = iBattleable.battleProp;

        effects = new Dictionary<string, List<Effects>>();

        EffectInit();
    }

    // Communicates with other components
    public void Receiver()
    {

    }

    public Dictionary<string, List<Effects>> effects;

    private void EffectInit()
    {
        List<Effects> temp = new List<Effects>().AddRange(movementProp.passiveAbilities.Get, statusProp.passiveAbilities.Get, battleProp.passiveAbilities.Get);
        foreach (Effects eff in temp)
        {
            AddEffect(eff);
        }
    }

    public void AddEffect(Effects effect)
    {
        List<Effects> target = FindEffect(effect);
        if (IsDuplicate(effect, target)) return;

        target.Add(effect);

        if (effect.infinite) return;
        StartCoroutine(EffectTimer(effect, effect.time));

        if (effect.count > 0) return;
        StartCoroutine(EffectCounter(effect));
    }

    private List<Effects> FindEffect(Effects effect)
    {
        string effectName = effect.name;
        if (!effects.ContainsKey(effectName)) effects.Add(effectName, new List<Effects>());

        return effects[effectName];
    }

    private bool IsDuplicate(Effects effect, List<Effects> target)
    {
        foreach (Effects eff in target)
        {
            if (eff.id == effect.id) return true;
        }
        return false;
    }

    public void RemoveEffect(Effects effect)
    {
        FindEffect(effect).Remove(effect);
    }

    private IEnumerator EffectTimer(Effects effect, float time)
    {
        yield return new WaitForSeconds(time);
        RemoveEffect(effect);
    }

    private IEnumerator EffectCounter(Effects effect)
    {
        yield return new WaitUntil(() => effect.count == 0);
        RemoveEffect(effect);
    }
}
