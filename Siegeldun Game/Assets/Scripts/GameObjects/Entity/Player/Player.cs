using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Beings
{
    // ========================================= Entity Properties =========================================
    private bool _isInstanceLimited = true;
    public override bool isInstanceLimited { get { return _isInstanceLimited; } }

    private int _maxEachEntityInField = 1;
    public override int maxEachEntityInField { get { return _maxEachEntityInField; } }

    private string _entityName = "Player";
    public override string entityName { get { return _entityName; }}
    public override string objectName { get { return _entityName; } }




    // ========================================= UNITY MAIN METHODS =========================================
    protected override void Awake()
    {
        base.Awake();
        defaultPower.SetValues(15f, .3f, .5f, .3f, 1, 0, 0, 0);
    }

    // Update is called once per frame
    protected void Update()
    {
        UpdateStats();
        PassiveSkills();
        HpBarUIUpdate();
        StamBarUIUpdate();

        Controller();
        Movement();
        AnimationState();
    }


    // ========================================= PLAYER METHODS =========================================
    // Damage Give
    protected override void Attack()
    {
        int attackID = Random.Range(-9999, 10000);
        anim.SetTrigger("sword" + curAtkCombo.ToString());
        curStam -= totalStamCost;
        _lastAttack = Time.time;
        if (doAtkCombo)
        {
            curAtkCombo = (curAtkCombo == 3) ? 1 : curAtkCombo + 1;
            StartCoroutine(ComboTimer());
        }

        // Collision Sensing
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, totalAtkRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<IDamageable>() == null) continue;

            int kbDir = (enemy.transform.position.x > transform.position.x) ? 1 : -1;
            enemy.GetComponent<IDamageable>().TakeDamage(attackID, kbDir, atkStatsProp);
        }
    }

    protected IEnumerator ComboTimer()
    {
        yield return new WaitForSeconds(totalAtkSpeed * 2);
        if (TimerIncrement(_lastAttack, totalAtkSpeed * 2)) curAtkCombo = 1;
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
        dirXFacing = (isAlive) ? Input.GetAxisRaw("Horizontal") : 0;


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