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
    public bool isBouncing = true;
    public int amountOfBounce = 4;
    private int currentBounced = 0;
    public float bounceSpeed = 20f;
    public float bounceRange = 10f;
    List<Transform> enemiesTarget = new List<Transform>();
    private int targetIndex = 0;

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

        isBouncing = true;
        targetIndex = 0;
        currentBounced = 0;
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
                player.CatchTheSword();
            }
        }

        if (isBouncing && enemiesTarget.Count > 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemiesTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemiesTarget[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                currentBounced++;
                if (currentBounced > amountOfBounce)
                {
                    isBouncing = false;
                    ReturnSword();
                }
                if (targetIndex >= enemiesTarget.Count)
                {
                    targetIndex = 0;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning) return;//don't trigger collision while returning to player

        if (isBouncing)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                if (enemiesTarget.Count <= 0)
                {
                    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, bounceRange);
                    foreach (Collider2D hit in hits)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                        {
                            enemiesTarget.Add(hit.transform);
                        }
                    }
                }


            }
            if (enemiesTarget.Count < 2)
            {
                isBouncing = false;
            }
        }
        StuckInto(collision);
    }

    private void StuckInto(Collider2D collision)
    {
        circleCollider.enabled = false;

        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing) return;
        animator.SetBool("Rotation", false);
        canRotate = false;
        transform.parent = collision.transform;
    }

    public void ReturnSword()
    {
        enemiesTarget.Clear();
        animator.SetBool("Rotation", true);
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
        isBouncing = false;
    }
}
