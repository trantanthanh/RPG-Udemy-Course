using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private bool canRotate;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale)
    {
        canRotate = true;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        canRotate = false;
        circleCollider.enabled = false;
        
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }
}
