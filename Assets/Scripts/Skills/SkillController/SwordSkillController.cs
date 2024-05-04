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
    private int bounceAmount;

    [Header("Pierce info")]
    private bool isPiercing = false;
    private int pierceAmount;

    public float bounceSpeed = 20f;
    public float bounceRange = 10f;
    private List<Transform> enemiesTarget = new List<Transform>();
    private int targetIndex = 0;

    [Header("Spin info")]
    [SerializeField] float damageRadius = 1f;
    private float maxTravelDistance;
    private float spinDuration;
    private bool spinWasStopped;
    private bool isSpinning;

    private float hitTimer;
    private float hitCooldown;

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
        if (!isPiercing)
        {
            animator.SetBool("Rotation", true);
        }
        canRotate = true;
        isReturning = false;
        rb.velocity = _dir;
        rb.gravityScale = _gravityScale;

        targetIndex = 0;
    }

    public void SetupBounce(bool _isBoucing, int _amountOfBounce)
    {
        this.isBouncing = _isBoucing;
        this.bounceAmount = _amountOfBounce;
    }

    public void SetupPierce(bool _isPiercing, int _amountOfPiercing)
    {
        this.isPiercing = _isPiercing;
        this.pierceAmount = _amountOfPiercing;
    }

    public void SetupSpin(bool _isSpinning, float _maxTravelDistance, float _spinDuration, float _hitCooldown)
    {
        spinWasStopped = false;
        this.isSpinning = _isSpinning;
        this.maxTravelDistance = _maxTravelDistance;
        this.spinDuration = _spinDuration;
        this.hitCooldown = _hitCooldown;
        hitTimer = 0;
    }

    private void Update()
    {
        if (canRotate)
        {
            transform.right = rb.velocity;//make the sword rotate to direction throw
        }

        UpdateReturningSwordToPlayer();

        UpdateBouncingLogic();
        UpdateSpinLogic();
    }

    private void UpdateReturningSwordToPlayer()
    {
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
    }

    private void UpdateSpinLogic()
    {
        if (isSpinning)
        {
            if (!spinWasStopped && Vector2.Distance(transform.position, player.transform.position) > maxTravelDistance)
            {
                StopFlyToSpinning();
            }

            if (spinWasStopped)
            {
                spinDuration -= Time.deltaTime;
                if (spinDuration < 0)
                {
                    isSpinning = false;
                    ReturnSword();
                }
            }

            hitTimer -= Time.deltaTime;
            if (hitTimer < 0)
            {
                //Can check damage enemy here
                hitTimer = hitCooldown;

                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, damageRadius);
                foreach (Collider2D hit in hits)
                {
                    hit.GetComponent<Enemy>()?.Damage();//hit enemy
                }
            }
        }
    }

    private void StopFlyToSpinning()
    {
        spinWasStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void UpdateBouncingLogic()
    {
        if (isBouncing && enemiesTarget.Count > 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemiesTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemiesTarget[targetIndex].position) < 0.1f)
            {
                enemiesTarget[targetIndex].GetComponent<Enemy>().Damage();//Damage when reach target
                targetIndex++;
                bounceAmount--;
                if (bounceAmount < 0)
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
        if (isSpinning && collision.GetComponent<Enemy>() != null)
        {
            StopFlyToSpinning();
            return;//check damage enemy by zone
        }

        //collision.GetComponent<Enemy>()?.Damage();//temporary
        if (isPiercing && pierceAmount > 0 && collision.GetComponent<Enemy>() != null)
        {
            pierceAmount--;
            return;
        }


        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        if (isBouncing) return;

        circleCollider.enabled = false;
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
