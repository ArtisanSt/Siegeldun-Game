using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
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
    protected SpriteRenderer sprRndr;
    protected Rigidbody2D rbody;
    protected CircleCollider2D cirCol;

    protected virtual void ComponentInit()
    {
        sprRndr = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        cirCol = GetComponent<CircleCollider2D>();
    }


    // ============================== OBJECT METHODS ==============================
    /*public static GameObject player = null;*/
}
