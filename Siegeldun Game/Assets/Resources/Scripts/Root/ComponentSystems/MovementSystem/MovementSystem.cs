using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IMoveable))]
public class MovementSystem : MonoBehaviour, IControllable
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

        Movement();
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

    private LayerMask groundLayer;

    private void SystemInit()
    {
        groundLayer = iMoveable.groundLayer; // Change to get to system
    }


    // ============================== COMPONENTS ==============================
    private IEffectable iEffectable;
    private IMoveable iMoveable;

    private MovementProp movementProp;

    private Rigidbody2D rbody;

    private Transform groundRaycastT;

    private LayerMask jumpableLayers;

    private void ComponentInit()
    {
        iEffectable = GetComponent<IEffectable>();

        movementProp = iMoveable.movementProp;

        rbody = iMoveable.rbody;
        jumpableLayers = iMoveable.jumpableLayers;

        groundRaycastT = transform.GetChild("RaycastGround");
        Debug.Log(groundRaycastT);
    }

    private void MainRestriction()
    {
        iMoveable = GetComponent<IMoveable>();
    }


    // ============================== INITIALIZATION ==============================

    private void PropertyInit()
    {
        MainRestriction();

        if (!allowMovement) return;
        ComponentInit();
        SystemInit();

        // Overall 
        totalSpeed = new Vector2(0, 0);

        // Jump Settings
        int curJumpCount = (movementProp.passiveAbilities.doubleJump.allow) ? 2 : 1;
        jumpCount = new int[2] { curJumpCount, curJumpCount };
    }


    // ============================== ICONTROLLABLE ==============================
    private float horizontal;
    private bool jump;
    private bool fly;
    private float vertical;
    private bool dash;
    private bool crouch;

    // Communicates with other components
    public void Receiver(Dictionary<string, object> controls)
    {
        if (!allowMovement) return;

        this.horizontal = (float)controls[nameof(horizontal)];
        this.jump = (bool)controls[nameof(jump)];
        this.fly = (bool)controls[nameof(fly)];
        this.vertical = (float)controls[nameof(vertical)];
        this.dash = (bool)controls[nameof(dash)];
        this.crouch = (bool)controls[nameof(crouch)];
    }


    // ============================== OVERALL PROPERTIES AND METHODS ==============================
    // Runtime Changing
    // Checks for default movement, effect restrictions
    public bool allowMovement
    {
        get
        {
            return iMoveable != null;
        }
    }

    private Vector2 totalSpeed = new Vector2();

    private void Movement()
    {
        totalSpeed.Set(0, 0);
        if (!allowMovement) return;
        totalSpeed.Set((crouch) ? movementProp.mvSpeed[0] : movementProp.mvSpeed[1], movementProp.jumpForce);

        // Jump Restrictions
        bool executeJump = JumpExecution();

        // Overall Velocity Calculation
        float freeFalling = (Mathf.Abs(rbody.velocity.y) < 0.001f) ? 0f : rbody.velocity.y;
        totalSpeed.Set(horizontal * totalSpeed.x, (executeJump) ? totalSpeed.y : freeFalling);
        rbody.velocity = totalSpeed;
    }


    // ------------------------------ HORIZONTAL PROPERTIES AND METHODS ------------------------------
    private bool HorizontalMovement()
    {
        return false;
    }


    // ------------------------------ VERTICAL PROPERTIES AND METHODS ------------------------------
    [SerializeField] private float mercyJumpDistance = 0f;
    [SerializeField] private float allowedJumpRatio = 0f; // Allowed Jump will always be a fraction of mercy jump distance
    private float allowedJumpDistance { get { return allowedJumpRatio * mercyJumpDistance; } }
    
    private bool mercyJump;
    private int[] jumpCount; // {Current Jump Count, Max Jump Count}

    private bool JumpExecution()
    {
        if (!allowMovement) return false;

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

    private bool FlyExecution()
    {
        return false;
    }

    private bool FloatExecution()
    {
        return false;
    }


    // ============================== GIZMOS ==============================
    void OnDrawGizmos()
    {
        if (!allowMovement) return;

        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundRaycastT.position, new Vector2(groundRaycastT.position.x, groundRaycastT.position.y - mercyJumpDistance));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundRaycastT.position, new Vector2(groundRaycastT.position.x, groundRaycastT.position.y - allowedJumpDistance));
    }
}
