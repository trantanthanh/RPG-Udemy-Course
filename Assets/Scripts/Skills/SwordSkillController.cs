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

    [Header("Bouce info")]
    private bool isBouncing = false;
    private int amountOfBounce;

    public float bounceSpeed = 20f;
    public float bounceRange = 10f;
    private List<Transform> enemiesTarget = new List<Transform>();
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

        targetIndex = 0;
    }

    public void SetUpBouce(bool _isBoucing, int _amountOfBounce)
    {
        this.isBouncing = _isBoucing;
        this.amountOfBounce = _amountOfBounce;
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

        UpdateBouncingLogic();
    }

    private void UpdateBouncingLogic()
    {
        if (isBouncing && enemiesTarget.Count > 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemiesTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemiesTarget[targetIndex].position) < 0.1f)
            {
                targetIndex++;
                amountOfBounce--;
                if (amountOfBounce < 0)
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

        CheckBoundList(collision);
        StuckInto(collision);
    }

    private void CheckBoundList(Collider2D collision)
    {
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
            if (enemiesTarget.Count < 2)//if below 2 enemies, don't bounce
            {
                isBouncing = false;
            }
        }
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
