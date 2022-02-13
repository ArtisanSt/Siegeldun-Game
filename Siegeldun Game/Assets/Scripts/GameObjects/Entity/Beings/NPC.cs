using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class NPC : Beings
{
    // ========================================= NPC PROPERTIES =========================================
    // Area of Detection
    protected Collider2D[] detectionCollider;
    private Vector2 detectionCenter;
    [Header("NPC SETTINGS", order = 5)]
    [SerializeField] private Vector2 detectionSize = new Vector2(3,1);

    // Target Properties
    protected Transform target = null;
    protected bool targetAlive = false;
    protected bool targetInDetection = false;
    protected bool targetChase = false;
    protected bool targetInLimitation = false;
    protected float targetLastDetected = 0f;

    // Pathfinding
    [SerializeField] protected bool allowPathfinding = false; // Cancels everything if false
    private Seeker seeker;
    private Path path;
    protected float pathUpdateSec = 0.25f;
    protected int currentWaypoint;
    protected float nextWayPointDistance;

    // NPC
    // Sleeping
    protected bool forcedAsleep = false;
    [SerializeField] protected bool doSleep = false; // Enemy might sleep
    [SerializeField] protected bool isAwake = true; // Enemy might start in a sleep mode that will only wake up when attacked or touched
    protected float sleepStartTime;
    [SerializeField] protected float sleepDelay = 10f;
    protected int awakeTask = 0; // 0 for standing for several seconds, 1 for walking until timer stops
    protected float awakeTaskTime = 0f;
    protected float awakeTaskLimit = 1f;
    [SerializeField] protected ParticleSystem sleepParticle;
    protected float _mvSpeed = 0f;

    // Triggers
    [SerializeField] protected bool isTriggered = false; // Enemy when triggered have time when it cannot reach the target
    protected bool targetSeen = false;
    protected float seenTime;
    [SerializeField] protected float forgivenessTime = 2f;
    [SerializeField] protected float triggerDistance = 3f; // Target's distance to trigger the enemy
    protected float targetNodalDistance; // Nodal Distance of the target
    protected float backOffDistance; // Prevents the enemy to collide entirely with the target
    protected float stayDistance; // Prevents the enemy to collide entirely with the target
    protected int inProximity;
    protected bool isChasing = false;

    // Stuck Escape
    [SerializeField] protected bool jumpEnabled = true;
    protected bool edgeStuck; // Toggles when stuck in the edge of obstacles
    protected float edgeStuckAlphaError = 0.02f; // Edge Stuck Position Margin of Error
    protected float lastXPosition;
    protected float edgeStuckTime;
    protected float allowJumpAfter = .05f;



    // ========================================= UNITY MAIN METHODS =========================================
    // Initializes when the Player Script is called
    protected void NPCInit()
    {
        seeker = GetComponent<Seeker>();
        sleepParticle = gameObject.transform.GetChild(1).gameObject.GetComponent<ParticleSystem>();

        lastXPosition = transform.position.x;

        backOffDistance = Mathf.Abs(transform.position.x - attackPoint.position.x); // Prevents the enemy to collide entirely with the target

        _mvSpeed = mvSpeed;

        InvokeRepeating("UpdatePath", 0f, .02f);
    }

    // Updates Every Frame
    protected virtual void Update()
    {
        UpdateStats();
        PassiveSkills();
        HpBarUIUpdate();
    }

    // Updates Every Physics Frame
    protected virtual void FixedUpdate()
    {
        NPCDecisionMakingSystem();
        Controller();
        Movement();
        AnimationState();
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(col.otherCollider, col.collider);
        }
    }


    // ========================================= ENEMY METHODS =========================================
    protected override void Attack()
    {
        float attackID = (float)gameObject.GetInstanceID() + Random.Range(-9999, 10000) / 10000;
        if (hasStam) { curStam -= totalStamCost; }
        _lastAttack = Time.time;
        anim.SetTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, totalAtkRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<IDamageable>() == null) continue;

            int kbDir = (enemy.transform.position.x > transform.position.x) ? 1 : -1;
            enemy.GetComponent<IDamageable>().TakeDamage(attackID, kbDir, atkStatsProp);
        }
    }


    // ========================================= AI PATHFINDING METHODS =========================================
    // Updates Path Every 0.02 seconds
    private void UpdatePath()
    {
        targetAlive = (target != null) ? target.GetComponent<Entity>().isAlive : false ;
        if (seeker.IsDone() && targetAlive)
        {
            try
            {
                seeker.StartPath(transform.position, target.position, OnPathComplete);
            }
            catch (MissingReferenceException)
            {
                TriggerOff();
            }
        }
    }

    private void PathFollow()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;

        // Pathfinding Update
        float distance = Vector2.Distance(transform.position, path.vectorPath[currentWaypoint]);
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
        }
    }

    // ========================================= NPC DETECTION SYSTEM =========================================
    private bool NPCDetectionSystem()
    {
        // Enemy Detector
        detectionCenter = transform.position;
        Vector2 detectionSize = new Vector2(this.detectionSize.x * ((isAwake) ? 1 : .5f), this.detectionSize.y);
        detectionCollider = Physics2D.OverlapBoxAll(detectionCenter, detectionSize, 0, enemyLayer);

        // Since there is no other ally planned, the only main target is the Player
        Transform pseudoTarget = null;
        for (int i = 0; i < detectionCollider.Length; i++)
        {
            Collider2D coll = detectionCollider[i];
            if (coll.GetComponent<Entity>().objectName == "Player")
            {
                pseudoTarget = coll.transform;
                break;
            }
        }
        // The Entity will only chase its enemy when it is inside the active points
        bool targetInLimitation = (hasSpawner) ? ((pseudoTarget != null) ? pseudoTarget.position.x >= activePoints[0].position.x && pseudoTarget.position.x <= activePoints[1].position.x : false) : true;
        bool outcome = pseudoTarget != null && targetInLimitation && !forcedAsleep;
        TargetDetected(outcome, pseudoTarget);

        return outcome;
    }

    private void TargetDetected(bool targetDetected, Transform target = null)
    {
        if (targetDetected)
        {
            TriggerOn(target, true, false);
        }
        else
        {
            StartCoroutine(ForgiveEnemy(forgivenessTime));
        }
    }

    private void TriggerOn(Transform target = null, bool wakeUp = false, bool forceWakeUp = false)
    {
        if (!isTriggered)
        {
            this.target = target;
            targetAlive = target.GetComponent<Entity>().isAlive;
            isTriggered = true;
            targetLastDetected = Time.time;

            if (wakeUp) WakeUp(forceWakeUp);
        }
    }

    private void TriggerOff()
    {
        target = null;
        isTriggered = false;
        targetAlive = false;
    }

    private IEnumerator ForgiveEnemy(float forgivenessTime)
    {
        if (isTriggered && ProcessEvaluator((float)targetLastDetected, forgivenessTime))
        {
            yield return new WaitForSeconds(forgivenessTime);
            if (TimerIncrement(targetLastDetected, forgivenessTime))
            {
                TriggerOff();
            }

            yield return new WaitForSeconds(sleepDelay - forgivenessTime);
            // Executes if the AI do sleep
            if (TimerIncrement(targetLastDetected, sleepDelay - forgivenessTime))
            {
                Sleep(false);
            }
        }
    }


    // ========================================= NPC DECISION MAKING SYSTEM =========================================
    private void NPCDecisionMakingSystem()
    {
        if (!isAlive) return;

        targetInDetection = NPCDetectionSystem();
    
        if (isTriggered)
        {
            PathFollow(); // Updates the nodal position of the target
        }
    }

    public void Sleep(bool forcedAsleep = false)
    {
        this.forcedAsleep = forcedAsleep;
        if (((!forcedAsleep && doSleep) || forcedAsleep) && isAwake)
        {
            var sleepingParticleShape = sleepParticle.shape;
            sleepingParticleShape.rotation = new Vector3(0, 0, transform.localScale.x * -45);
            isAwake = false;
            sleepParticle.Play();
        }
    }

    public void WakeUp(bool forcedWakeUp = false)
    {
        forcedAsleep = (forcedWakeUp) ? false : forcedAsleep;
        if (!forcedAsleep && !isAwake)
        {
            var sleepingParticleShape = sleepParticle.shape;
            sleepingParticleShape.rotation = new Vector3(0, 0, transform.localScale.x * -45);
            isAwake = true;
            sleepParticle.Stop();
        }
    }

    protected override void Controller()
    {
        if (!isAlive)
        {
            allowJump = false;
            inProximity = 0;
            return;
        }

        // Anti-glitching code when target is on unreacheable location
        if (targetAlive && target.gameObject.GetComponent<Beings>().isGrounded && Mathf.Abs(target.position.x - transform.position.x) < stayDistance && Mathf.Abs(target.position.y - transform.position.y) > backOffDistance)
        {
            TriggerOff();
        }

        // Vertical Parameter
        if (allowJump)
        {
            allowJump = false;
            edgeStuck = false;
            edgeStuckTime = Time.time;
        }
        else if (transform.position.x <= lastXPosition + (edgeStuckAlphaError / 2) && transform.position.x >= lastXPosition - (edgeStuckAlphaError / 2) && inProximity == 1 && boxColl.IsTouchingLayers(groundLayer) && isAwake)
        {
            if (edgeStuck)
            {
                if (TimerIncrement(edgeStuckTime, allowJumpAfter)) allowJump = true;
            }
            else
            {
                edgeStuck = true;
                edgeStuckTime = Time.time;
            }
        }

        // Horizontal Parameter
        stayDistance = backOffDistance + totalAtkRange; // Prevents the enemy to collide entirely with the target
        inProximity = (target != null && targetAlive) ? ((targetNodalDistance < backOffDistance) ? -1 : ((targetNodalDistance >= backOffDistance && targetNodalDistance <= stayDistance) ? 0 : 1)) : 1;
        if (isTriggered && target != null)
        {
            mvSpeed = _mvSpeed;
            isChasing = target.GetComponent<Beings>().isGrounded || (!target.GetComponent<Beings>().isGrounded && Mathf.Abs(target.position.x - transform.position.x) >= stayDistance);
            dirXFacing = ((isAlive) ? ((target.position.x > transform.position.x) ? 1 : -1) : 0) * ((isChasing) ? 1 : 0) * inProximity;
        }
        else
        {
            NPCWanderingSystem();
        }
        lastXPosition = transform.position.x; // Updates the last X position of the AI

        // Attack Code
        if (inProximity == 0 && !isAttacking && !isHurting && TimerIncrement(_lastAttack, totalAtkDelay))
        {
            Attack();
        }
    }

    private void NPCWanderingSystem()
    {
        if (!isAwake)
        {
            dirXFacing = 0;
            return;
        }

        // The AI might go for a walk or idle in a certain position for several seconds
        // Current task time is done
        if (TimerIncrement(awakeTaskTime, awakeTaskLimit))
        {
            mvSpeed = _mvSpeed / 3;
            awakeTaskTime = Time.time;

            awakeTask = Random.Range(0, 2);  // 0 for standing for several seconds, 1 for walking until timer stops
            awakeTaskLimit = Random.Range(3f, 6f); // Time doing task

            dirXFacing = (isAlive) ? ((awakeTask == 1) ? (new float[] { -1, 1 }[Random.Range(0, 2)]) : 0) : 0; // Negative value of the previous value of dirX
        }
        // Current task time is playing
        else
        {
            if (hasSpawner && transform.position.x <= activePoints[0].position.x)
            {
                dirXFacing = 1;
            }
            else if (hasSpawner && transform.position.x >= activePoints[1].position.x)
            {
                dirXFacing = -1;
            }
        }
    }


    // ========================================= NPC DEATH =========================================
    protected override void Die()
    {
        base.Die();
        if (hasSpawner) spawner.transform.parent.gameObject.GetComponent<Spawner>().ClearInstance(objectName, gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(detectionCenter, detectionSize);
    }
}