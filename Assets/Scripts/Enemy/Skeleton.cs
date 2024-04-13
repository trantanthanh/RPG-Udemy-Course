using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Entity
{
    [Header("Player detection")]
    [SerializeField] float playerCheckDistance;
    [SerializeField] float rangeAttack;
    [SerializeField] float multipleSpeedWhileDetectPlayer = 1.5f;
    [SerializeField] LayerMask playerMask;
    RaycastHit2D isPlayerDetected;
    Vector2 direction;
    bool isFaceWithPlayer = false;
    Player player;
    protected override void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        base.Start();
    }
    protected override void Update()
    {
        CheckFaceWall();
        CheckFaceWithPlayer();
        RaycastPlayerDetect();
        UpdateSpeed();
        base.Update();
    }

    private void UpdateSpeed()
    {
        if (isFaceWithPlayer)
        {
            if (isPlayerDetected.distance > rangeAttack)
            {
                currentMoveSpeed = baseMoveSpeed * multipleSpeedWhileDetectPlayer;
            }
            else
            {
                currentMoveSpeed = baseMoveSpeed;
            }
        }
    }

    private void CheckFaceWithPlayer()
    {
        direction = (player.transform.position - transform.position).normalized * playerCheckDistance;
        if (Mathf.Sign(direction.x) == Mathf.Sign(transform.localScale.x))
        {
            isFaceWithPlayer = true;
        }
        else
        {
            isFaceWithPlayer = false;
        }
    }

    protected override void Movement()
    {
        if (!isGrounded || isFaceWall) Flip();
        inputHorizontal = 1.0f * Mathf.Sign(transform.localScale.x);//for auto move to forward (face direction)
        base.Movement();
    }

    protected override void CheckFlipSprite()
    {
        //don't check Flip sprite auto follow velocity cause enemy auto move
        //if some other object push this enemy, it will auto flip backward
    }

    void RaycastPlayerDetect()
    {
        if (player != null)
        {
            if (isFaceWithPlayer)
            {
                isPlayerDetected = Physics2D.Raycast(transform.position, direction, playerCheckDistance, playerMask);
            }
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        if (player != null)
        {
            Gizmos.color = Color.green;
            if (isFaceWithPlayer)
            {
                //when face direction
                Vector2 target = transform.position + (new Vector3(direction.x, direction.y));
                Gizmos.DrawLine(transform.position, target);
            }
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + Mathf.Sign(transform.localScale.x) * rangeAttack, transform.position.y));
    }
}
