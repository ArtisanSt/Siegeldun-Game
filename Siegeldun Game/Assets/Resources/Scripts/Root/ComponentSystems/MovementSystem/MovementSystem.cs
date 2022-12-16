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
    private bool isAlive { get { return GetComponent<StatusSystem>() ? GetComponent<StatusSystem>().isAlive : false; } } // Updates whenever is called

    private Rigidbody2D rbody;
    private LayerMask groundLayer;
    private LayerMask jumpableLayers;

    [SerializeField] private Transform groundRaycastT;
    [SerializeField] private float mercyJumpDistance = 0f;
    [SerializeField] private float allowedJumpRatio = 0f; // Allowed Jump will always be a fraction of mercy jump distance
    private float allowedJumpDistance { get { return allowedJumpRatio * mercyJumpDistance; } }
    
    private bool mercyJump;
    private int[] jumpCount; // {Current Jump Count, Max Jump Count}



    private Vector2 totalSpeed = new Vector2();

    protected virtual void ComponentInit()
    {
        // Component Init
        rbody = iMoveable.rbody;
    }


    protected virtual void PropertyInit()
    {
        allowMovement = false;
        iMoveable = GetComponent<IMoveable>();
        if (iMoveable == null) return;

        movementProp = iMoveable.movementProp;
        /*if (movementProp == null) return;*/

        /*allowMovement = (movementProp != null);*/
        allowMovement = true;

        ComponentInit();

        // Jump Settings
        int curJumpCount = (movementProp.passiveAbilities.doubleJump) ? 2 : 1;
        jumpCount = new int[2] { curJumpCount, curJumpCount };

        groundRaycastT = transform.GetChild("RaycastGround");

        groundLayer = iMoveable.groundLayer;
        jumpableLayers = iMoveable.jumpableLayers;

        totalSpeed = new Vector2(0,0);
    }

    // Controller Shared
    private float horizontal;
    private bool jump;
    private bool fly;
    private bool climb;
    private bool dash;
    private bool crouch;

    // Communicates with other components
    public void Receiver(float horizontal, bool jump, bool fly, bool climb, bool dash, bool crouch)
    {
        if (!(isAlive && allowMovement)) return;

        this.horizontal = horizontal;
        this.jump = jump;
        this.fly = fly;
        this.climb = climb;
        this.dash = dash;
        this.crouch = crouch;
    }

    protected void Movement()
    {
        if (!(isAlive && allowMovement))
        {
            totalSpeed.Set(0, 0);
        }

        // Jump Restrictions
        bool executeJump = JumpExecution();


        totalSpeed.Set((crouch) ? movementProp.mvSpeed[0] : movementProp.mvSpeed[1], movementProp.jumpForce);
        float freeFalling = (Mathf.Abs(rbody.velocity.y) < 0.001f) ? 0f : rbody.velocity.y;
        totalSpeed.Set(horizontal * totalSpeed.x, (executeJump) ? totalSpeed.y : freeFalling);
        rbody.velocity = totalSpeed;
    }

    private bool JumpExecution()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(groundRaycastT.position, -Vector2.up, mercyJumpDistance, jumpableLayers);
        float rayDistance = (rayHit.collider != null) ? groundRaycastT.position.y - rayHit.point.y : 2 * mercyJumpDistance;

        // Jump Count Reset
        if (rayDistance <= allowedJumpDistance && totalSpeed.y <= 0 && jumpCount[0] != jumpCount[1])
            jumpCount[0] = jumpCount[1];

        System.Func<float, bool> distanceRestriction = jumpDistance => rayDistance <= jumpDistance && totalSpeed.y <= 0;
        bool jumpRestriction = distanceRestriction(allowedJumpDistance) || jumpCount[0] + jumpCount[1] == 3;
        bool executeJump = jumpCount[0] > 0 && jumpRestriction && (jump || mercyJump);

        if (executeJump)
        {
            jumpCount[0]--;
            mercyJump = false;
        }
        else if (!mercyJump) mercyJump = (jump && distanceRestriction(mercyJumpDistance)); // Checks if the player pressed jumped n seconds early
        return executeJump;
    }

    void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundRaycastT.position, new Vector2(groundRaycastT.position.x, groundRaycastT.position.y - mercyJumpDistance));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundRaycastT.position, new Vector2(groundRaycastT.position.x, groundRaycastT.position.y - allowedJumpDistance));
    }
}
