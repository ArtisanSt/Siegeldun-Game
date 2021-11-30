using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// ENEMY AI SCRIPT VERSION : V2.1

public class EnemyAIv2 : MonoBehaviour
{
    private Seeker seeker;
    private Rigidbody2D rBody;
    private CapsuleCollider2D capColl;
    private Animator anim;
    private int state;
    private bool facingRight = true;

    [Header("Movement")]
    [SerializeField] float speed = 150;
    [SerializeField] float jumpForce = 600;

    [Header("Pathfinding")]
    [SerializeField] LayerMask groundLayers;
    [SerializeField] Transform target;
    [SerializeField] float jumpNodeHeight = 0.8f;
    [SerializeField] float activateDistance = 10f;
    [SerializeField] float proximity = 3f;

    [Header("Custom Behavior")]
    [SerializeField] bool jumpEnabled = true;

    [Header("Battle Parameters")]
    [SerializeField] float maxHealth = 100;
    [SerializeField] float currentHealth;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] public float cooldownTime = 2;
    [SerializeField] float attackDamage;
    [SerializeField] float attackRange = 3f;
    private float nextAttack = 0;
    
    private Path path;
    private float pathUpdateSec = 0.5f;
    private int currentWaypoint = 0;
    public bool isGrounded = false;



    // ========================================= UNITY MAIN METHODS =========================================
    // Initializes when the Player Script is called
    public void Start()
    {
        currentHealth = maxHealth;

        seeker = GetComponent<Seeker>();
        rBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        capColl = GetComponent<CapsuleCollider2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSec);
    }


    // Updates Every Physics Frame
    private void FixedUpdate()
    {
        if(TargetInDistance())
        {
            PathFollow();
        }
    }

    private void Update()
    {
        AnimationState();
    }

    private void PathFollow()
    {
        if(path == null)
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
        if(target.transform.position.x > transform.position.x && distEntity >= proximity)
        {
            rBody.velocity = new Vector2(speed * Time.deltaTime, rBody.velocity.y);
            if(!facingRight)
                Flip();
        }
        else if(target.transform.position.x < transform.position.x && distEntity >= proximity)
        {
            rBody.velocity = new Vector2(-speed * Time.deltaTime, rBody.velocity.y);
            if(facingRight)
                Flip();
        }

        // Attack if in proximity
        if(distEntity <= proximity)
        {
            Attack();
        }

        float distance = Vector2.Distance(rBody.position, path.vectorPath[currentWaypoint]);
        if (distance < proximity)
            currentWaypoint++;
    }

    private void UpdatePath()
    {
        if (TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rBody.position, target.position, OnPathComplete);
        }
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

    private void AnimationState()
    {
        if(Mathf.Abs(rBody.velocity.x) > 0)
            state = 1;
        if(Mathf.Abs(rBody.velocity.x) == 0 && Mathf.Abs(rBody.velocity.y) == 0)
            state = 0;

        anim.SetInteger("state", state);
    }

    private void Flip()
    {
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

    private void Attack()
    {
        if(Time.time - nextAttack < cooldownTime)
        {
            return;
        }
        nextAttack = Time.time;

        anim.SetTrigger("attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Player>().TakeDamage(attackDamage, (enemy.GetComponent<Player>().sprite.flipX) ? 1 : -1, enemy.GetComponent<Player>().kbHorDisplacement);
        }
        
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Enemy dead");
    }
}