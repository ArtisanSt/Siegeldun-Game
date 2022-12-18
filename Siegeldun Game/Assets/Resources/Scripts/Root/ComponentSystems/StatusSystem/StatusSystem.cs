using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IStatusable))]
public class StatusSystem : MonoBehaviour
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
        if (paused) return;

    }

    protected virtual void FixedUpdate()
    {
        if (paused) return;

    }

    protected virtual void LateUpdate()
    {
        if (paused) return;

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


    // ============================== SYSTEM PROPERTIES AND METHODS ==============================
    // Checks if game is paused
    public bool paused
    {
        get
        {
            return false;
        }
    }


    // ============================== COMPONENTS ==============================
    private IEffectable iEffectable;
    private IStatusable iStatusable;
    
    private StatusProp statusProp;

    private void ComponentInit()
    {
        iEffectable = GetComponent<IEffectable>();

        statusProp = iStatusable.statusProp;

    }

    private void MainRestriction()
    {
        iStatusable = GetComponent<IStatusable>();
    }


    // ============================== OVERALL PROPERTIES AND METHODS ==============================
    // Runtime Changing
    // Checks for default life, effect restrictions
    public bool isAlive
    {
        get
        {
            return iStatusable != null;
        }
    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    private void PropertyInit()
    {
        MainRestriction();

        if (!isAlive) return;
        ComponentInit();
    }

    // Communicates with other components
    public void Receiver()
    {

    }
}
