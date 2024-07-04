using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

public enum StatType
{
    strength,
    agility,
    inteligence,
    vitality,

    damage,
    critChance,
    critPower,

    fireDamage,
    iceDamage,
    lightningDamage,

    maxHealth,
    armor,
    evasion,
    magicResistance
}

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength;//1 point increase damage by 1 and critical power by 1%
    public Stat agility;//1 point increase evasion by 1% and crit chance by 1%
    public Stat inteligence;//1 point increase magic damage by 1 and magic resistance by 3
    public Stat vitality;//1 point increase health by 3 or 5 points

    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightningDamage;

    public bool isIgnited;//do damage overtime
    public bool isChilled;//reduce armor by 20%, speed by 50%
    public bool isShocked;//reduce accuracy by 20%

    private bool isVulnerability = false;
    private float vulnerabilityDamageMultiplier = 1.0f;

    private float igniteTimer;
    private float igniteDuration = 4f;
    private float igniteDamageCoolDown = 0.3f;//interval take burn damage
    private float igniteDamageTimer;
    private int igniteDamage;

    private float chillTimer;
    private float chillDuration = 2f;

    private float shockTimer;
    private float shockDuration = 2f;

    [SerializeField] private GameObject shockStrikePrefab;
    [SerializeField] private int shockDamage;


    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    [HideInInspector]
    public Stat critPower;//default value is 150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat evasion;
    public Stat armor;
    public Stat magicResistance;

    #region Components
    private Entity entity;
    private EntityFx fx;
    #endregion

    protected int currentHealth;
    public int CurrentHealth { get => currentHealth; }
    public System.Action onHealthChanged;

    protected bool isAlive = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        //currentHealth = maxHealth.GetValue();
        currentHealth = GetMaxHealth();//calc with other stats

        fx = GetComponent<EntityFx>();
        entity = GetComponent<Entity>();
        critPower.SetDefaultValue(150);
        isAlive = true;
    }

    protected virtual void Update()
    {
        igniteTimer -= Time.deltaTime;
        chillTimer -= Time.deltaTime;
        shockTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if (isIgnited && igniteTimer < 0)
        {
            isIgnited = false;
        }

        if (isChilled && chillTimer < 0)
        {
            isChilled = false;
        }

        if (isShocked && shockTimer < 0)
        {
            isShocked = false;
        }

        if (isIgnited && igniteDamageTimer < 0)
        {
            //Take burn damage
            igniteDamageTimer = igniteDamageCoolDown;
            TakeDamageWithoutEffect(igniteDamage);
        }
    }

    public void VulnerabilityFor(float _seconds, float _vulnerabilityDamageMultiplier = 1.0f)
    {
        vulnerabilityDamageMultiplier = _vulnerabilityDamageMultiplier;
        StartCoroutine(VulnerabilityForCoroutine(_seconds));
    }

    IEnumerator VulnerabilityForCoroutine(float _seconds)
    {
        isVulnerability = true;
        yield return _seconds.WaitForSeconds();
        isVulnerability = false;
    }

    public virtual void DoDamage(CharacterStats _targetStats, float multiplierDamage = 1)
    {
        bool isCrit = false;
        if (CheckTargetCanAvoidAttack(_targetStats)) return;

        //int totalDamage = damage.GetValue() + strength.GetValue();
        int totalDamage = GetFinalValueStat(StatType.damage);
        if (isVulnerability)
        {
            totalDamage = Mathf.RoundToInt(totalDamage * vulnerabilityDamageMultiplier);
        }
        else
        {
            totalDamage = Mathf.RoundToInt(totalDamage * multiplierDamage);
        }
        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
            isCrit = true;
        }
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        if (totalDamage > 0)
        {

            _targetStats.TakeDamage(totalDamage);//Apply damage to target
            if (_targetStats.isAlive)
            {
                fx.CreateHitFx(_targetStats.transform, isCrit);
            }
        }
        else
        {
            _targetStats.fx.CreatePopupText("Missed");
            //evaded or damage is reduce to 0
        }

        DoMagicDamage(_targetStats);
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        if (!isAlive) return;
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + inteligence.GetValue();
        totalMagicDamage = CheckTargetMagicResist(_targetStats, totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

        _targetStats.TakeDamageWithoutEffect(totalMagicDamage);//Apply magic damage to target

        AttempToApplyAilment(_targetStats, _fireDamage, _iceDamage, _lightningDamage);
    }

    private int CheckTargetMagicResist(CharacterStats targetStats, int totalMagicDamage)
    {
        //totalMagicDamage -= targetStats.magicResistance.GetValue() + targetStats.inteligence.GetValue() * 3;
        totalMagicDamage -= targetStats.GetFinalValueStat(StatType.magicResistance);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    private void AttempToApplyAilment(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightningDamage)
    {
        bool _canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightningDamage;
        bool _canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightningDamage;
        bool _canApplyShock = _lightningDamage > _fireDamage && _lightningDamage > _iceDamage;

        while (!_canApplyIgnite && !_canApplyChill && !_canApplyShock)
        {
            if (Random.value < 0.5f && _fireDamage > 0)
            {
                _canApplyIgnite = true;
                break;
            }

            if (Random.value < 0.5f && _iceDamage > 0)
            {
                _canApplyChill = true;
                break;
            }

            //if (Random.value < 0.5f && _lightningDamage > 0)
            //{
            //    _canApplyShock = true;
            //    break;
            //}

            if (_lightningDamage > 0)//optimized : if both !_canApplyIgnite && !_canApplyChill then _canApplyShock = true
            {
                _canApplyShock = true;
                break;
            }
        }

        if (_canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }

        _targetStats.ApplyAilment(_canApplyIgnite, _canApplyChill, _canApplyShock, shockDamage);
    }

    public void ApplyAilment(bool _ignite, bool _chill, bool _shock, int _shockDamage)
    {
        bool _canApplyIgnite, _canApplyChill, _canApplyShock;
        _canApplyIgnite = _canApplyChill = !isIgnited && !isChilled && !isShocked;
        _canApplyShock = !isIgnited && !isChilled;

        if (_ignite && _canApplyIgnite)
        {
            isIgnited = _ignite;
            igniteTimer = igniteDuration;
            fx.IgniteFxFor(igniteDuration, igniteDamageCoolDown);
        }

        if (_chill && _canApplyChill)
        {
            isChilled = _chill;
            chillTimer = chillDuration;
            fx.ChillFxFor(chillDuration);
            entity.SlowEntityBy(0.5f, chillDuration);
        }

        if (_shock && _canApplyShock)
        {
            if (!isShocked)
            {
                isShocked = _shock;
                shockTimer = shockDuration;
                fx.ShockFxFor(shockDuration);
            }
            else
            {
                if (GetComponent<Player>() != null) return;//not apply to player
                if (_shockDamage > 0)
                {
                    //instantiate thunderstrike to nearest enemy target
                    Transform closestEnemy = PlayerManager.Instance.player.FindClosestEnemy(transform.position, 25, 1f);
                    if (closestEnemy == null)
                    {
                        closestEnemy = transform;//no enemy nearest, shock this target
                    }
                    if (closestEnemy != null)
                    {
                        GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
                        newShockStrike.GetComponent<ShockStrikeController>().Setup(_shockDamage, closestEnemy.GetComponent<CharacterStats>());
                    }
                }
            }
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if (_targetStats.isChilled)
        {
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);//reduce 20% armor
        }
        else
        {
            totalDamage -= _targetStats.armor.GetValue();
        }
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CheckTargetCanAvoidAttack(CharacterStats _targetStats)
    {
        //int totalTargetEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        int totalTargetEvasion = _targetStats.GetFinalValueStat(StatType.evasion);
        if (isShocked)
        {
            totalTargetEvasion += 20;
        }
        if (Random.Range(0, 100) <= totalTargetEvasion)
        {
            _targetStats.OnDodge();
            return true;
        }
        return false;
    }

    protected virtual void OnDodge()
    {

    }

    private bool CanCrit()
    {
        //int totalCritChance = critChance.GetValue() + agility.GetValue();
        int totalCritChance = GetFinalValueStat(StatType.critChance);

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCritDamage(int _damage)
    {
        //float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float totalCritPower = GetFinalValueStat(StatType.critPower) * 0.01f;
        float totalDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(totalDamage);
    }

    public bool IsAlive()
    {
        return isAlive;
    }

    public virtual void TakeDamage(int damage)
    {
        TakeDamageWithoutEffect(damage);
    }

    protected virtual void TakeDamageWithoutEffect(int damage)
    {
        if (!isAlive) return;
        fx.CreatePopupText(damage.ToString());
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
        onHealthChanged?.Invoke();
    }

    public void RestoreHealthBy(int _amount)
    {
        if (!isAlive) return;
        currentHealth += _amount;
        if (currentHealth >= GetMaxHealth())
        {
            currentHealth = GetMaxHealth();
        }
        onHealthChanged?.Invoke();
    }

    public int GetMaxHealth() => (maxHealth.GetValue() + vitality.GetValue() * 5);

    protected virtual void Die()
    {
        isAlive = false;
    }

    public virtual void IncreaseStatBy(int _modifier, float _duration, Stat _statToModify)
    {
        //Start Coroutine for stat increase
        if (_statToModify != null)
        {
            StartCoroutine(StatModIncrease(_modifier, _duration, _statToModify));
        }
    }

    IEnumerator StatModIncrease(int _modifier, float _duration, Stat _statToModify)
    {
        _statToModify.AddModifier(_modifier);
        yield return _duration.WaitForSeconds();
        _statToModify.RemoveModifier(_modifier);
    }

    public Stat GetBaseStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                {
                    return strength;
                }
            case StatType.inteligence:
                {
                    return inteligence;
                }
            case StatType.agility:
                {
                    return agility;
                }
            case StatType.vitality:
                {
                    return vitality;
                }
            case StatType.damage:
                {
                    return damage;
                }
            case StatType.critChance:
                {
                    return critChance;
                }
            case StatType.critPower:
                {
                    return critPower;
                }
            case StatType.fireDamage:
                {
                    return fireDamage;
                }
            case StatType.iceDamage:
                {
                    return iceDamage;
                }
            case StatType.lightningDamage:
                {
                    return lightningDamage;
                }
            case StatType.maxHealth:
                {
                    return maxHealth;
                }
            case StatType.armor:
                {
                    return armor;
                }
            case StatType.evasion:
                {
                    return evasion;
                }
            case StatType.magicResistance:
                {
                    return magicResistance;
                }
        }
        return null;
    }

    public int GetFinalValueStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                {
                    return strength.GetValue();
                }
            case StatType.inteligence:
                {
                    return inteligence.GetValue();
                }
            case StatType.agility:
                {
                    return agility.GetValue();
                }
            case StatType.vitality:
                {
                    return vitality.GetValue();
                }
            case StatType.damage:
                {
                    return damage.GetValue() + strength.GetValue();
                }
            case StatType.critChance:
                {
                    return critChance.GetValue() + agility.GetValue();
                }
            case StatType.critPower:
                {
                    return critPower.GetValue() + strength.GetValue();
                }
            case StatType.fireDamage:
                {
                    return fireDamage.GetValue();
                }
            case StatType.iceDamage:
                {
                    return iceDamage.GetValue();
                }
            case StatType.lightningDamage:
                {
                    return lightningDamage.GetValue();
                }
            case StatType.maxHealth:
                {
                    return GetMaxHealth();
                }
            case StatType.armor:
                {
                    if (isChilled)
                    {
                        return Mathf.RoundToInt(armor.GetValue() * 0.8f);//Reduce armor 20% when chilled
                    }
                    else
                    {
                        return armor.GetValue();
                    }
                }
            case StatType.evasion:
                {
                    return evasion.GetValue() + agility.GetValue();
                }
            case StatType.magicResistance:
                {
                    return magicResistance.GetValue() + inteligence.GetValue() * 3;
                }
        }
        return 0;
    }
}
