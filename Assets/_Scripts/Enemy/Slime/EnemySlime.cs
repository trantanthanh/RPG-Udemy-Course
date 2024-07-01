using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlimeType
{
    Big,
    Medium,
    Small
}

public class EnemySlime : Enemy
{
    [Header("Slime specific")]
    [SerializeField] SlimeType slimeType;
    [SerializeField] int slimesToCreate;
    [SerializeField] GameObject slimePrefab;
    [SerializeField] Vector2 minCreateVelocity;
    [SerializeField] Vector2 maxCreateVelocity;

    #region States
    public SlimeIdleState idleState { get; private set; }
    public SlimeMoveState moveState { get; private set; }
    public SlimeBattleState battleState { get; private set; }
    public SlimeAttackState attackState { get; private set; }
    public SlimeStunnedState stunnedState { get; private set; }
    public SlimeDeadState deadState { get; private set; }
    #endregion

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
        idleState = new SlimeIdleState(this, stateMachine, "Idle", this);
        moveState = new SlimeMoveState(this, stateMachine, "Move", this);
        battleState = new SlimeBattleState(this, stateMachine, "Move", this);
        attackState = new SlimeAttackState(this, stateMachine, "Attack", this);
        stunnedState = new SlimeStunnedState(this, stateMachine, "Stunned", this);
        deadState = new SlimeDeadState(this, stateMachine, "Dead", this);
        stateMachine.Initialize(idleState);
    }

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

    public void CheckSpawnSlime()
    {
        if (slimeType == SlimeType.Small || slimePrefab == null)
        {
            return;
        }

        CreateSlimes(slimesToCreate, slimePrefab);
    }

    public void SetupSlime(int _facingDir)
    {
        if (facingDir != _facingDir)//if doesn't face with player, flip to face
        {
            Flip();
        }
        Vector2 randomVecolity = new Vector2(Random.Range(minCreateVelocity.x, maxCreateVelocity.x), Random.Range(minCreateVelocity.y, maxCreateVelocity.y));
        isKnocked = true;
        GetComponent<Rigidbody2D>().velocity = randomVecolity;
        Invoke(nameof(CancelKnockedBack), 1.5f);
    }

    private void CancelKnockedBack() => isKnocked = false;

    public void CreateSlimes(int _amountOfSlimes, GameObject _slimePrefab)
    {
        for (int i = 0; i < _amountOfSlimes; i++)
        {
            GameObject newSlime = Instantiate(_slimePrefab, transform.position, Quaternion.identity);

            newSlime.GetComponent<EnemySlime>().SetupSlime(facingDir);
        }
    }
}
