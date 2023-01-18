using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MovementScript : MonoBehaviour, IRestrictable, IInitializeable, IControllable
{
    // ============================== MAIN RESTRICTION ==============================
    public bool paused { get { return GameSystem.paused; } }
    [SerializeField] private bool _allowed;
    public bool allowed => _allowed;
    public bool alive { get; set; }
    public bool initialized { get; private set; }

    public bool IsRestricted() { return paused || !allowed || !initialized; }


    // ============================== IINITIALIZEABLE ==============================
    public void Init()
    {
        alive = true;
        initialized = true;
        if (IsRestricted()) return;
        PropertyInit();
        ComponentInit();
        StatsInit();
    }


    // ------------------------------ PROPERTIES ------------------------------
    public int difficulty { get { return GameSystem.difficulty; } }
    protected void PropertyInit()
    {
        state = new State();
    }


    // ------------------------------ COMPONENTS ------------------------------
    private LayerMask groundLayer { get { return GameSystem.instance.groundLayer; } }

    private Rigidbody2D rbody;
    [SerializeField] private Transform groundRaycastT;
    private LayerMask jumpableLayers;

    private void ComponentInit()
    {
        rbody = GetComponent<IMoveable>().rbody;
        jumpableLayers = baseProp.jumpableLayers;

        groundRaycastT = transform.GetChild("RaycastGround");
    }


    // ============================== UNITY METHODS ==============================
    protected virtual void Update()
    {
        if (IsRestricted()) return;
        Movement();
    }

    protected virtual void FixedUpdate()
    {
        if (IsRestricted()) return;

    }

    protected virtual void LateUpdate()
    {
        if (IsRestricted()) return;

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


    // ============================== OBJECT PROPERTIES AND METHODS ==============================
    // ------------------------------ EFFECT RESTRICTIONS ------------------------------
    public struct State
    {
        public bool heavy; // cannot jump

        public bool stunned;
        public bool root; // stunned but with roots effect

        public bool cold; // Slowed but with cold effect
        public bool frozen; // Stunned but with cold effect

        public bool _slowed;
        public bool slowed { get { return cold || _slowed; } }

        private bool _immovable; // cannot jump nor move
        public bool immovable { get { return stunned || root || frozen || _immovable; } }
    }
    public State state { get; private set; }


    // ------------------------------ MOVEMENT PROPERTIES ------------------------------
    public MovementProp baseProp;

    public MovementProp.Stats statsBase { get; private set; } // Base Stats
    public MovementProp.Stats statsBstd { get; private set; } // Boosted Stats (equipables, potions)
    public MovementProp.Stats statsBoots { get; private set; } // Boots Stats (only for mv speed and jump)

    public float runSpeed { get { return (statsBoots.runSpeed.x + statsBase.runSpeed.x * (1 + statsBoots.runSpeed.y)) * (1 + statsBstd.runSpeed.y) + statsBstd.runSpeed.x; } }
    public float walkSpeed { get { return runSpeed * baseProp.crouchMultiplier; } }
    public float mvForce { get { return (controls.crouch) ? walkSpeed : runSpeed; } }
    public float jumpForce { get { return (statsBoots.jumpForce.x + statsBase.jumpForce.x * (1 + statsBoots.jumpForce.y)) * (1 + statsBstd.jumpForce.y) + statsBstd.jumpForce.x; } }

    public int curJumpCount { get; private set; }
    public int maxJumpCount { get { return ((statsBase.jumpCount >= statsBoots.jumpCount) ? statsBase.jumpCount : statsBoots.jumpCount) + statsBstd.jumpCount; } }

    public void StatsInit()
    {
        statsBase = baseProp.stats[difficulty];
        statsBstd = new MovementProp.Stats();
        statsBoots = new MovementProp.Stats();

        curJumpCount = maxJumpCount;
    }


    // ------------------------------ MOVEMENT METHODS ------------------------------
    private void Movement()
    {
        rbody.velocity = MVSpeed();
    }

    private Vector2 MVSpeed()
    {
        if (state.immovable) return new Vector2(0,0);

        // Jump Restrictions
        bool executeJump = JumpExecution();
        float freeFalling = (Mathf.Abs(rbody.velocity.y) < 0.001f) ? 0f : rbody.velocity.y;

        return new Vector2(controls.horizontal * mvForce, executeJump ? jumpForce : freeFalling);
    }


    // ------------------------------ HORIZONTAL PROPERTIES AND METHODS ------------------------------
    private bool HorizontalMovement()
    {
        return false;
    }


    // ------------------------------ VERTICAL PROPERTIES AND METHODS ------------------------------
    [SerializeField] private float mercyJumpDistance;
    [SerializeField] private float allowedJumpRatio; // Allowed Jump will always be a fraction of mercy jump distance
    private float allowedJumpDistance { get { return allowedJumpRatio * mercyJumpDistance; } }

    private bool mercyJump;

    private bool JumpExecution()
    {
        if (state.heavy) return false;

        // --------------------- Change to cone-shaped raycast ---------------------
        RaycastHit2D rayHit = Physics2D.Raycast(groundRaycastT.position, Vector2.down, mercyJumpDistance, jumpableLayers);
        float rayDistance = (rayHit.collider != null) ? groundRaycastT.position.y - rayHit.point.y : 2 * mercyJumpDistance;

        // Jump Count Reset
        System.Func<float, bool> distanceRestriction = jumpDistance => rayDistance <= jumpDistance && rbody.velocity.y <= 0;
        if (distanceRestriction(allowedJumpDistance) && curJumpCount != maxJumpCount)
            curJumpCount = maxJumpCount;

        bool jumpRestriction = distanceRestriction(allowedJumpDistance) || maxJumpCount - curJumpCount >= 1; // Can still configure
        bool executeJump = curJumpCount > 0 && jumpRestriction && (controls.jump || mercyJump);
        if (executeJump)
        {
            curJumpCount--;
            mercyJump = false;
        }
        else if (!mercyJump) mercyJump = (controls.jump && distanceRestriction(mercyJumpDistance)); // Checks if the player pressed jumped n seconds early
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


    // ------------------------------ IControllable ------------------------------
    public Controller.Controls controls { get; private set; }

    // Communicates with other components
    public void Receiver(Controller.Controls controls)
    {
        if (IsRestricted()) return;
        if (!alive || state.immovable)
            this.controls = new Controller.Controls();
        else
            this.controls = controls;
    }


    // ============================== JSON ==============================
    public string DefaultsToJson() => $"{{ \"{baseProp.GetType()}\" : {baseProp.ToJson()} }}";


    // ============================== GIZMOS ==============================
    void OnDrawGizmos()
    {
        if (groundRaycastT == null) return;

        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundRaycastT.position, new Vector2(groundRaycastT.position.x, groundRaycastT.position.y - mercyJumpDistance));

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundRaycastT.position, new Vector2(groundRaycastT.position.x, groundRaycastT.position.y - allowedJumpDistance));
    }
}
