using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public abstract class Root : MonoBehaviour
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected virtual void Awake()
    {
        ComponentInit();

        /*if (GetComponent<Player>() != null) { player = gameObject; }*/
    }

    protected virtual void Start()
    {

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
    public BoxCollider2D boxColl { get; private set; }

    protected virtual void ComponentInit()
    {
        boxColl = GetComponent<BoxCollider2D>();
    }


    // ============================== OBJECT METHODS ==============================
    /*public static GameObject player = null;*/
}
