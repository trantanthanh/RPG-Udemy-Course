using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public class Entity : MonoBehaviour
{
    [Header("Collision check")]
    [SerializeField] protected GameObject groundCheckStartPoint;
    [SerializeField] protected GameObject wallCheckStartPoint;
    [SerializeField] protected float distanceGroundCheck;
    [SerializeField] protected float distanceWallCheck;
    [SerializeField] protected LayerMask groundMask;

    [Header("Move info")]
    [SerializeField] protected float moveSpeed = 8f;
    [SerializeField] protected float moveSpeedFast = 12f;
    [SerializeField] protected float jumpForce = 12f;
    protected float currentMoveSpeed = 0f;
    protected float defaultMoveSpeed = 0f;
    protected float defaultJumpForce = 0f;

    [Header("Attack info")]
    public Vector2[] attackMovements;
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("Knockback info")]
    [SerializeField] Vector2 knockBackPower;
    [SerializeField] float knockBackDuration = 0.07f;
    protected bool isKnocked = false;
    protected bool isAlive = true;

    public int knockBackDir { get; private set; }

    public int facingDir { get; private set; } = 1;//-1 left, 1 right
    protected bool isFacingRight = true;

    public bool IsFacingRight => isFacingRight;

    public bool isBusy { get; private set; } // for delay to skip block code in frame after

    public float CurrentMoveSpeed
    {
        get
        {
            return currentMoveSpeed;
        }
        set
        {
            currentMoveSpeed = value;
        }
    }

    public float NormalSpeed => moveSpeed;
    public float FastSpeed => moveSpeedFast;

    public float JumpForce => jumpForce;

    #region Components
    [HideInInspector]
    public Animator animator;
    public SpriteRenderer spriteRenderer { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D capsuleCollider { get; private set; }
    #endregion

    public System.Action onFlipped;

    protected virtual void Reset()
    {
        //reset to default value
        distanceGroundCheck = 0.19f;
        distanceWallCheck = 0.77f;

        moveSpeed = 8f;
        moveSpeedFast = 12f;
        jumpForce = 12f;

        attackCheckRadius = 0.8f;

        knockBackPower = new Vector2(5, 9);
        knockBackDuration = 0.07f;
    }

    protected virtual void Awake()
    {
        currentMoveSpeed = moveSpeed;//normal speed
        BackupMoveSpeedAndJump();
    }

    private void BackupMoveSpeedAndJump()
    {
        defaultMoveSpeed = currentMoveSpeed;
        defaultJumpForce = jumpForce;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        isAlive = true;
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        stats = GetComponent<CharacterStats>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();

        if (transform.rotation.eulerAngles.y == 180)
        {
            //Debug.Log("facing left");
            isFacingRight = false;
            facingDir = -1;
            onFlipped?.Invoke();
        }
        else
        {
            //Debug.Log("facing right");
            isFacingRight = true;
            facingDir = 1;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public virtual void SetupKnockbackDir(Transform _damageDirection)
    {
        if (_damageDirection.position.x > transform.position.x)
        {
            knockBackDir = 1;
        }
        else if (_damageDirection.position.x < transform.position.x)
        {
            knockBackDir = -1;
        }

        //Debug.Log("knockBackDir " + knockBackDir);
    }

    public virtual void DamageImpact() => StartCoroutine(KnockBackHit());

    public virtual void DamageEffect()
    {

    }

    protected virtual IEnumerator KnockBackHit()
    {
        isKnocked = true;
        //rb.velocity = new Vector2(knockBackPower.x * (-facingDir), knockBackPower.y);
        rb.velocity = new Vector2(knockBackPower.x * (-knockBackDir), knockBackPower.y);
        yield return knockBackDuration.WaitForSeconds();
        isKnocked = false;
    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);

        onFlipped?.Invoke();
    }

    public void FlipController(float _xVelocity)
    {
        if (_xVelocity < 0 && isFacingRight || _xVelocity > 0 && !isFacingRight)
        {
            Flip();
        }
    }
    public void SetVelocity(float _xVelocity, float yVelocity)
    {
        if (isKnocked) return;
        rb.velocity = new Vector2(_xVelocity, yVelocity);
        FlipController(_xVelocity);
    }

    public void SetZeroVelocity() => rb.velocity = Vector2.zero;
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheckStartPoint.transform.position, Vector2.down, distanceGroundCheck, groundMask);
    public bool IsFaceWallDetected() => Physics2D.Raycast(wallCheckStartPoint.transform.position, Vector2.right * facingDir, distanceWallCheck, groundMask);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheckStartPoint.transform.position, new Vector2(groundCheckStartPoint.transform.position.x, groundCheckStartPoint.transform.position.y - distanceGroundCheck));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheckStartPoint.transform.position, new Vector2(wallCheckStartPoint.transform.position.x + facingDir * distanceWallCheck, wallCheckStartPoint.transform.position.y));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackCheckRadius);
    }

    public void BusyFor(float _duration) => StartCoroutine(CoroutineBusyFor(_duration));

    IEnumerator CoroutineBusyFor(float _seconds)
    {
        isBusy = true;
        yield return _seconds.WaitForSeconds();
        isBusy = false;
    }

    public virtual void SlowEntityBy(float _slowPercentage, float _duration)
    {
        BackupMoveSpeedAndJump();
        animator.speed *= (1 - _slowPercentage);
        currentMoveSpeed *= (1 - _slowPercentage);
        jumpForce *= (1 - _slowPercentage);
        Invoke(nameof(ReturnDefaultSpeed), _duration);
    }

    public virtual void ReturnDefaultSpeed()
    {
        animator.speed = 1;
        currentMoveSpeed = defaultMoveSpeed;
        jumpForce = defaultJumpForce;
    }

    public virtual void KillEntity()
    {
        if (isAlive)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        isAlive = false;
    }

    
}
