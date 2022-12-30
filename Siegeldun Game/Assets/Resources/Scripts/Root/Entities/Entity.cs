using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]

[RequireComponent(typeof(AnimationSystem))]
[RequireComponent(typeof(Controller))]
public abstract class Entity<TEntityProp> : Root
    where TEntityProp : EntityProp
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
    public SpriteRenderer sprRndr { get; private set; }
    public Rigidbody2D rbody { get; private set; }
    public CircleCollider2D cirColl { get; private set; }

    // No need to call
    protected override void ComponentInit()
    {
        base.ComponentInit();

        sprRndr = GetComponent<SpriteRenderer>();
        rbody = GetComponent<Rigidbody2D>();
        cirColl = GetComponent<CircleCollider2D>();
    }


    // ============================== MAIN PROPERTIES AND METHODS ==============================
    public TEntityProp entityProp;
    public abstract void PropertyInit();
    public string parentPath { get { return entityProp.parentPath; } }
    public Transform parentT { get { return transform.Find(parentPath); } }

    /*public T entityProp<T>(T entityProp) where T:  notnull, EntityProp { return entityProp; }*/


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


    // ============================== INSTANTIATION ==============================
    public virtual void Duplicate(GameObject gameObject)
    {
        Entity<TEntityProp> entity = gameObject.GetComponent<Entity<TEntityProp>>();
    }
}
