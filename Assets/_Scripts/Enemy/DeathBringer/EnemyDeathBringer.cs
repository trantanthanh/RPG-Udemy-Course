using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBringer : Enemy
{
    [Header("Teleport detail")]
    [SerializeField] BoxCollider2D arena;
    [SerializeField] Vector2 surroundingCheckSize;

    Vector3 lastPosition = Vector3.zero;
    private int numOfRecursive = 0;

    #region States
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerMoveState moveState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        idleState = new DeathBringerIdleState(this, stateMachine, "Idle", this);
        moveState = new DeathBringerMoveState(this, stateMachine, "Move", this);
        battleState = new DeathBringerBattleState(this, stateMachine, "Move", this);
        attackState = new DeathBringerAttackState(this, stateMachine, "Attack", this);
        deadState = new DeathBringerDeadState(this, stateMachine, "Dead", this);
        teleportState = new DeathBringerTeleportState(this, stateMachine, "Teleport", this);
        stateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void StartFindNewPos()
    {
        numOfRecursive = 0;
        lastPosition = transform.position;
        FindPosition();
    }

    private void FindPosition()
    {
        numOfRecursive++;
        if (numOfRecursive > 3)
        {
            transform.position = lastPosition;
            return;
        }
        float x = Random.Range(arena.bounds.min.x + 3, arena.bounds.max.x - 3);
        float y = Random.Range(arena.bounds.min.y + 3, arena.bounds.max.y - 3);
        transform.position = new Vector2(x, y);
        transform.position = new Vector2(transform.position.x, transform.position.y - GroundBelow().distance + capsuleCollider.size.y / 2);

        if (!GroundBelow() || SomethingIsAround())
        {
            Debug.Log("Looking for new position");
            FindPosition();
        }
    }

    private RaycastHit2D GroundBelow() => Physics2D.Raycast(transform.position, Vector2.down, 100, groundMask);

    public bool SomethingIsAround() => Physics2D.BoxCast(transform.position, surroundingCheckSize, 0, Vector2.zero, 0, groundMask);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - GroundBelow().distance));
        Gizmos.DrawWireCube(transform.position, surroundingCheckSize);
    }
}
