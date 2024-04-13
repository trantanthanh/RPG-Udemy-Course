using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Entity
{
    protected override void Update()
    {
        base.Update();
    }

    protected override void Movement()
    {
        if (!isGrounded) Flip();
        inputHorizontal = 1.0f * Mathf.Sign(transform.localScale.x);
        base.Movement();
    }

    void Flip()
    {
        transform.localScale = new Vector2(-1.0f * transform.localScale.x, transform.localScale.y);
    }
}
