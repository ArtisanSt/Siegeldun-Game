using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

[RequireComponent(typeof(AnimationSystem))]
[RequireComponent(typeof(Controller))]
public abstract class Entity<T> : Root where T: EntityProp
{
    // ============================== UNITY METHODS ==============================
    // When this script is loaded
    protected override void Awake()
    {
        base.Awake();

    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

    }

    // When turned disabled
    protected override void OnDisable()
    {
        base.OnDisable();

    }

    // When turned enabled
    protected override void OnEnable()
    {
        base.OnEnable();

    }

    // When scene ends
    protected override void OnDestroy()
    {
        base.OnDestroy();

    }


    // ============================== COMPONENTS ==============================
    protected SpriteRenderer sprRndr;
    protected Rigidbody2D rbody;
    protected CircleCollider2D cirCol;

    protected override void ComponentInit()
    {
        base.ComponentInit();

        sprRndr = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        cirCol = GetComponent<CircleCollider2D>();


    }


    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public T entityProp;
    public string parentPath { get { return entityProp.parentPath; } }
    public Transform parentT { get { return transform.Find(parentPath); } }

    /*public T entityProp<T>(T entityProp) where T:  notnull, EntityProp { return entityProp; }*/


    public enum EntityType { Unit, Structure, Item }
    public abstract EntityType entityType { get; }
    [SerializeField] public string entityNickname;

    public string nickname
    {
        get
        {
            ProjectUtils.Defaults(ref entityNickname);

            if (entityProp == null) return entityNickname;
            else return entityProp.entityName;
        }
    }


    // ============================== DROPS ==============================
    public List<GameObject> itemDrops;
}
