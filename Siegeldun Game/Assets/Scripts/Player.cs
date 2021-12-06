using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D body;
    private CapsuleCollider2D coll;
    private Animator animator;
    private SpriteRenderer sprite;
    private float dirX;
    private bool facingRight = true;
    
    [Header("Movement Parameters")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] private LayerMask groundLayers;

    [Header("Battle Parameters")]
    [SerializeField] Transform attackPoint; 
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] float attackRange = 0.3f;
    [SerializeField] float attackDamage = 10;
    [SerializeField] float maxHealth = 100;
    public float currentHealth;
    private bool attacking = false;

    private enum MovementAnim { idle, run, jump, fall };
    private MovementAnim state;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        coll = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        currentHealth = maxHealth;
    }

    private void Update()
    {
        dirX = Input.GetAxisRaw("Horizontal");
        body.velocity = new Vector2(dirX * speed, body.velocity.y);

        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(groundLayers))
        {
            
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        }

        AnimationState();

        // Attack Code
        attacking = GetComponent<SpriteRenderer>().sprite.ToString().Substring(0, 7) == "Noob_Attack"; // Anti-spamming code
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
            if (dirX > 0f && !facingRight)
                Flip();
            else if (dirX < 0 && facingRight)
                Flip();
        }

        if (body.velocity.y > .99f)
        {
            state = MovementAnim.jump;
        }
        else if (body.velocity.y < -1f)
        {
            state = MovementAnim.fall;
        }

<<<<<<< Updated upstream
        animator.SetInteger("state", (int)state);
    }
=======
        // Pseudo Damage Taken 
        if (Input.GetKeyDown(KeyCode.Q))
        {
            TakeDamage(Random.Range(5, 10), (sprite.flipX) ? 1 : -1, kbHorDisplacement);
            // Pseudo Hurt Anim
            anim.SetTrigger("hurt");
        }
>>>>>>> Stashed changes

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
            enemy.GetComponent<EnemyAIv2>().TakeDamage(attackDamage);
        }
    }

    private void Flip()
    {
		facingRight = !facingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
    }

    void OnDrawGizmosSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
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

