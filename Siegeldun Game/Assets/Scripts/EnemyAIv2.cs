using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// ENEMY AI SCRIPT VERSION : V2.1

public class EnemyAIv2 : Entity
{
    // ========================================= UNITY PROPERTIES =========================================
    // Component Declaration
    public Rigidbody2D rBody;
    public SpriteRenderer sprite;
    private CapsuleCollider2D capColl;
    private Animator anim;
    private Seeker seeker;
    private enum MovementAnim { idle, run, jump, fall };
    private MovementAnim state;

    private bool facingRight = true;

    [Header("Pathfinding")]
    [SerializeField] Transform target;
    [SerializeField] float jumpNodeHeight;
    [SerializeField] float activateDistance;
    [SerializeField] float proximity;

    [Header("Custom Behavior")]
    [SerializeField] bool jumpEnabled;

    protected Path path;
    protected float pathUpdateSec;
    protected int currentWaypoint;


    // Enemy NPC Properties Initialization
    protected void EnemyNPCInitialization()
    {
        entityName = rBody.name;
        EntityStatsInitialization(entityName);
        isAlive = true;

        // Battle Initialization
        entityWeapon = 0; // Pseudo Weapon Index
        entityDamage = 3f;
        attackSpeed = .85f;
        attackDelay = 2f;
        lastAttack = 0f;
        attackRange = 0.3f; // Pseudo Weapon Range
        EqWeaponStamCost = 0f;
        weaponDrag = 0f; // Pseudo Weapon Drag
        weaponKbForce = .8f; // Pseudo Weapon Knockback Force
        attacking = false;
        kbDir = 0;
        kTick = 0f;
        kbHorDisplacement = .8f;
        kbVerDisplacement = 0f;

        // HP Initialization
        maxHealth = 100f;
        entityHp = maxHealth;
        hpRegenAllowed = true;
        healthRegen = healthRegenScaling[idxDiff];
        regenDelay = 3f;
        hpRegenTimer = 0f;

        // Movement Initialization
        isGrounded = false;
        mvSpeed = 150f;
        jumpForce = 600f;
        rBody.gravityScale = 6;

        // Pathfinding Initialization
        jumpNodeHeight = 0.8f;
        activateDistance = 10f;
        proximity = 3f;
        jumpEnabled = true;
        pathUpdateSec = 0.5f;
        currentWaypoint = 0;
    }


    // ========================================= UNITY MAIN METHODS =========================================
    // Initializes when the Player Script is called
    public void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        capColl = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSec);

        EnemyNPCInitialization();
    }

    // Updates Every Frame
    void Update()
    {
        if (isAlive)
        {
            PassiveSkills(hpRegenAllowed, stamRegenAllowed);
        }
        // HealthBarUpdate(); No hp bar ui yet
    }

    // Updates Every Physics Frame
    void FixedUpdate()
    {   
        if (isAlive)
        {
            if (TargetInDistance())
            {
                PathFollow();
            }
        }
        AnimationState();
    }

    // Updates Path Every Frame
    private void UpdatePath()
    {
        if (TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rBody.position, target.position, OnPathComplete);
        }
    }


    // ========================================= ENEMY METHODS =========================================
    private void Attack()
    {
        lastAttack = Time.time;

        anim.SetTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            int kbDir = (enemy.GetComponent<Player>().rBody.position.x > rBody.position.x) ? 1 : -1;
            enemy.GetComponent<Player>().TakeDamage(entityDamage, kbDir, weaponKbForce);
        }
    }


    // ========================================= AI PATHFINDING METHODS =========================================
    private void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Grounded Check
        isGrounded = capColl.IsTouchingLayers(groundLayers);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rBody.position).normalized;

        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeight)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, jumpForce * Time.deltaTime);
            }
        }

        // Movement
        float distEntity = Mathf.Abs(target.transform.position.x - transform.position.x);
        if (target.transform.position.x > transform.position.x && distEntity >= proximity)
        {
            rBody.velocity = new Vector2(mvSpeed * Time.deltaTime, rBody.velocity.y);
            if (!facingRight)
                Flip();
        }
        else if (target.transform.position.x < transform.position.x && distEntity >= proximity)
        {
            rBody.velocity = new Vector2(-mvSpeed * Time.deltaTime, rBody.velocity.y);
            if (facingRight)
                Flip();
        }

        // Attack if in proximity
        attacking = GetComponent<SpriteRenderer>().sprite.ToString().Substring(0, 13) == "Goblin_Attack"; // Anti-spamming code
        if (distEntity <= proximity && attacking == false && Time.time - lastAttack > attackDelay)
        {
            Attack();
        }

        float distance = Vector2.Distance(rBody.position, path.vectorPath[currentWaypoint]);
        if (distance < proximity)
            currentWaypoint++;
    }

    private bool TargetInDistance()
    {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }


    // ========================================= ANIMATION METHODS =========================================
    private void AnimationState()
    {
        if (isAlive)
        {
            if (Mathf.Abs(rBody.velocity.x) > 0)
                state = MovementAnim.run;
            if (Mathf.Abs(rBody.velocity.x) == 0 && Mathf.Abs(rBody.velocity.y) == 0)
                state = MovementAnim.idle;


            if (attacking)
            {
                anim.speed = attackSpeed;
            }
            else
            {
                anim.speed = animationSpeed;
            }
        }
        else
        {
            state = MovementAnim.idle;
        }

        anim.SetInteger("state", (int)state);
    }
}