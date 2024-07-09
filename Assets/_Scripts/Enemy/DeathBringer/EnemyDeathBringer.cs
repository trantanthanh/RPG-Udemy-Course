using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathBringer : Enemy
{
    [Header("Teleport detail")]
    [SerializeField] BoxCollider2D arena;
    [SerializeField] Vector2 surroundingCheckSize;
    [Range(0,100)]
    [SerializeField] float defaultChanceToTeleport = 25f;
    [Range(0, 100)]
    [SerializeField] float percentChanceTeleportIncrease = 20f;
    [SerializeField] float timeCannotAttackLong = 8f;
    [HideInInspector]
    public float chanceToTeleport;

    [Header("Spell cast info")]
    [SerializeField] GameObject spellPrefab;
    [SerializeField] int amountOfSpell = 3;
    [SerializeField] float spellCastCooldown = 10f;
    [SerializeField] float spellCastInterval = 0.9f;
    [SerializeField] float timeCastSpellDuration = 5f;
    float lastDoSpellCast;

    public float TimeCannotAttackLong => timeCastSpellDuration;
    public float TimeCastSpellDuration => timeCastSpellDuration;
    public int AmountOfSpell => amountOfSpell;
    public float SpellCastInterval => spellCastInterval;

    Vector3 lastPosition = Vector3.zero;
    private int numOfRecursive = 0;

    #region States
    public DeathBringerIdleState idleState { get; private set; }
    public DeathBringerMoveState moveState { get; private set; }
    public DeathBringerBattleState battleState { get; private set; }
    public DeathBringerAttackState attackState { get; private set; }
    public DeathBringerDeadState deadState { get; private set; }
    public DeathBringerTeleportState teleportState { get; private set; }
    public DeathBringerSpellCastState spellCastState { get; private set; }
    #endregion

    protected override void Reset()
    {
        base.Reset();
        timeCannotAttackLong = 8f;
        defaultChanceToTeleport = 25;
        percentChanceTeleportIncrease = 20;

        amountOfSpell = 3;
        spellCastCooldown = 10f;
        spellCastInterval = 0.9f;
        timeCastSpellDuration = 5f;
    }

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
        spellCastState = new DeathBringerSpellCastState(this, stateMachine, "SpellCast", this);
        stateMachine.Initialize(idleState);
    }

    public override void DamageImpact()
    {
        base.DamageImpact();
        if (stateMachine.IsState(moveState))
        {
            Debug.Log("check change to battle");
            battleState.InitStateTimer();
            stateMachine.ChangeState(battleState);
        }
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public void CastSpell()
    {
        GameObject newSpell = Instantiate(spellPrefab, PlayerManager.Instance.player.transform.position, Quaternion.identity);
        newSpell.GetComponentInChildren<DeathBringerSpellController>().SetupSpell(this);
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
            //can't find new position
            Appear();
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
        else
        {
            //found new position
            Appear();
        }
    }

    private void Appear()
    {
        fx.MakeTransparent(false);
        teleportState.Appear();
    }

    public bool CanTeleport()
    {
        if (Random.Range(0, 100) <= chanceToTeleport)
        {
            chanceToTeleport = defaultChanceToTeleport;
            return true;
        }
        chanceToTeleport += percentChanceTeleportIncrease;
        return false;
    }

    public bool CanDoSpellCast()
    {
        if (Time.time > lastDoSpellCast + spellCastCooldown)
        {
            lastDoSpellCast = Time.time;
            return true;
        }
        return false;
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
