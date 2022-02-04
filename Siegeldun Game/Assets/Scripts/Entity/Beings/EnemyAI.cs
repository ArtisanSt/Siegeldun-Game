using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : Beings
{
    // ========================================= UNITY PROPERTIES =========================================
    // Component Declaration
    private Seeker seeker;

    [Header("Pathfinding", order = 0)]
    private Path path;
    [SerializeField] protected Transform target;
    protected bool targetAlive = true;
    [SerializeField] protected float pathUpdateSec = 0.25f;
    [SerializeField] protected int currentWaypoint;
    [SerializeField] protected float nextWayPointDistance;
    [SerializeField] protected bool isChasing;

    [Header("NPC", order = 1)]
    [Header("Sleeping", order = 2)]
    [SerializeField] protected bool doSleep = false; // Enemy might sleep
    [SerializeField] protected bool isAwake = true; // Enemy might start in a sleep mode that will only wake up when attacked or touched
    [SerializeField] protected float sleepStartTime;
    [SerializeField] protected float sleepTime = 10f;
    [SerializeField] protected int awakeTask = 0; // 0 for standing for several seconds, 1 for walking until timer stops
    [SerializeField] protected float wanderMvSpeed = 3f; // 0 for standing for several seconds, 1 for walking until timer stops
    [SerializeField] protected float awakeTaskTime = 0f;
    [SerializeField] protected float awakeTaskLimit = 1f;
    [SerializeField] protected ParticleSystem sleepParticle;

    [Header("Triggering", order = 2)]
    [SerializeField] protected bool isTriggered = false; // Enemy when triggered have time when it cannot reach the target
    [SerializeField] protected bool targetSeen = false;
    [SerializeField] protected float unseenTime;
    [SerializeField] protected float forgivenessTime = 2f;
    [SerializeField] protected float triggerDistance = 3f; // Target's distance to trigger the enemy
    [SerializeField] protected float targetNodalDistance; // Nodal Distance of the target
    [SerializeField] protected float targetStraightDistance; // Straigh Distance of the target
    [SerializeField] protected float backOffDistance; // Prevents the enemy to collide entirely with the target
    [SerializeField] protected float stayDistance; // Prevents the enemy to collide entirely with the target
    [SerializeField] protected int inProximity;

    [Header("Stuck Escaping", order = 2)]
    [SerializeField] protected bool jumpEnabled = true;
    [SerializeField] protected bool edgeStuck; // Toggles when stuck in the edge of obstacles
    [SerializeField] protected float edgeStuckAlphaError = 0.02f; // Edge Stuck Position Margin of Error
    protected float lastXPosition;
    [SerializeField] protected float edgeStuckTime;
    [SerializeField] protected float allowJumpAfter = .05f;

    [Header("Limitations", order = 2)]
    public GameObject spawnerObject = null;
    public bool hasLimitations;
    [SerializeField] public List<Transform> activePoints = new List<Transform>();
    protected bool inLimitation;
    protected bool deathFinalized = false;


    // ========================================= UNITY MAIN METHODS =========================================
    // Initializes when the Player Script is called
    protected void NPCInit()
    {
        seeker = GetComponent<Seeker>();
        sleepParticle = gameObject.transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        if (hasLimitations)
        {
            Debug.Log(entityName + " " + entityID.ToString());
            Debug.Log(activePoints[0]);
            Debug.Log(activePoints[1]);
        }

        // Pathfinding Initialization
        pathUpdateSec = 0.25f;
        isChasing = false;
        awakeTaskTime = 0f;
        awakeTaskLimit = 1f;

        target = GameObject.Find("Player").transform;
        isTriggered = false;
        targetSeen = false;

        edgeStuckAlphaError = 0.02f;
        allowJumpAfter = .05f;

        lastXPosition = rBody.position.x;
        nextWayPointDistance = backOffDistance;

        InvokeRepeating("UpdatePath", 0f, .02f);
    }

    // Updates Every Frame
    protected void EnemyNPCUpdate()
    {
        if (isAlive)
        {
            PassiveSkills(hpRegenAllowed, stamRegenAllowed, forgivenessTime, !isTriggered);
        }
        HealthBarUpdate();
    }

    // Updates Every Physics Frame
    protected void EnemyNPCFixedUpdate()
    {
        NPCDecisionMakingSystem();
        Timer();
    }

    // Updates Path Every Frame
    private void UpdatePath()
    {
        targetAlive = target != null && target.GetComponent<Player>().isAlive;
        if (seeker.IsDone() && targetAlive)
        {

            try
            {
                seeker.StartPath(rBody.position, target.position, OnPathComplete);
            }
            catch (MissingReferenceException)
            {
                isTriggered = false;
                unseenTime = 0;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(col.otherCollider, col.collider);
        }

        if (isAttacking)
        {

        }
    }


    // ========================================= ENEMY METHODS =========================================
    private void Attack()
    {
        int attackID = Random.Range(-9999, 10000);
        lastAttack = Time.time;
        anim.SetTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            //Debug.Log(enemy.gameObject.GetComponent<Rigidbody2D>().position.x);
            int kbDir = (enemy.GetComponent<Rigidbody2D>().position.x > rBody.position.x) ? 1 : -1;
            enemy.GetComponent<Entity>().TakeDamage(entityDamage, attackID, kbDir, weaponKbForce);
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

        if (!targetSeen)
        {
            // When triggered time is over forgiveness time then the AI stops chasing the Player
            if (unseenTime >= forgivenessTime)
            {
                isTriggered = false;
            }
            else
            {
                unseenTime += Time.deltaTime;
            }
        }
        else
        {
            unseenTime = 0;
        }
    }

    // ========================================= NPC MOVEMENT METHODS =========================================
    private void NPCDecisionMakingSystem()
    {
        inLimitation = (hasLimitations && targetAlive) ? target.position.x >= activePoints[0].position.x && target.position.x <= activePoints[1].position.x : true;
        if (!isTriggered)
        {
            var sleepingParticleShape = sleepParticle.shape;
            sleepingParticleShape.rotation = new Vector3(0, 0, transform.localScale.x * -45);
            // AI does tasks when awake and not triggered
            if (isAwake)
            {
                // The AI chases the player when triggered and in reachable location
                if (targetNodalDistance <= triggerDistance && inLimitation)
                {
                    isTriggered = true;
                    targetSeen = true;
                }
                // When the player is out of range, the AI does its tasks
                else
                {
                    // Executes if the AI do sleep
                    if (doSleep)
                    {
                        if (sleepStartTime >= sleepTime)
                        {
                            isAwake = false;
                            sleepStartTime = 0;
                            sleepParticle.Play();
                        }
                        else
                        {
                            sleepStartTime += Time.deltaTime;
                        }
                    }
                }
            }
            // AI awakens when the player entered half of its trigger distance
            else if (targetNodalDistance <= triggerDistance * (2 / 3) && inLimitation)
            {
                isAwake = true;
                sleepParticle.Stop();
            }
        }
        // AI will chase the player and will try to kill him when detected
        else
        {
            // The trigger timer resets when the player stays inside the trigger distance
            if (targetNodalDistance <= triggerDistance && inLimitation)
            {
                targetSeen = true;
            }
            // The trigger timer increments when then player leaves the trigger distance but the AI will still chase until it forgives the player
            else
            {
                // When triggered time is over forgiveness time then the AI stops chasing the Player
                targetSeen = false;
            }
            PathFollow(); // Updates the nodal position of the target
        }
    }

    private void NPCDecisionMakingExecution()
    {
        // The AI might go for a walk or idle in a certain position for several seconds
        if (isAwake)
        {
            // Current task time is done
            if (awakeTaskTime >= awakeTaskLimit)
            {
                awakeTaskTime = 0f;
            }
            // Current task time is playing
            else
            {
                // The AI decides what task it should do
                if (awakeTaskTime == 0f)
                {
                    awakeTask = Random.Range(0, 2);  // 0 for standing for several seconds, 1 for walking until timer stops
                    awakeTaskLimit = Random.Range(3f, 6f); // Time doing task

                    dirFacing = (isAlive) ? ((awakeTask == 1) ? (new float[] { -1, 1 }[Random.Range(0, 2)]) : 0 ) : 0; // Negative value of the previous value of dirX
                }
                else if (hasLimitations && rBody.position.x <= activePoints[0].position.x)
                {
                    dirFacing = 1;
                    isTriggered = false;
                    unseenTime = 0;
                }
                else if (hasLimitations && rBody.position.x >= activePoints[1].position.x)
                {
                    dirFacing = -1;
                    isTriggered = false;
                    unseenTime = 0;
                }
                awakeTaskTime += Time.deltaTime;
            }
        }
        // If the AI decides to sleep
        else
        {
            dirFacing = 0;
        }
    }

    protected void Controller()
    {
        if (isAlive)
        {
            // Anti-glitching code when target is on unreacheable location
            if ((targetAlive && ((Mathf.Abs(target.position.y - rBody.position.y) > 1) && (Mathf.Abs(target.position.x - rBody.position.x) < backOffDistance))) || !inLimitation)
            {
                isTriggered = false;
                unseenTime = 0;
            }

            // Vertical Parameter
            if (allowJump)
            {
                allowJump = false;
                edgeStuck = false;
                edgeStuckTime = 0f;
            }
            else if (rBody.position.x <= lastXPosition + (edgeStuckAlphaError / 2) && rBody.position.x >= lastXPosition - (edgeStuckAlphaError / 2) && inProximity == 1 && boxColl.IsTouchingLayers(groundLayers) && isAwake)
            {
                if (edgeStuck)
                {
                    if (edgeStuckTime >= allowJumpAfter)
                    {
                        allowJump = true;
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

            // Horizontal Parameter
            inProximity = (targetAlive) ? ((targetNodalDistance < backOffDistance) ? -1 : ((targetNodalDistance >= backOffDistance && targetNodalDistance <= stayDistance) ? 0 : 1)) : 1;
            if (isTriggered)
            {
                if (target.GetComponent<Player>().isGrounded || (!target.GetComponent<Player>().isGrounded && Mathf.Abs(target.position.x - rBody.position.x) >= stayDistance))
                {
                    isChasing = true;
                }
                else
                {
                    isChasing = false;
                }

                dirFacing = ((isAlive) ? ((target.position.x > rBody.position.x) ? 1 : -1) : 0) * ((isChasing) ? 1 : 0) * inProximity;
            }
            else
            {
                NPCDecisionMakingExecution();
            }
            lastXPosition = rBody.position.x; // Updates the last X position of the AI

            // Attack Code
            if (inProximity == 0 && !isAttacking && !isHurting && Time.time - lastAttack > attackDelay)
            {
                Attack();
            }
        }
        else
        {
            allowJump = false;
            inProximity = 0;
        }
    }


    // ========================================= NPC DEATH FINALIZER =========================================
    protected void DeathFinalizer()
    {
        if (spawnerObject != null && !deathFinalized)
        {
            spawnerObject.transform.parent.gameObject.GetComponent<Spawner>().ClearInstance(entityName, gameObject);
        }
        EntityInstances[entityName].Remove(gameObject);
    }
}