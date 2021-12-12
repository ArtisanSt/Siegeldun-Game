using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// ENEMY AI SCRIPT VERSION : V2.1

public class EnemyAIv2 : Entity
{
    // ========================================= UNITY PROPERTIES =========================================
    // Component Declaration
    private Seeker seeker;
    private enum MovementAnim { idle, run, jump, fall };
    private MovementAnim state;


    [Header("Pathfinding")]
    protected Path path;
    [SerializeField] Transform target;
    private bool targetAlive;
    [SerializeField] protected float pathUpdateSec = 0.25f;
    [SerializeField] protected int currentWaypoint;
    [SerializeField] private float nextWayPointDistance;

    [Header("NPC")]
    [SerializeField] private bool doSleep = false; // Enemy might sleep
    [SerializeField] private bool isAwake = true; // Enemy might start in a sleep mode that will only wake up when attacked or touched
    [SerializeField] private float sleepStartTime;
    [SerializeField] private float sleepTime = 10f;
    [SerializeField] protected int awakeTask = 0; // 0 for standing for several seconds, 1 for walking until timer stops
    [SerializeField] protected float wanderMvSpeed = 3f; // 0 for standing for several seconds, 1 for walking until timer stops
    [SerializeField] protected float awakeTaskTime = 0f;
    [SerializeField] protected float awakeTaskLimit = 1f;


    [SerializeField] private bool isTriggered = false; // Enemy when triggered have time when it cannot reach the target
    [SerializeField] private float triggeredTime;
    [SerializeField] private float forgivenessTime = 2f;
    [SerializeField] private float triggerDistance = 3f; // Target's distance to trigger the enemy
    [SerializeField] private float targetNodalDistance; // Nodal Distance of the target
    [SerializeField] private float targetStraightDistance; // Straigh Distance of the target
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
        transform.localScale = new Vector3(-1, 1, 1);

        entityName = rBody.name;
        EntityStatsInitialization(entityName);
        isAlive = true;

        // Battle Initialization
        entityWeapon = 0; // Pseudo Weapon Index
        entityDamage = 20f;
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
        sleepTime = 10f;
        doSleep = false;
        isAwake = true;
        wanderMvSpeed = 3f;
        awakeTaskTime = 0f;
        awakeTaskLimit = 1f;

        isTriggered = false;
        forgivenessTime = 3f;
        triggerDistance = 5f;
        backOffDistance = 0.5f; // Prevents the enemy to collide entirely with the target
        stayDistance = 0.8f; // Prevents the enemy to collide entirely with the target

        jumpEnabled = true;
        edgeStuckAlphaError = 0.02f;
        allowJumpAfter = .05f;

        lastXPosition = rBody.position.x;
        nextWayPointDistance = backOffDistance;
    }


    // ========================================= UNITY MAIN METHODS =========================================
    // Initializes when the Player Script is called
    public void Start()
    {
        seeker = GetComponent<Seeker>();
        ComponentInitialization();
        EnemyNPCInitialization();

        targetAlive = target.GetComponent<Player>().isAlive;
        InvokeRepeating("UpdatePath", 0f, .02f);

    }

    // Updates Every Frame
    void Update()
    {
        if (isAlive)
        {
            PassiveSkills(hpRegenAllowed, stamRegenAllowed, forgivenessTime, !isTriggered);
        }
        HealthBarUpdate();
    }

    // Updates Every Physics Frame
    void FixedUpdate()
    {
        if (isAlive)
        {
            if (!isTriggered)
            {
                // AI does tasks when awake and not triggered
                if (isAwake)
                {
                    // The AI chases the player when triggered and in reacable location
                    if (targetNodalDistance <= triggerDistance && target.GetComponent<Player>().isAlive)
                    {
                        isTriggered = true;
                        triggeredTime = 0;
                    }
                    // When the player is out of range, the AI does its tasks
                    else
                    {
                        // The AI might go to sleep or wander


                        // Executes if the AI do sleep
                        if (doSleep)
                        {
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
                // AI awakens when the player entered half of its trigger distance
                else if (targetNodalDistance <= triggerDistance / 2)
                {
                    isAwake = true;
                }
            }
            // AI will chase the player and will try to kill him when detected
            else
            {
                // The trigger timer resets when the player stays inside the trigger distance
                if (targetNodalDistance <= triggerDistance && target.GetComponent<Player>().isAlive)
                {
                    triggeredTime = 0;
                }
                // The trigger timer increments when then player leaves the trigger distance but the AI will still chase until it forgives the player
                else
                {
                    // When triggered time is over forgiveness time then the AI stops chasing the Player
                    if (triggeredTime >= forgivenessTime)
                    {
                        isTriggered = false;
                        triggeredTime = 0;
                    }
                    else
                    {
                        triggeredTime += Time.deltaTime;
                    }

                    PathFollow(); // Updates the nodal position of the target
                }
            }
            Timer();
            Movement(); // Updates the movements of the AI
            AnimationState(); // Updates the Animation of the Entity
        }
    }

    // Updates Path Every Frame
    private void UpdatePath()
    {
        if (seeker.IsDone() && targetAlive)
        {
            try
            {
                targetAlive = true;
                seeker.StartPath(rBody.position, target.position, OnPathComplete);
            }
            catch (MissingReferenceException)
            {
                targetAlive = false;
                isTriggered = false;
                triggeredTime = 0;
            }
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
            enemy.gameObject.GetComponent<Player>().TakeDamage(entityDamage / 3, kbDir, weaponKbForce);
        }
    }


    // ========================================= AI PATHFINDING METHODS =========================================
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

    private void Timer()
    {
        // Knockback Timer
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
    }

    private void Movement()
    {
        // When chasing Player
        if (isTriggered)
        {
            // Anti-glitching code when target is on unreacheable location
            if (triggeredTime > 0f && (Mathf.Abs(target.position.y - rBody.position.y) > 1) && (Mathf.Abs(target.position.x - rBody.position.x) < .1))
            {
                isTriggered = false;
                triggeredTime = 0;
            }

            // Horizontal Parameter
            inProximity = (targetNodalDistance < backOffDistance) ? -1 : ((targetNodalDistance >= backOffDistance && targetNodalDistance <= stayDistance) ? 0 : 1);
            dirFacing = (targetAlive) ? ((inProximity < 1) ? 0 : ((target.position.x > rBody.position.x) ? 1 : -1)) : dirFacing;


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
        }

        // When not chasing player
        else
        {
            // Horizontal Parameter
            inProximity = 1;

            // Vertical Parameter
            allowJump = false;

            // The AI might go for a walk or idle in a certain position for several seconds
            if (isAwake)
            {
                // Current task time is done
                if (awakeTaskTime >= awakeTaskLimit)
                {
                    awakeTaskTime = 0f;
                    edgeStuckTime = 0f;
                }
                // Current task time is playing
                else
                {
                    // The AI decides what task it should do
                    if (awakeTaskTime == 0f)
                    {
                        awakeTask = Random.Range(0, 2);  // 0 for standing for several seconds, 1 for walking until timer stops
                        awakeTaskLimit = Random.Range(3f, 6f); // Time doing task

                        // If AI will wander then it should face the other direction
                        if (awakeTask == 1)
                        {
                            dirFacing = (dirX >= 1) ? -1 : 1; // Negative value of the previous value of dirX
                        }
                    }
                    // The AI is currently doing its tasks
                    else
                    {
                        // If the AI decides to wander
                        if (awakeTask == 1)
                        {
                            // Checks if the AI is stuck in a position
                            if (rBody.position.x <= lastXPosition + (edgeStuckAlphaError / 2) && rBody.position.x >= lastXPosition - (edgeStuckAlphaError / 2))
                            {
                                // if stuck, it should rotate
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
                            else
                            {
                                edgeStuck = true;
                            }
                        }
                        // If the AI decides to idle
                        else
                        {
                            dirFacing = 0;
                        }
                    }
                    awakeTaskTime += Time.deltaTime;
                }
            }
            // If the AI decides to idle
            else
            {
                dirFacing = 0;
            }
        }
        lastXPosition = rBody.position.x; // Updates the last X position of the AI
        Debug.Log("isTriggered " + isTriggered.ToString());
        Debug.Log("awakeTask " + awakeTask.ToString());
        Debug.Log("awakeTaskLimit " + awakeTaskLimit.ToString());
        Debug.Log("awakeTaskTime " + awakeTaskTime.ToString());

        // Horizontal Movement
        attackFacing = (attacking) ? ((sprite.flipX) ? -1 : 1) : 0; // Weapondrag Effect
        knockbackFacing = (isKnockbacked) ? kbDir : 0; // Knockback Effect
        dirX = (attacking) ? dirX * slowDownConst : dirFacing; // Front movement with a slowdown effect when attacking
        totalMvSpeed = ((isTriggered) ? mvSpeed : wanderMvSpeed) + mvSpeedBoost; // Run when triggered but walk when wandering
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
            Attack();
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
                transform.localScale = new Vector3((dirX > 0f) ? -1 : 1, 1, 1);
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
                anim.speed = totalMvSpeed / mvSpeed;
            }

            if (!attacking)
            {
                anim.SetInteger("state", (int)state);
            }
        }
    }
}