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

    [Header("Attack info")]
    public Vector2[] attackMovements;

    public int facingDir { get; private set; } = 1;//-1 left, 1 right
    protected bool isFacingRight = true;

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
    public Rigidbody2D rb { get; private set; }
    #endregion

    protected virtual void Awake()
    {
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
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
        rb.velocity = new Vector2(_xVelocity, yVelocity);
        FlipController(_xVelocity);
    }

    public void ZeroVelocity() => rb.velocity = Vector2.zero;
    public bool IsGroundDetected() => Physics2D.Raycast(groundCheckStartPoint.transform.position, Vector2.down, distanceGroundCheck, groundMask);
    public bool IsFaceWallDetected() => Physics2D.Raycast(wallCheckStartPoint.transform.position, Vector2.right * facingDir, distanceWallCheck, groundMask);
    
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheckStartPoint.transform.position, new Vector2(groundCheckStartPoint.transform.position.x, groundCheckStartPoint.transform.position.y - distanceGroundCheck));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(wallCheckStartPoint.transform.position, new Vector2(wallCheckStartPoint.transform.position.x + facingDir * distanceWallCheck, wallCheckStartPoint.transform.position.y));
    }
    
    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }
}
