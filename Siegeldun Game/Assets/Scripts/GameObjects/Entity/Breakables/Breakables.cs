using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Breakables: Entity
{
    // ========================================= BEINGS PROPERTIY SCALING =========================================
    private string _entityType = "Breakables";
    public override string entityType { get { return _entityType; } }

    protected enum FreezeOn { Drop, Air }
    [Header("BREAKABLES SETTINGS", order = 0)]
    [SerializeField] protected FreezeOn freezeOn;
    protected bool frozen = false;
    [SerializeField] protected LayerMask groundLayer;

    protected void FreezeObject()
    {
        if (frozen) return;

        if (freezeOn == FreezeOn.Air || (freezeOn == FreezeOn.Drop && boxColl.IsTouchingLayers(groundLayer))) { frozen = true; }
        if (frozen) { GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation; }
    }




    // ========================================= BATTLING METHODS =========================================
    protected virtual void Start()
    {
        deadOnGround = true;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        FreezeObject();
    }
}
