using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    protected StatusProp statusProp;
    public bool isAlive = true;
    protected virtual void PropertyInit()
    {
        statusProp = GetComponent<IStatusable>().statusProp;
        if (statusProp == null) return;
    }

    // Communicates with other components
    public void Receiver(float horizontal, bool jump, bool climb, bool crouch)
    {

    }
}
