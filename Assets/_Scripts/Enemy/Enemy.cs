using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(EntityFx))]
[RequireComponent(typeof(EnemyStats))]
[RequireComponent(typeof(ItemDrop))]
public class Enemy : Entity, IAnimationDoneTrigger
{
    [Header("Stunned info")]
    public float stunDuration = 1.0f;
    public Vector2 stunDirection;
    [SerializeField] GameObject counterImage;
    protected bool canBeStunned = false;

    [Header("Collision info")]
    [SerializeField] public float idleTime = 1.0f;
    [SerializeField] protected LayerMask playerMask;
    [SerializeField] protected GameObject playerCheckStartPoint;
    [SerializeField] protected float distancePlayerCheck;
    [SerializeField] protected float distanceAttack;
    [SerializeField] protected float minAttackCoolDown = 0.4f;
    [SerializeField] protected float maxAttackCoolDown = 0.8f;
    [SerializeField] protected float battleTime = 1f;

    [Header("Dead info")]
    [SerializeField] protected float deadTimeFlyUp = 0.1f;
    [SerializeField] protected Vector2 deadVelocityFallDown = new Vector2(0, 10);
    public float DeadTimeFlyUp => deadTimeFlyUp;
    public Vector2 DeadVelocityFallDown => deadVelocityFallDown;

    private float attackCooldown = 0f;
    protected float lastTimeAttack = 0.0f;
    public float BattleTime => battleTime;

    public float DistanceAttack => distanceAttack;
    public float DistancePlayerCheck => distanceAttack;

    public string lastAnimBoolName { get; private set; }

    public EnemyStateMachine stateMachine { get; private set; }

    public EntityFx fx { get; private set; }

    protected override void Reset()
    {
        base.Reset();
        stunDuration = 1.0f;
        stunDirection = new Vector2(8, 12);
        idleTime = 1.0f;

        distancePlayerCheck = 3f;//11f for acher
        distanceAttack = 1.5f;//11f for archer
        minAttackCoolDown = 0.4f;
        maxAttackCoolDown = 0.8f;
        battleTime = 3f;

        deadTimeFlyUp = 0.1f;
        deadVelocityFallDown = new Vector2(0, 10);
    }

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        fx = GetComponent<EntityFx>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        stateMachine.currentState.Update();
    }

    public virtual void OpenCounterAttack()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttack()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttack();
            return true;
        }
        return false;
    }

    public virtual void AssignLastAnimName(string _lastAnimBoolName) => lastAnimBoolName = _lastAnimBoolName;

    public override void Die()
    {
        base.Die();
        counterImage.SetActive(false);
        CheckReturnSword();
    }

    private void CheckReturnSword()
    {
        SwordSkillController swordSkillController = GetComponentInChildren<SwordSkillController>();
        if (swordSkillController != null)
        {
            swordSkillController.ReturnSword();
        }
    }

    public virtual void FreezeTimer(bool isFrozen)
    {
        if (isFrozen)
        {
            currentMoveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            ReturnDefaultSpeed();
        }
    }

    public void FreezeTimerFor(float _duration) => StartCoroutine(FreezeTimerForCoroutine(_duration));

    protected virtual IEnumerator FreezeTimerForCoroutine(float _seconds)
    {
        FreezeTimer(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTimer(false);
    }

    public void UpdateNextAttack()
    {
        lastTimeAttack = Time.time;
        attackCooldown = Random.Range(minAttackCoolDown, maxAttackCoolDown);
    }

    public void AnimationDoneTrigger() => stateMachine.currentState.AnimationDoneTrigger();
    public bool CanAttack() => Time.time > lastTimeAttack + attackCooldown;
    public RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(playerCheckStartPoint.transform.position, Vector2.right * facingDir, distancePlayerCheck, playerMask);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(playerCheckStartPoint.transform.position, new Vector2(playerCheckStartPoint.transform.position.x + facingDir * distancePlayerCheck, playerCheckStartPoint.transform.position.y));
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheckStartPoint.transform.position, new Vector2(playerCheckStartPoint.transform.position.x + facingDir * distanceAttack, playerCheckStartPoint.transform.position.y));
    }

    #region Utilities
    public void DoDamagePlayerInCircle(Vector3 _position, float _radius)
    {
        Collider2D[] collders = Physics2D.OverlapCircleAll(_position, _radius);
        foreach (Collider2D collider in collders)
        {
            Player player = collider.GetComponent<Player>();
            if (player != null && player.CanDamage())
            {
                //player.Damage();
                stats.DoDamage(player.stats);
                //player.stats.TakeDamage(stats.damage.GetValue());
            }
        }
    }

    public override void DamageEffect()
    {
        base.DamageEffect();
        fx.StartCoroutine(fx.FlashFx());
    }

    public void SetupForDead()
    {
        animator.SetBool(lastAnimBoolName, true);
        animator.speed = 0;
        capsuleCollider.enabled = false;
    }

    #endregion
}
