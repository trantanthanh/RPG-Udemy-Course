using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float jumpForce = 12f;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;

    [Header("Dash Info")]
    [SerializeField] float dashSpeed = 30f;
    [SerializeField] float dashDuration = 0.3f;
    float dashTime = 0f;
    [SerializeField] float timeBetweenDash = 3.0f;
    float timeNextDash = 0f;
    bool isDashing = false;

    float timeBetweenJump = 0.3f;
    float timeNextJump = 0f;
    bool isGrounded = false;

    Rigidbody2D myRigid;
    Animator myAnimator;

    enum PlayerState
    {
        NONE,
        IDLE,
        RUN,
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
        if (isGrounded && Input.GetKeyDown(KeyCode.LeftShift) && Time.time > timeNextDash)
        {
            isDashing = true;
            dashTime = Time.time + dashDuration;
            timeNextDash = Time.time + timeBetweenDash;
        }
    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && CanJump())
        {
            timeNextJump = Time.time + timeBetweenJump;
            isGrounded = false;
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
                isDashing = false;
            }
        }
        myRigid.velocity = new Vector2(inputHorizontal * _moveSpeed, myRigid.velocity.y);
        if (myRigid.velocity.x != 0)
        {
            if (isGrounded)
            {
                SetState(PlayerState.RUN);
            }
            CheckFlipSprite();
        }
        else
        {
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
            if (!CanJump()) return;
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
                    break;
                }
            case PlayerState.JUMP:
                {
                    myAnimator.SetBool("isMoving", false);
                    break;
                }
            case PlayerState.RUN:
                {
                    myAnimator.SetBool("isMoving", true);
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
                    if (isGrounded && myRigid.velocity.x == 0)
                    {
                        SetState(PlayerState.IDLE);
                    }
                    break;
                }
            case PlayerState.RUN:
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
