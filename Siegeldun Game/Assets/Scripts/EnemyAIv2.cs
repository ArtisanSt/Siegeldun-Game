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


    [Header("Pathfinding")]
    protected Path path;
    [SerializeField] Transform target;
    [SerializeField] protected float pathUpdateSec = 0.25f;
    [SerializeField] protected int currentWaypoint;
    [SerializeField] private float nextWayPointDistance;

    [Header("NPC")]
    [SerializeField] private bool doSleep = false; // Enemy might sleep
    [SerializeField] private bool isAwake = true; // Enemy might start in a sleep mode that will only wake up when attacked or touched
    [SerializeField] private float sleepStartTime;
    [SerializeField] private float sleepTime = 2f;
    [SerializeField] protected int awakeTask = 0; // 0 for standing for several seconds, 1 for walking until reaching position
    [SerializeField] protected float awakeTaskTime = 0f;
    [SerializeField] protected float awakeTaskLimit;


    [SerializeField] private bool isTriggered = false; // Enemy when triggered have time when it cannot reach the target
    [SerializeField] private float triggeredTime;
    [SerializeField] private float forgivenessTime = 2f;
    [SerializeField] private float triggerDistance = 3f; // Target's distance to trigger the enemy
    [SerializeField] private float targetNodalDistance; // Nodal Distance of the target
    [SerializeField] private float targetStraightDistance; // Straigh Distance of the target
    [SerializeField] private float distanceAlphaError = 0.2f; // Straigh Distance of the target
    [SerializeField] private int isReachable = 0;
    [SerializeField] private float backOffDistance = 0.5f; // Prevents the enemy to collide entirely with the target
    [SerializeField] private float stayDistance = 0.8f; // Prevents the enemy to collide entirely with the target
    [SerializeField] private int inProximity;

    [SerializeField] bool jumpEnabled = true;
    [SerializeField] private bool edgeStuck; // Toggles when stuck in the edge of obstacles
    [SerializeField] private float edgeStuckAlphaError = 0.02f; // Edge Stuck Position Margin of Error
    private float lastXPosition;
    [SerializeField] private float edgeStuckTime;
    [SerializeField] private float allowJumpAfter = .05f;
    [SerializeField] private bool allowJump;



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
        mvSpeed = 5f;
        jumpForce = 15.5f;
        rBody.gravityScale = 6;

        // Pathfinding Initialization
        pathUpdateSec = 0.25f;
        doSleep = false;
        isAwake = true;
        sleepTime = 2f;

        isTriggered = false;
        forgivenessTime = 3f;
        triggerDistance = 5f;
        backOffDistance = 0.5f; // Prevents the enemy to collide entirely with the target
        stayDistance = 0.8f; // Prevents the enemy to collide entirely with the target

        jumpEnabled = true;
        edgeStuckAlphaError = 0.02f;
        allowJumpAfter = .25f;

        lastXPosition = rBody.position.x;
        nextWayPointDistance = backOffDistance;
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

        EnemyNPCInitialization();

        InvokeRepeating("UpdatePath", 0f, .02f);

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
            if (!isTriggered)
            {
                if (isAwake)
                {
                    if (targetNodalDistance <= triggerDistance)
                    {
                        isTriggered = true;
                        triggeredTime = 0;
                    }
                    else
                    {
                        if (doSleep)
                        {
                            // idle and walking from one place to another
                            if (sleepStartTime >= sleepTime)
                            {
                                isAwake = false;
                                sleepStartTime = 0;
                            }
                            else
                            {
                                sleepStartTime += Time.deltaTime;
                            }
                        }
                    }
                }
                else
                {
                    if (targetNodalDistance <= backOffDistance)
                    {
                        isAwake = true;
                    }
                    else
                    {
                        //sleep in a current position
                    }
                }
            }
            else
            {
                if (targetNodalDistance <= triggerDistance)
                {
                    triggeredTime = 0;
                }
                else
                {
                    if (triggeredTime >= forgivenessTime)
                    {
                        isTriggered = false;
                        triggeredTime = 0;
                    }
                    else
                    {
                        triggeredTime += Time.deltaTime;
                    }
                    PathFollow();
                }
            }
            Movement();
        }
        AnimationState();
    }

    // Updates Path Every Frame
    private void UpdatePath()
    {
        if (seeker.IsDone())
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
            //Debug.Log(enemy.gameObject.GetComponent<Rigidbody2D>().position.x);
            int kbDir = (enemy.gameObject.GetComponent<Rigidbody2D>().position.x > rBody.position.x) ? 1 : -1;
            enemy.gameObject.GetComponent<Player>().TakeDamage(entityDamage, kbDir, weaponKbForce);
        }
    }


    // ========================================= AI PATHFINDING METHODS =========================================
    private void Movement()
    {
        // Pseudo Knockback Timer
        if (isKnockbacked)
        {
            if (kTick > .1f)
            {
                isKnockbacked = false;
                knockbackedForce = kbHorDisplacement;
                kTick = 0f;
            }
            else
            {
                kTick += Time.deltaTime;
            }
        }

        // When chasing Player
        if (isTriggered)
        {
            // Horizontal Parameter
            dirFacing = (target.position.x > rBody.position.x) ? 1 : -1;

            // Vertical Parameter
            if (rBody.position.x <= lastXPosition + (edgeStuckAlphaError / 2) && rBody.position.x >= lastXPosition - (edgeStuckAlphaError / 2) && inProximity >= 1)
            {
                if (edgeStuck)
                {
                    if (edgeStuckTime >= allowJumpAfter)
                    {
                        allowJump = true;
                        edgeStuckTime = 0f;
                    }
                    else
                    {
                        edgeStuckTime += Time.deltaTime;
                    }
                }
                else
                {
                    edgeStuck = true;
                    edgeStuckTime = 0f;
                }
            }
            else
            {
                allowJump = false;
                edgeStuckTime = 0f;
            }
            lastXPosition = rBody.position.x;

            Debug.Log(Mathf.Abs(target.position.y - rBody.position.y));
            inProximity = (targetNodalDistance < backOffDistance) ? -1 : ((targetNodalDistance >= backOffDistance && targetNodalDistance <= stayDistance) ? 0 : 1);
            if (triggeredTime > 0f && (Mathf.Abs(target.position.y - rBody.position.y) > 1) && (Mathf.Abs(target.position.x - rBody.position.x) < .1))
            {
                isTriggered = false;
                triggeredTime = 0;
            }
        }

        // When not chasing player
        else
        {
            // Horizontal Parameter
            inProximity = 1;
            isReachable = 1;
            // Vertical Parameter
            allowJump = false;

            if (isAwake)
            {
                if (awakeTaskTime == 0f)
                {
                    awakeTask = Random.Range(0, 1);
                    awakeTaskLimit = Random.Range(1f, 3f);
                    awakeTaskTime += Time.deltaTime;

                    if (awakeTask == 1)
                    {
                        dirFacing = (dirX >= 1) ? -1 : 1; // Negative value of the previous value of dirX
                    }
                }
                else
                {
                    if (awakeTaskTime >= awakeTaskLimit)
                    {
                        awakeTaskTime = 0f;
                        edgeStuckTime = 0f;
                    }
                    else
                    {
                        if (awakeTask == 1)
                        {
                            // Last X Position Check
                            if (rBody.position.x <= lastXPosition + (edgeStuckAlphaError / 2) && rBody.position.x >= lastXPosition - (edgeStuckAlphaError / 2))
                            {
                                if (edgeStuck)
                                {
                                    if (edgeStuckTime >= .02f)
                                    {
                                        dirFacing = -dirFacing;
                                    }
                                    else
                                    {
                                        edgeStuckTime += Time.deltaTime;
                                    }
                                }
                                else
                                {
                                    edgeStuck = true;
                                    edgeStuckTime = 0f;
                                }
                            }
                        }
                        else
                        {
                            dirFacing = 0;
                        }
                    }
                }
                lastXPosition = rBody.position.x;
            }
        }

        // Horizontal Movement
        attackFacing = (attacking) ? ((sprite.flipX) ? -1 : 1) : 0; // Weapondrag Effect
        knockbackFacing = (isKnockbacked) ? kbDir : 0; // Knockback Effect
        dirX = (attacking) ? dirX * slowDownConst : dirFacing; // Front movement with a slowdown effect when attacking
        totalMvSpeed = mvSpeed + mvSpeedBoost;
        runVelocity = (dirX * totalMvSpeed * inProximity) + (attackFacing * weaponDrag) + (knockbackFacing * knockbackedForce);

        // Vertical Movement
        isGrounded = capColl.IsTouchingLayers(groundLayers);
        dirY = allowJump ? jumpForce : ((0f < rBody.velocity.y && rBody.velocity.y < 0.001f) ? 0f : rBody.velocity.y);
        jumpVelocity = (jumpEnabled && isGrounded) ? dirY : rBody.velocity.y;

        rBody.velocity = new Vector2(runVelocity, jumpVelocity);

        // Attack Code
        attacking = GetComponent<SpriteRenderer>().sprite.ToString().Substring(0, 13) == "Goblin_Attack"; // Anti-spamming code
        if (inProximity < 1 && !attacking && Time.time - lastAttack > attackDelay)
        {
            Debug.Log(true);
            Attack();
        }
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Pathfinding Update
        float distance = Vector2.Distance(rBody.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWayPointDistance)
            currentWaypoint++;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            targetNodalDistance = path.GetTotalLength();
            targetStraightDistance = Mathf.Sqrt(Mathf.Pow(target.position.x - transform.position.x, 2) + Mathf.Pow(target.position.y - transform.position.y, 2));
            //GraphNode node = AstarPath.active.GetNearest(transform.position).node;
        }
    }


    // ========================================= ANIMATION METHODS =========================================
    private void AnimationState()
    {
        if (isAlive)
        {
            // Horizontal Movement Animation
            runAnimationSpeed = Mathf.Abs(runVelocity);
            if (dirX == 0)
            {
                state = MovementAnim.idle;
                anim.speed = animationSpeed;
            }
            else
            {
                state = MovementAnim.run;
                anim.speed = runAnimationSpeed;
                if (dirX > 0f)
                {
                    sprite.flipX = false;
                }
                else
                {
                    sprite.flipX = true;
                }
            }

            // Vertical Movement Animation
            if (rBody.velocity.y > .99f)
            {
                state = MovementAnim.jump;
            }
            else if (rBody.velocity.y < -1f)
            {
                state = MovementAnim.fall;
            }


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