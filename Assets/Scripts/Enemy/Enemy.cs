using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IAnimationDoneTrigger
{
    [SerializeField] public float idleTime = 1.0f;
    [SerializeField] LayerMask playerMask;
    [SerializeField] protected GameObject playerCheckStartPoint;
    [SerializeField] protected float distancePlayerCheck;
    [SerializeField] protected float distanceAttack;
    [SerializeField] protected float attackCoolDown = 0.4f;
    protected float lastTimeAttack = 0.0f;
    public float LastTimeAttack { get { return lastTimeAttack; } set { lastTimeAttack = value; } }
    public float AttackCoolDown { get { return attackCoolDown; } }

    public float DistanceAttack { get { return distanceAttack; } }
    public float DistancePlayerCheck { get { return distanceAttack; } }
    public EnemyStateMachine stateMachine { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        stateMachine.currentState.Update();
    }

    public void AnimationDoneTrigger() => stateMachine.currentState.AnimationDoneTrigger();
    public bool CanAttack() => Time.time > lastTimeAttack + attackCoolDown;
    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(playerCheckStartPoint.transform.position, Vector2.right * facingDir, distancePlayerCheck, playerMask);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheckStartPoint.transform.position, new Vector2(playerCheckStartPoint.transform.position.x + facingDir * distancePlayerCheck, playerCheckStartPoint.transform.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheckStartPoint.transform.position, new Vector2(playerCheckStartPoint.transform.position.x + facingDir * distanceAttack, playerCheckStartPoint.transform.position.y));
    }
}
