using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatusSystem))]
public class MovementSystem : MonoBehaviour
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
        Movement();
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


    }


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    protected MovementProp movementProp;
    protected IMoveable iMoveable;
    public bool allowMovement = false;
    private bool isAlive { get { return GetComponent<StatusSystem>().isAlive; } }

    private BoxCollider2D boxColl;
    private CapsuleCollider2D capColl;
    private Rigidbody2D rbody;
    private Collider2D ground;
    private LayerMask groundLayer;
    private bool groundTriggered = false;
    private bool onGround = false;


    private Vector2 totalSpeed = new Vector2();


    protected virtual void PropertyInit()
    {
        allowMovement = false;
        iMoveable = GetComponent<IMoveable>();
        if (iMoveable == null) return;

        movementProp = iMoveable.movementProp;
        if (movementProp == null) return;

        allowMovement = (movementProp != null);
        boxColl = iMoveable.boxColl;
        capColl = iMoveable.capColl;
        rbody = iMoveable.rbody;
        GameObject groundG = GameObject.Find("Grid/Ground");
        ground = groundG.GetComponent<CompositeCollider2D>();
        groundLayer = groundG.layer;

        totalSpeed = new Vector2(0,0);
    }

    // Controller Shared
    private float horizontal;
    private bool jump;
    private bool climb;
    private bool crouch;

    // Communicates with other components
    public void Receiver(float horizontal, bool jump, bool climb, bool crouch)
    {
        if (!(isAlive && allowMovement)) return;

        this.horizontal = horizontal;
        this.jump = jump;
        this.climb = climb;
        this.crouch = crouch;
    }

    protected void Movement()
    {
        if (!(isAlive && allowMovement))
        {
            totalSpeed.Set(0, 0);
        }

        totalSpeed.Set((crouch) ? movementProp.mvSpeed[0] : movementProp.mvSpeed[1], movementProp.jumpForce);
        float freeFalling = (Mathf.Abs(rbody.velocity.y) < 0.001f) ? 0f : rbody.velocity.y;
        onGround = capColl.IsTouchingLayers(groundLayer);

        totalSpeed.Set(horizontal * totalSpeed.x, (onGround && jump && groundTriggered) ? totalSpeed.y : freeFalling);
        rbody.velocity = totalSpeed;

        groundTriggered = (groundTriggered) ? !(onGround && jump) : groundTriggered ;

        Debug.Log(onGround);
        Debug.Log(groundG.layer);
        Debug.Log(groundTriggered);


        /*totalSpeed.Set(mvSpeed + TotalBoost("MVSpeed"), jumpForce + TotalBoost("JumpHeight"));

        // Horizontal Movement
        float slowDown = (Mathf.Abs(dirXFinal * slowDownConst) > 0.001f) ? dirXFinal * slowDownConst : 0;
        dirXFinal = (!isAttacking && !isHurting && !isDoingAbility) ? dirXFacing : slowDown; // Front movement with a slowdown effect when attacking
        runVelocity = (isAlive) ? inProximity * dirXFinal * totalSpeed.x + rcvKbDisplacement : slowDown;
        runVelocity = (doMoveX) ? runVelocity : 0;

        // Vertical Movement
        if ((capColl.IsTouchingLayers(groundLayer) || capColl.IsTouchingLayers(enemyLayer)) && rBody.velocity.y == 0 && !isGrounded) StartCoroutine(GroundCheckAlpha());
        float freeFalling = (Mathf.Abs(rBody.velocity.y) < 0.001f) ? 0f : rBody.velocity.y;
        dirYFinal = (allowJump && isGrounded) ? jumpForce : freeFalling;
        jumpVelocity = (isAlive) ? dirYFinal : freeFalling;
        jumpVelocity = (doMoveY) ? jumpVelocity : 0;

        rBody.velocity = new Vector2(runVelocity, jumpVelocity);
        deadOnGround = !isAlive && (rBody.velocity == new Vector2(0, 0)) && isGrounded;*/
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll == ground)
            groundTriggered = true;
    }
}
