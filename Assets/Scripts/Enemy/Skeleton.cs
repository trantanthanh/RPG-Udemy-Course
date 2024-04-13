using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Entity
{
    protected override void Update()
    {
        CheckFaceWall();
        base.Update();
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
}
