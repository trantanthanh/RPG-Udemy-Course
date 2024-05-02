using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    [SerializeField] float returnSpeed = 12f;
    private Animator animator;
    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;

    private bool canRotate;
    private bool isReturning;

    Player player;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void SetupSword(Vector2 _dir, float _gravityScale, Player _player)
    {
        this.player = _player;
        animator.SetBool("Rotation", true);
        canRotate = true;
        isReturning = false;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;
        }

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);

            if (Vector2.Distance(transform.position, player.transform.position) < 0.5f)
            {
                animator.SetBool("Rotation", false);
                isReturning = false;
                player.ClearTheSword();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        animator.SetBool("Rotation", false);
        canRotate = false;
        circleCollider.enabled = false;
        
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        transform.parent = collision.transform;
    }

    public void ReturnSword()
    {
        animator.SetBool("Rotation", true);
        rb.isKinematic = false;
        transform.parent = null;
        isReturning = true;
    }
}
