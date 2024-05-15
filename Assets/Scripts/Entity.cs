using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] protected float jumpForce = 12f;
    protected float defaultMoveSpeed = 0f;

    [Header("Attack info")]
    public Vector2[] attackMovements;
    public Transform attackCheck;
    public float attackCheckRadius;

    [Header("Knockback info")]
    [SerializeField] Vector2 knockBackPower;
    [SerializeField] float knockBackDuration = 0.07f;
    protected bool isKnocked = false;

    public int facingDir { get; private set; } = 1;//-1 left, 1 right
    protected bool isFacingRight = true;

    public bool IsFacingRight
    {
        get
        {
            return isFacingRight;
        }
    }

    public bool isBusy { get; private set; } // for delay to skip block code in frame after

    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
    }

    public float JumpForce
    {
        get
        {
            return jumpForce;
        }
    }

    #region Components
    [HideInInspector]
    public Animator animator;
    public SpriteRenderer spriteRenderer { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public EntityFx fx { get; private set; }
    public CharacterStats stats { get; private set; }
    public CapsuleCollider2D capsuleCollider { get; private set; }
    #endregion

    protected virtual void Awake()
    {
        defaultMoveSpeed = moveSpeed;
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        fx = GetComponent<EntityFx>();
        stats = GetComponent<CharacterStats>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
    }

    public virtual void DamageEffect()
    {
        fx.StartCoroutine("FlashFx");
        StartCoroutine("KnockBackHit");
    }

    protected virtual IEnumerator KnockBackHit()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockBackPower.x * (-facingDir), knockBackPower.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }

    public void Flip()
    {
        facingDir = facingDir * -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
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

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }

    public void MakeTransparent(bool _isTransparent)
    {
        if (_isTransparent)
        {
            spriteRenderer.color = Color.clear;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }

    public virtual void Die()
    {
    }
}
