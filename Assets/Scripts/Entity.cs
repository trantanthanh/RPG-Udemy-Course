using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Moving Info")]
    [SerializeField] protected float moveSpeed = 8f;
    [SerializeField] protected float jumpForce = 12f;
    protected float jumpCooldown = 0.3f;
    protected float timeNextJump = 0f;
    protected float inputHorizontal = 0f;

    [Header("Dash Info")]
    [SerializeField] protected float dashSpeed = 30f;
    [SerializeField] protected float dashDuration = 0.3f;
    protected float dashTime = 0f;
    [SerializeField] protected float dashCooldown = 3.0f;
    protected float timeNextDash = 0f;
    protected bool isDashing = false;

    [Header("Collision check")]
    [SerializeField] Transform groundCheckStartPoint;
    [SerializeField] Transform wallCheckStartPoint;
    [SerializeField] float groundCheckDistance;//Distance from raycast to ground
    [SerializeField] float wallCheckDistance;//Distance from raycast to wall, use same ground mask
    [SerializeField] protected float timeFlipCooldown = 0.2f;//Prevent call flip too much in short time
    protected float timeNextFlip = 0f;
    [SerializeField] LayerMask groundMask;
    protected bool isGrounded = false;
    protected bool isFaceWall = false;

    [Header("Attack Info")]
    [SerializeField] protected int numOfAnimsAttack = 3;
    [Tooltip("Time between 2 times attack in row must under this value time to increase combo attack")]
    [SerializeField] protected private float comboTime = 1.2f;
    protected float comboTimeCounter = 0f;
    protected bool isAttacking = false;
    protected int comboCounter = 0;

    protected Rigidbody2D myRigid;
    protected Animator myAnimator;

    protected enum EntityState
    {
        NONE,
        IDLE,
        RUN,
        DASH,
        ATTACK,
        JUMP
    }

    protected EntityState entityState;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CheckIsOnGround();
        CheckFlipSprite();

        Movement();
        UpdateState();
        myAnimator.SetFloat("yVelocity", myRigid.velocity.y);
    }

    protected void Attack()
    {
        if (!isGrounded || isAttacking) return;
        SetState(EntityState.ATTACK);
    }

    public void AttackOver()
    {
        SetState(EntityState.IDLE);
    }

    protected void Dash()
    {
        if (!isAttacking && Time.time > timeNextDash)
        {
            isDashing = true;
            SetState(EntityState.DASH);
            dashTime = Time.time + dashDuration;
            timeNextDash = Time.time + dashCooldown;
        }
    }

    protected void Jump()
    {
        if (isGrounded && CanJump())
        {
            timeNextJump = Time.time + jumpCooldown;
            isGrounded = false;
            myAnimator.SetBool("isGrounded", isGrounded);//make sure update imediately isGrounded in animator
            isDashing = false;//Stop dash when jump from ground
            SetState(EntityState.JUMP);
            myRigid.velocity = new Vector2(myRigid.velocity.x, jumpForce);
        }
    }

    protected virtual void Movement()
    {
        //need set inputHorizontal in derived class, inputHorizontal > 0 (moving right) inputHorizontal < 0 (moving left)
        if (isAttacking) return;
        float _moveSpeed = moveSpeed;
        if (isDashing)
        {
            _moveSpeed = dashSpeed;
            if (Time.time > dashTime)
            {
                //Dash time has expired, just stop dash, state will check to change below
                isDashing = false;
                myAnimator.SetBool("isDashing", false);
            }
        }
        myRigid.velocity = new Vector2(inputHorizontal * _moveSpeed, (isDashing && !isGrounded) ? 0 : myRigid.velocity.y);
        if (myRigid.velocity.x != 0)
        {
            if (isGrounded && !isDashing)
            {
                SetState(EntityState.RUN);
            }
        }
        else
        {
            isDashing = false;//if won't move, stop dash
            myAnimator.SetBool("isDashing", false);//make sure update boolean isDashing immediately to animator
            if (isGrounded && !IsState(EntityState.JUMP))
            {
                SetState(EntityState.IDLE);
            }
        }
    }

    protected virtual void CheckFlipSprite()
    {
        if (myRigid.velocity.x != 0)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigid.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }

    protected void Flip()
    {
        if (Time.time > timeNextFlip)
        {
            timeNextFlip = Time.time + timeFlipCooldown;
            transform.localScale = new Vector2(-1.0f * transform.localScale.x, transform.localScale.y);
        }
    }

    protected void CheckIsOnGround()
    {
        if (IsState(EntityState.JUMP))
        {
            if (!CanJump()) return;//Check cooldown to next jump, this condition to prevent multiple jump in many frames in short time
        }
        RaycastHit2D hitInfo = Physics2D.Raycast(groundCheckStartPoint.position, Vector2.down, groundCheckDistance, groundMask);
        if (hitInfo.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        myAnimator.SetBool("isGrounded", isGrounded);
    }

    protected void CheckFaceWall()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(wallCheckStartPoint.position, Mathf.Sign(transform.localScale.x) * Vector2.right, wallCheckDistance, groundMask);
        if (hitInfo.collider != null)
        {
            isFaceWall = true;
        }
        else
        {
            isFaceWall = false;
        }
    }

    protected void SetState(EntityState state)
    {
        if (entityState == state) return;
        entityState = state;
        switch (entityState)
        {
            case EntityState.NONE:
                {
                    break;
                }
            case EntityState.IDLE:
                {
                    myAnimator.SetBool("isMoving", false);
                    myAnimator.SetBool("isDashing", false);
                    isAttacking = false;
                    myAnimator.SetBool("isAttacking", isAttacking);
                    break;
                }
            case EntityState.JUMP:
                {
                    myAnimator.SetBool("isMoving", false);
                    myAnimator.SetBool("isDashing", false);
                    break;
                }
            case EntityState.RUN:
                {
                    myAnimator.SetBool("isMoving", true);
                    myAnimator.SetBool("isDashing", false);
                    break;
                }
            case EntityState.DASH:
                {
                    myAnimator.SetBool("isMoving", false);
                    myAnimator.SetBool("isDashing", true);
                    break;
                }
            case EntityState.ATTACK:
                {
                    isAttacking = true;
                    myRigid.velocity = Vector2.zero;//stop movingW
                    myAnimator.SetBool("isMoving", false);
                    myAnimator.SetBool("isDashing", false);
                    if (Time.time > comboTimeCounter)
                    {
                        comboCounter = 0;
                    }
                    comboTimeCounter = Time.time + comboTime;
                    myAnimator.SetBool("isAttacking", isAttacking);
                    myAnimator.SetInteger("comboCounter", comboCounter);
                    comboCounter = ++comboCounter % numOfAnimsAttack;
                    break;
                }
        }
    }

    void UpdateState()
    {
        switch (entityState)
        {
            case EntityState.NONE:
                {
                    break;
                }
            case EntityState.IDLE:
                {
                    break;
                }
            case EntityState.JUMP:
                {
                    break;
                }
            case EntityState.RUN:
                {
                    break;
                }
            case EntityState.DASH:
                {
                    break;
                }
            case EntityState.ATTACK:
                {
                    break;
                }
        }
    }

    protected bool IsState(EntityState state)
    {
        return entityState == state;
    }

    protected bool CanJump()
    {
        return !isAttacking && Time.time > timeNextJump;
    }


    protected void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheckStartPoint.position, new Vector3(groundCheckStartPoint.position.x, groundCheckStartPoint.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheckStartPoint.position, new Vector3(wallCheckStartPoint.position.x + Mathf.Sign(transform.localScale.x) * wallCheckDistance, wallCheckStartPoint.position.y));
    }
}
