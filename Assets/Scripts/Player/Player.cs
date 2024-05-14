using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : Entity
{
    [Header("Dash info")]
    [SerializeField] float dashSpeed = 16f;
    [SerializeField] float timeDash = 0.5f;

    [Header("Wall slide jump info")]
    [SerializeField] float yVelocitySlideMulti = 0.7f;
    [Tooltip("Can control after period time wall jump")]
    [SerializeField] float timeWallJump = 0.4f;
    [SerializeField] float xJumpForceWall = 5f;

    [Header("Counter attack info")]
    [SerializeField] float counterAttackDuration = 0.2f;
    [SerializeField] private LayerMask enemyMask;
    #region Property
    public float CounterAttackDuration
    {
        get
        {
            return counterAttackDuration;
        }
    }
    public float X_JumpForceWall
    {
        get
        {
            return xJumpForceWall;
        }
    }

    public float TimeWallJump
    {
        get
        {
            return timeWallJump;
        }
    }
    public float VelocitySlideMulti
    {
        get
        {
            return yVelocitySlideMulti;
        }
    }

    public float DashSpeed
    {
        get
        {
            return dashSpeed;
        }
    }
    public float TimeDash
    {
        get
        {
            return timeDash;
        }
    }
    #endregion

    #region Components
    public SkillManager skills { get; private set; }
    #endregion
    public GameObject sword { get; private set; }

    #region States
    public PlayerSateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState primaryAttackState { get; private set; }
    public PlayerCounterAttackState counterAttackState { get; private set; }
    public PlayerCatchSwordState catchSwordState { get; private set; }
    public PlayerAimSwordState aimSwordState { get; private set; }
    public PlayerBlackHoleState blackHoleState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerSateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
        catchSwordState = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        aimSwordState = new PlayerAimSwordState(this, stateMachine, "AimSword");
        blackHoleState = new PlayerBlackHoleState(this, stateMachine, "Jump");

        skills = SkillManager.Instance;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
        //Debug.Log($"IsGrounded {IsGroundDetected()}");
        //Debug.Log($"IsFaceWall {IsFaceWallDetected()}");
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (skills.crystal.CanUseSkill())
            {
                skills.crystal.UseSkill();
            }
        }

        CheckDash();
    }

    private void CheckDash()
    {
        if (IsFaceWallDetected()) return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.dash.CanUseSkill())
        {
            skills.dash.UseSkill();
            stateMachine.ChangeState(dashState);
        }
    }

    public void Jump()
    {
        SetVelocity(rb.velocity.x, jumpForce);
    }

    public void AnimationDoneTrigger() => stateMachine.currentState.AnimationDoneTrigger();

    public override void Damage()
    {
        base.Damage();

        Debug.Log("Player has damaged");
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSwordState);
    }
    public void DestroySword()
    {
        Destroy(sword);
    }

    #region Utilities
    public Transform FindClosestEnemy(Vector3 _position, float _radius)
    {
        Transform _closestEnemy = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_position, _radius, enemyMask);
        float _closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            Enemy _enemy = collider.GetComponent<Enemy>();
            if (_enemy != null)
            {
                float _distance = Vector2.Distance(_position, _enemy.transform.position);
                if (_closestDistance > _distance)
                {
                    _closestDistance = _distance;
                    _closestEnemy = _enemy.transform;
                }

            }
        }
        return _closestEnemy;
    }

    public Transform FindRandomEnemy(Vector3 _position, float _radius)
    {
        Transform _randomEnemy = null;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_position, _radius, enemyMask);

        if (colliders.Length > 0)
        {
            _randomEnemy = colliders[Random.Range(0, colliders.Length)].transform;
        }

        return _randomEnemy;
    }

    public Transform DoDamageEnemiesInCircle(Vector3 _position, float _radius)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_position, _radius);
        Transform hasEnemy = null;

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage();
                stats.DoDamage(enemy.stats);
                //enemy.GetComponent<CharacterStats>().TakeDamage(stats.damage.GetValue());
                hasEnemy = enemy.transform;
            }
        }
        return hasEnemy;
    }
    #endregion
}
