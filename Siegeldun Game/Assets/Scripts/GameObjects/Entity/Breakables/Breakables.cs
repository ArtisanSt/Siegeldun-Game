using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Breakables: Entity
{
    // ========================================= BEINGS PROPERTIY SCALING =========================================
    [Header("BREAKABLES SETTINGS", order = 0)]
    [SerializeField] protected bool doFreeze;
    protected enum FreezeOn { Drop, Air }
    [SerializeField] protected FreezeOn freezeOn;
    protected bool frozen = false;
    [SerializeField] protected LayerMask groundLayer;

    protected void FreezeObject()
    {
        if (!doFreeze || frozen) return;

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
        if (pauseMenu.isPaused) return;
        FreezeObject();
    }
}
