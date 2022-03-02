using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Beings, IInteractor
{
    [Header("WEAPON ANIMATION SETTINGS", order = 1)]
    [SerializeField] private Animator wpnAnim;
    [SerializeField] private SpriteRenderer wpnSprite;
    [SerializeField] private List<Sprite> attackSprites;

    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
        GetComponent<AchievementUnlocks>().ChangeState();
    }

    protected override void Start()
    {
        base.Start();
        transform.position = new Vector2(LevelProperties.resPlatform.position.x, transform.position.y);
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!PauseMechanics.isPlaying) return;
        PassiveSkills();
        UpdateStats();

        Controller();
        Movement();
        AnimationState();
    }


    // ========================================= PLAYER METHODS =========================================
    // Damage Give
    protected override void Attack()
    {
        anim.SetTrigger("sword" + curAtkCombo.ToString());
        if (hasWeapon)
        {
            wpnAnim.SetTrigger("sword" + curAtkCombo.ToString());
            wpnAnim.speed = totalAtkSpeed / 5;
        }

        base.Attack();
        SoundManager.instance.PlayDagger();
    }

    public bool InteractorColliderConditions(Collider2D coll)
    {
        return true;
    }


    // ========================================= CONTROLLER METHODS =========================================
    protected override void Controller()
    {
        // Vertical Movement
        if (isAlive)
        {
            if (Input.GetButtonDown("Jump") && !allowJump) StartCoroutine(InputJumpAlpha());
        }
        else
        {
            allowJump = false;
        }

        // Horizontal Movement
        dirXFacing = (isAlive) ? ((isAttacking || isHurting) ? dirXFacing : Input.GetAxisRaw("Horizontal")) : 0;


        if (isAlive)
        {
            // Attack Code
            if (Input.GetKeyDown(KeyCode.Mouse1) && !isAttacking && !isHurting && TimerIncrement(_lastAttack, totalAtkDelay) && totalStamCost <= curStam)
            {
                Attack();
            }
        }
    }

    // Jump Alpha
    private IEnumerator InputJumpAlpha()
    {
        if (!allowJump)
        {
            allowJump = true;
            StartCoroutine(InputJumpDecay());
            yield return new WaitUntil(() => isGrounded);
            yield return null; // Skip 1 frame before turning back to false
            allowJump = false;
        }
    }

    private IEnumerator InputJumpDecay()
    {
        yield return new WaitForSeconds(.25f);
        allowJump = false;
    }


    // ========================================= NPC DEATH =========================================
    // Executes after death animation and instance clearing on memory
    protected override void Die()
    {
        base.Die();
        InventoryDeathSpill();
    }

    // Executes right before entity to be destroyed
    protected override void OnEntityDestroy()
    {
        base.OnEntityDestroy();
        gameMechanics.PlayerDied();
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, totalAtkRange);
    }
}