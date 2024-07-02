using System.Collections;
using UnityEngine;

public class EnemyArcher : Enemy
{
    [Header("Archer Specific info")]
    public Vector2 jumpVelocity;
    [SerializeField] private float jumpCooldown = 1f;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] Transform behindGroundCheckStartpoint;
    [SerializeField] Transform behindWallCheckStartpoint;
    [SerializeField] Transform jumpCheckStartpoint;
    [SerializeField] float distanceSafe;

    private float nextTimeCanjump = 0f;

    #region States
    public ArcherIdleState idleState { get; private set; }
    public ArcherMoveState moveState { get; private set; }
    public ArcherBattleState battleState { get; private set; }
    public ArcherAttackState attackState { get; private set; }
    public ArcherStunnedState stunnedState { get; private set; }
    public ArcherDeadState deadState { get; private set; }
    public ArcherJumpState jumpState { get; private set; }
    #endregion


    protected override void Reset()
    {
        base.Reset();
        jumpVelocity = new Vector2(15f, 15f);
        jumpCooldown = 1.5f;
        distanceSafe = 2.56f;
    }
    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(stunnedState);
            return true;
        }
        return false;
    }

    public override void DamageImpact()
    {
        base.DamageImpact();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        idleState = new ArcherIdleState(this, stateMachine, "Idle", this);
        moveState = new ArcherMoveState(this, stateMachine, "Run", this);
        battleState = new ArcherBattleState(this, stateMachine, "Run", this);
        attackState = new ArcherAttackState(this, stateMachine, "Attack", this);
        stunnedState = new ArcherStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ArcherDeadState(this, stateMachine, "Dead", this);
        jumpState = new ArcherJumpState(this, stateMachine, "Jump", this);
        stateMachine.Initialize(idleState);
    }

    public bool UpdateCheckToJumpState()
    {
        if (CanJump() && IsPlayerNear() && IsGroundBehindDetected() && !IsBehindFaceWallDetected())
        {
            stateMachine.ChangeState(jumpState);
            return true;
        }
        return false;
    }
    public void SpawnArrow()
    {
        GameObject newArrow = Instantiate(arrowPrefab, attackCheck.position, transform.rotation);
        newArrow.GetComponent<ArrowController>().Direction = facingDir;
    }
    public bool CanJump() => Time.time > nextTimeCanjump;
    public void UpdateTimeNextJump() => nextTimeCanjump = Time.time + jumpCooldown;
    public RaycastHit2D IsPlayerNear() => Physics2D.Raycast(jumpCheckStartpoint.position, Vector2.right * facingDir, distanceSafe, playerMask);
    public bool IsGroundBehindDetected() => Physics2D.Raycast(behindGroundCheckStartpoint.position, Vector2.down, distanceGroundCheck, groundMask);
    public bool IsBehindFaceWallDetected() => Physics2D.Raycast(behindWallCheckStartpoint.position, Vector2.left * facingDir, distanceWallCheck, groundMask);

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        //if (Input.GetKeyDown(KeyCode.U))
        //{
        //    stateMachine.ChangeState(stunnedState);
        //}
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Gizmos.color = Color.white;
        Gizmos.DrawLine(jumpCheckStartpoint.position, new Vector2(jumpCheckStartpoint.position.x + facingDir * distanceSafe, jumpCheckStartpoint.position.y));

        Gizmos.color = Color.green;
        Gizmos.DrawLine(behindGroundCheckStartpoint.position, new Vector2(behindGroundCheckStartpoint.position.x, behindGroundCheckStartpoint.position.y - distanceGroundCheck));

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(behindWallCheckStartpoint.position, new Vector2(behindWallCheckStartpoint.position.x + -facingDir * distanceWallCheck, behindWallCheckStartpoint.position.y));
    }
}
