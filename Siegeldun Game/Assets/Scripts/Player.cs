using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private CapsuleCollider2D collider;
    private Animator animator;
    private SpriteRenderer sprite;
    private float dirX;
    
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] private LayerMask groundLayers;

    // Attack Parameters
    private bool attacking = false;
    public float attackRange = 0.3f;
    [SerializeField] Transform attackPoint; 
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage = 10;

    private enum MovementAnim { idle, run, jump, fall };
    private MovementAnim state;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * speed, body.velocity.y);

        if (Input.GetButtonDown("Jump") && collider.IsTouchingLayers(groundLayers))
        {
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }

        AnimationState();


        // Attack Code
        attacking = GetComponent<SpriteRenderer>().sprite.ToString().Substring(0, 11) == "Noob_Attack"; // Anti-spamming code
        if (Input.GetKeyDown(KeyCode.Mouse0) && attacking == false)
        {
            Attack();
        }
    }

    private void AnimationState()
    {
        if (dirX == 0)
        {
            state = MovementAnim.idle;
        }
        else
        {
            state = MovementAnim.run;
            if (dirX > 0f)
                sprite.flipX = false;
            else
                sprite.flipX = true;
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

    private void Attack()
    {
        attacking = true;
        // Play Attack anim
        animator.SetTrigger("attack");
        // Detect enemy in range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
        attacking = false;

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

