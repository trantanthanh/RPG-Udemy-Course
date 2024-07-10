using UnityEngine;

public class EnemyShady : Enemy
{
    [Header("Shady specific config")]
    [SerializeField] GameObject explosionFxPrefab;
    [SerializeField] float shadyDistanceExplode = 0.5f;
    [SerializeField] float growSpeed = 16f;
    [SerializeField] float growMaxSize = 5f;

    public float ShadyDistanceExplode => shadyDistanceExplode;

    #region States
    public ShadyIdleState idleState { get; private set; }
    public ShadyMoveState moveState { get; private set; }
    public ShadyBattleState battleState { get; private set; }
    public ShadyAttackState attackState { get; private set; }
    public ShadyStunnedState stunnedState { get; private set; }
    public ShadyDeadState deadState { get; private set; }
    #endregion

    protected override void Reset()
    {
        base.Reset();
        growSpeed = 16f;
        growMaxSize = 5f;
        shadyDistanceExplode = 0.2f;
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

    protected override void Start()
    {
        base.Start();
        idleState = new ShadyIdleState(this, stateMachine, "Idle", this);
        moveState = new ShadyMoveState(this, stateMachine, "Move", this);
        battleState = new ShadyBattleState(this, stateMachine, "MoveFast", this);
        attackState = new ShadyAttackState(this, stateMachine, "Explosion", this);
        stunnedState = new ShadyStunnedState(this, stateMachine, "Stunned", this);
        deadState = new ShadyDeadState(this, stateMachine, "Dead", this);
        stateMachine.Initialize(idleState);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void SpecificAttackTrigger()
    {
        GameObject newExplosion = Instantiate(explosionFxPrefab, transform.position, Quaternion.identity);
        newExplosion.GetComponent<ExplosionController>().SetupExplosion(this, growSpeed, growMaxSize);
    }
}
