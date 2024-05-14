using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity, IAnimationDoneTrigger
{
    [Header("Stunned info")]
    public float stunDuration = 1.0f;
    public Vector2 stunDirection;
    [SerializeField] GameObject counterImage;
    protected bool canBeStunned = false;

    [Header("Collision info")]
    [SerializeField] public float idleTime = 1.0f;
    [SerializeField] LayerMask playerMask;
    [SerializeField] protected GameObject playerCheckStartPoint;
    [SerializeField] protected float distancePlayerCheck;
    [SerializeField] protected float distanceAttack;
    [SerializeField] protected float attackCoolDown = 0.4f;
    [SerializeField] protected float battleTime = 1f;
    protected float lastTimeAttack = 0.0f;
    public float LastTimeAttack { get { return lastTimeAttack; } set { lastTimeAttack = value; } }
    public float AttackCoolDown { get { return attackCoolDown; } }
    public float BattleTime { get { return battleTime; } }

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

    public virtual void FreezeTimer(bool isFrozen)
    {
        if (isFrozen)
        {
            moveSpeed = 0;
            animator.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            animator.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTimer(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTimer(false);
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

    #region Utilities
    public void DoDamagePlayerInCircle(Vector3 _position, float _radius)
    {
        Collider2D[] collders = Physics2D.OverlapCircleAll(_position, _radius);
        foreach (Collider2D collider in collders)
        {
            Player player = collider.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
                player.stats.TakeDamage(stats.damage);
            }
        }
    }
    #endregion
}
