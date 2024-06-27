using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : Entity
{
    [Header("Dash info")]
    [SerializeField] float dashSpeed = 16f;
    [SerializeField] float timeDash = 0.5f;
    private float defaultDashSpeed = 0f;

    [Header("Wall slide jump info")]
    [SerializeField] float yVelocitySlideMulti = 0.7f;
    [Tooltip("Can control after period time wall jump")]
    [SerializeField] float timeWallJump = 0.4f;
    [SerializeField] float xJumpForceWall = 5f;

    [Header("Counter attack info")]
    [SerializeField] float counterAttackDuration = 0.2f;
    [SerializeField] private LayerMask enemyMask;

    [Header("Sword return")]
    [SerializeField] float swordReturnImpact = 5f;

    #region Property
    public float SwordReturnImpact => swordReturnImpact;
    public float CounterAttackDuration => counterAttackDuration;
    public float X_JumpForceWall => xJumpForceWall;

    public float TimeWallJump => timeWallJump;
    public float VelocitySlideMulti => yVelocitySlideMulti;

    public float DashSpeed => dashSpeed;
    public float TimeDash => timeDash;
    #endregion

    #region Components
    public SkillManager skills { get; private set; }
    #endregion
    public GameObject sword { get; private set; }

    #region States
    public PlayerStateMachine stateMachine { get; private set; }
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
    public PlayerDeadState deadState { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
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
        deadState = new PlayerDeadState(this, stateMachine, "Die");

        skills = SkillManager.Instance;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);

        defaultDashSpeed = dashSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if (GameManager.Instance.isPaused) return;
        stateMachine.currentState.Update();
        //Debug.Log($"IsGrounded {IsGroundDetected()}");
        //Debug.Log($"IsFaceWall {IsFaceWallDetected()}");
        if (skills.crystal.crystalUnlocked && Input.GetKeyDown(KeyCode.F))
        {
            if (skills.crystal.CanUseSkill())
            {
                skills.crystal.UseSkill();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            InventoryManager.Instance.UseFlask();
        }

        if (skills.dash.dashUnlocked)
        {
            CheckDash();
        }
    }

    private void CheckDash()
    {
        if (IsFaceWallDetected()) return;
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.Instance.dash.CanUseSkill())
        {
            PlayerManager.Instance.uiIngame.SetDashCooldown();
            skills.dash.UseSkill();
            stateMachine.ChangeState(dashState);
        }
    }

    public bool CanDamage()
    {
        return (!stateMachine.IsState(blackHoleState) && !stateMachine.IsState(dashState));
    }

    public void Jump()
    {
        SetVelocity(rb.velocity.x, jumpForce);
    }

    public void AnimationDoneTrigger() => stateMachine.currentState.AnimationDoneTrigger();

    public override void DamageImpact()
    {
        base.DamageImpact();
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
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    #region Utilities
    public Transform FindClosestEnemy(Vector3 _position, float _radius, float _minRange = -1f)
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
                if (_minRange > 0 && _distance < _minRange)
                {
                    continue;
                }
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

    public Transform DoDamageEnemiesInCircle(Vector3 _position, float _radius, bool _isMagicalDamage = false, float _multiplierDamage = 1f, Transform _damageDirection = null)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_position, _radius);
        Transform hasEnemy = null;

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                //enemy.Damage();
                if (_isMagicalDamage)
                {
                    stats.DoMagicDamage(enemy.stats);
                    DoEffectFromAmulet(enemy);
                }
                else
                {
                    stats.DoDamage(enemy.stats, _multiplierDamage);
                }
                enemy.GetComponent<Entity>().SetupKnockbackDir(_damageDirection == null ? transform : _damageDirection);
                //enemy.GetComponent<CharacterStats>().TakeDamage(stats.damage.GetValue());
                hasEnemy = enemy.transform;

                //inventory get weapon call item effect
                InventoryManager.Instance.GetEquipment(EquipmentType.Weapon)?.Effect(enemy.transform);
            }
        }
        return hasEnemy;
    }

    public void DoEffectFromAmulet(Enemy enemy)
    {
        ItemData_Equipment_SO amuletEquipped = InventoryManager.Instance.GetEquipment(EquipmentType.Amulet);
        if (amuletEquipped != null)
        {
            amuletEquipped.Effect(enemy.transform);
        }
    }

    public override void SlowEntityBy(float _slowPercentage, float _duration)
    {
        base.SlowEntityBy(_slowPercentage, _duration);
        dashSpeed *= (1 - _slowPercentage);
    }

    public override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        dashSpeed = defaultDashSpeed;
    }
    #endregion
}
