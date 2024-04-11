using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float jumpForce = 12f;
    [SerializeField] float groundCheckDistance;//Distance from raycast to ground
    [SerializeField] LayerMask groundMask;

    [Header("Dash Info")]
    [SerializeField] float dashSpeed = 30f;
    [SerializeField] float dashDuration = 0.3f;
    float dashTime = 0f;
    [SerializeField] float dashCooldown = 3.0f;
    float timeNextDash = 0f;
    bool isDashing = false;

    float jumpCooldown = 0.3f;
    float timeNextJump = 0f;
    bool isGrounded = false;

    Rigidbody2D myRigid;
    Animator myAnimator;

    enum PlayerState
    {
        NONE,
        IDLE,
        RUN,
        DASH,
        ATTACK,
        JUMP
    }

    PlayerState playerSate;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        myAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckIsOnGround();

        Movement();
        CheckJump();
        CheckDash();

        UpdateState();
        myAnimator.SetFloat("yVelocity", myRigid.velocity.y);
    }

    private void CheckDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > timeNextDash)
        {
            isDashing = true;
            SetState(PlayerState.DASH);
            dashTime = Time.time + dashDuration;
            timeNextDash = Time.time + dashCooldown;
        }
    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && CanJump())
        {
            timeNextJump = Time.time + jumpCooldown;
            isGrounded = false;
            myAnimator.SetBool("isGrounded", isGrounded);//make sure update imediately isGrounded in animator
            isDashing = false;//Stop dash when jump from ground
            SetState(PlayerState.JUMP);
            myRigid.velocity = new Vector2(myRigid.velocity.x, jumpForce);
        }
    }

    private void Movement()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
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
                SetState(PlayerState.RUN);
            }
            CheckFlipSprite();
        }
        else
        {
            isDashing = false;//if won't move, stop dash
            if (isGrounded && !IsState(PlayerState.JUMP))
            {
                SetState(PlayerState.IDLE);
            }
        }
    }

    bool CanJump()
    {
        return Time.time > timeNextJump;
    }

    void CheckIsOnGround()
    {
        if (IsState(PlayerState.JUMP))
        {
            if (!CanJump()) return;//Check cooldown to next jump, this condition to prevent multiple jump in many frames in short time
        }
        RaycastHit2D hitInfo = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, groundMask);
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
    void CheckFlipSprite()
    {
        transform.localScale = new Vector2(Mathf.Sign(myRigid.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    void SetState(PlayerState state)
    {
        if (playerSate == state) return;
        playerSate = state;
        switch (playerSate)
        {
            case PlayerState.NONE:
                {
                    break;
                }
            case PlayerState.IDLE:
                {
                    myAnimator.SetBool("isMoving", false);
                    myAnimator.SetBool("isDashing", false);
                    break;
                }
            case PlayerState.JUMP:
                {
                    myAnimator.SetBool("isMoving", false);
                    myAnimator.SetBool("isDashing", false);
                    break;
                }
            case PlayerState.RUN:
                {
                    myAnimator.SetBool("isMoving", true);
                    myAnimator.SetBool("isDashing", false);
                    break;
                }
            case PlayerState.DASH:
                {
                    myAnimator.SetBool("isMoving", false);
                    myAnimator.SetBool("isDashing", true);
                    break;
                }
            case PlayerState.ATTACK:
                {
                    break;
                }
        }
    }

    bool IsState(PlayerState state)
    {
        return playerSate == state;
    }

    void UpdateState()
    {
        switch (playerSate)
        {
            case PlayerState.NONE:
                {
                    break;
                }
            case PlayerState.IDLE:
                {
                    break;
                }
            case PlayerState.JUMP:
                {
                    break;
                }
            case PlayerState.RUN:
                {
                    break;
                }
            case PlayerState.DASH:
                {
                    break;
                }
            case PlayerState.ATTACK:
                {
                    break;
                }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
    }
}
