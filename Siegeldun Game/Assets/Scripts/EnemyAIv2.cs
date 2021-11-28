using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

// ENEMY AI SCRIPT VERSION : V2.1

public class EnemyAIv2 : MonoBehaviour
{
    private Seeker seeker;
    private Rigidbody2D rb;
    private CapsuleCollider2D coll;
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
    
    private float pathUpdateSec = 0.5f;
    public bool isGrounded = false;



    private Path path;
    private int currentWaypoint = 0;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<CapsuleCollider2D>();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSec);
    }

    private void FixedUpdate()
    {
        if(TargetInDistance())
        {
            PathFollow();
        }

        AnimationState();
    }

    private void PathFollow()
    {
        if(path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
            return;
        
        // Grounded Check
        isGrounded = coll.IsTouchingLayers(groundLayers);

        // Direction Calculation
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        if (jumpEnabled && isGrounded)
        {
            if (direction.y > jumpNodeHeight)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * Time.deltaTime);
            }
        }

        // Movement
        float distEntity = Mathf.Abs(target.transform.position.x - transform.position.x);
        if(target.transform.position.x > transform.position.x && distEntity >= proximity)
        {
            rb.velocity = new Vector2(speed * Time.deltaTime, rb.velocity.y);
            if(!facingRight)
                Flip();
        }
        else if(target.transform.position.x < transform.position.x && distEntity >= proximity)
        {
            rb.velocity = new Vector2(-speed * Time.deltaTime, rb.velocity.y);
            if(facingRight)
                Flip();
        }

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < proximity)
            currentWaypoint++;
    }

    private void UpdatePath()
    {
        if (TargetInDistance() && seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
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
        if(Mathf.Abs(rb.velocity.x) > 0)
            state = 1;
        if(Mathf.Abs(rb.velocity.x) == 0 && Mathf.Abs(rb.velocity.y) == 0)
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
        anim.SetTrigger("attack");
    }
}