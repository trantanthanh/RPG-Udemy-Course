using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float groundCheckDistance;
    [SerializeField] LayerMask groundMask;

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

        UpdateState();
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
        myRigid.velocity = new Vector2(inputHorizontal * moveSpeed, myRigid.velocity.y);
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
                    myAnimator.SetTrigger("Jump");
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
                        myAnimator.SetTrigger("Landed");
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
