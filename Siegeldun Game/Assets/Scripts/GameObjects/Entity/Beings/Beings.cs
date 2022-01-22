using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBoost
{
    public string boostName { get; private set; }
    public Vector2 speedBoost { get; private set; }

    public SpeedBoost(string boostName, Vector2 speedBoost)
    {
        this.boostName = boostName;
        BoostAdjustment(speedBoost);
    }

    public void BoostAdjustment(Vector2 speedBoost)
    {
        this.speedBoost = speedBoost;
    }
}

public abstract class Beings : Entity
{
    // ========================================= BEINGS PROPERTIY SCALING =========================================
    private string _entityType = "Beings";
    public override string entityType { get { return _entityType; } }




    // ========================================= MOVEMENT PROPERTIES =========================================
    [Header("MOVEMENT SETTINGS", order = 1)]
    [SerializeField] protected bool doMoveX = false;
    [SerializeField] protected float mvSpeed = 0;
    protected float dirXFacing = 0, dirXFinal = 0, runVelocity = 0;

    private const float _slowDownConst = 0.9f;
    public bool isGrounded;// Updates

    [SerializeField] protected LayerMask groundLayer;

    [SerializeField] protected bool doMoveY = false;
    [SerializeField] protected bool allowJump = false;
    [SerializeField] protected float jumpForce = 0;
    protected float dirYFinal = 0, jumpVelocity = 0;

    protected Vector2 totalSpeed; // Updates
    protected Dictionary<string, SpeedBoost> speedBoost = new Dictionary<string, SpeedBoost>();

    protected enum MovementAnim { idle, run, jump, fall };
    protected MovementAnim state;


    protected abstract void Controller();

    protected void Movement()
    {
        totalSpeed.Set(mvSpeed + TotalBoost("MVSpeed"), jumpForce + TotalBoost("JumpHeight"));

        // Horizontal Movement
        float slowDown = dirXFinal * _slowDownConst;
        dirXFinal = (!isAttacking) ? dirXFacing : slowDown; // Front movement with a slowdown effect when attacking
        runVelocity = (isAlive) ? dirXFinal * totalSpeed.x + rcvKbDisplacement : slowDown;
        runVelocity = (doMoveX) ? runVelocity : 0;

        // Vertical Movement
        if (capColl.IsTouchingLayers(groundLayer) || capColl.IsTouchingLayers(enemyLayer) && !isGrounded) StartCoroutine(GroundCheckAlpha());
        float freeFalling = (0f < rBody.velocity.y && rBody.velocity.y < 0.001f) ? 0f : rBody.velocity.y;
        dirYFinal = (allowJump && isGrounded) ? jumpForce : freeFalling;
        jumpVelocity = (isAlive) ? dirYFinal : freeFalling;
        jumpVelocity = (doMoveY) ? jumpVelocity : 0;

        rBody.velocity = new Vector2(runVelocity, jumpVelocity);
        deadOnGround = !isAlive && (rBody.velocity == new Vector2(0, 0)) && isGrounded;
    }

    // Grounded Alpha
    private IEnumerator GroundCheckAlpha()
    {
        if (!isGrounded)
        {
            isGrounded = true;
            StartCoroutine(GroundCheckDecay());
            yield return new WaitUntil(() => allowJump);
            yield return null; // Skip 1 frame before turning back to false
            isGrounded = false;
        }
    }

    private IEnumerator GroundCheckDecay()
    {
        yield return new WaitForSeconds(.1f);
        isGrounded = false;
    }




    // ========================================= ANIMATION METHODS =========================================
    protected override void AnimationState()
    {
        curSpriteName = sprite.sprite.name;
        curAnimStateName = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Substring(entityName.Length + 1);
        if (curAnimStateName == "Hurt")
        {
            isHurting = true;
        }
        else
        {
            isHurting = false;
        }

        if (curAnimStateName == "Attack")
        {
            isAttacking = true;
            anim.speed = totalAtkSpeed;
        }
        else
        {
            isAttacking = false;
        }

        if (isAlive)
        {
            if (!isHurting && !isAttacking && !isDying)
            {
                // Vertical Movement Animation
                if (jumpVelocity > .99f)
                {
                    state = MovementAnim.jump;
                }
                else if (jumpVelocity < -1f)
                {
                    state = MovementAnim.fall;
                }
                else
                {
                    // Horizontal Movement Animation
                    runAnimationSpeed = totalSpeed.x / mvSpeed;
                    if (dirXFinal == 0)
                    {
                        state = MovementAnim.idle;
                        anim.speed = animationSpeed;
                    }
                    else
                    {
                        state = MovementAnim.run;
                        anim.speed = runAnimationSpeed;
                        transform.localScale = new Vector3(((dirXFinal > 0f) ? 1 : -1) * spriteDefaultFacing, 1, 1);
                    }
                }
            }
            anim.SetInteger("state", (int)state);
        }
    }
}