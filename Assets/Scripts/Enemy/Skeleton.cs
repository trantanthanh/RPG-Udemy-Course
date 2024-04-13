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
        inputHorizontal = 1.0f * Mathf.Sign(transform.localScale.x);
        base.Movement();
    }
}
