using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Beings, IInteractor
{
    // ========================================= Entity Properties =========================================
    /*private bool _isInstanceLimited = true;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 1;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _entityName = "Player";
    public override string entityName { get { return _entityName; }}
    public override string objectName { get { return _entityName; } }
*/



    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
    }

    // Update is called once per frame
    protected void Update()
    {
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

        base.Attack();
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
    protected override void Die()
    {
        base.Die();
        // Clear Inventory
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, totalAtkRange);
    }
}