using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float jumpForce = 5f;
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
        float inputHorizontal = Input.GetAxis("Horizontal");
        myRigid.velocity = new Vector2(inputHorizontal * moveSpeed, myRigid.velocity.y);
        if (myRigid.velocity.x != 0)
        {
            SetState(PlayerState.RUN);
            CheckFlipSprite();
        }
        else
        {
            SetState(PlayerState.IDLE);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetState(PlayerState.JUMP);
            myRigid.velocity = new Vector2(myRigid.velocity.x, jumpForce);
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
}
