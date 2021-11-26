using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private BoxCollider2D collider;
    private Animator animator;
    private SpriteRenderer sprite;
    private float dirX;
    
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] private LayerMask groundLayer;

    // Attack Parameters
    public float attackRange = 0.3f;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage = 10;

    private enum MovementAnim { idle, run, jump, fall };

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * speed, body.velocity.y);

        if(Input.GetButtonDown("Jump") && IsGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }
        
        AnimationState();

        // Attack Code
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
    }

    private void AnimationState()
    {
        MovementAnim state;

        if (dirX > 0f) 
        {
            state = MovementAnim.run;
            sprite.flipX = false;
        }
        else if (dirX < 0f)
        {
            state = MovementAnim.run;
            sprite.flipX = true;
        }
        else
        {
            state = MovementAnim.idle;
        }

        if (body.velocity.y > .99f)
        {
            state = MovementAnim.jump;
        }
        else if (body.velocity.y < -1f)
        {
            state = MovementAnim.fall;
        }

        animator.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        return Physics2D.BoxCast(collider.bounds.center, collider.bounds.size, 0f, Vector2.down, .1f, groundLayer);
    }

    private void Attack()
    {
        // Play Attack anim
        animator.SetTrigger("attack");
        // Detect enemy in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        // Damage
        foreach(Collider2D enemy in hitEnemies)
        {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}

