using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public bool isChilled;//reduce armor by 20%
    public bool isShocked;//reduce accuracy by 20%

    private float igniteTimer;
    private float igniteDuration = 4f;
    private float igniteDamageCoolDown = 0.3f;//interval take burn damage
    private float igniteDamageTimer;
    private int igniteDamage;

    private float chillTimer;
    private float chillDuration = 2f;

    private float shockTimer;
    private float shockDuration = 2f;


    [Header("Offensive stats")]
    public Stat damage;
    public Stat critChance;
    public Stat critPower;//default value is 150%

    [Header("Defensive stats")]
    public Stat maxHealth;
    public Stat evasion;
    public Stat armor;
    public Stat magicResistance;

    private EntityFx fx;

    protected int currentHealth;
    public int CurrentHealth { get => currentHealth;}
    public System.Action onHealthChanged;

    private bool isAlive = true;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHealth = maxHealth.GetValue();
        fx = GetComponent<EntityFx>();
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

    public virtual void DoDamage(CharacterStats _targetStats)
    {

        if (CheckTargetCanAvoidAttack(_targetStats)) return;

        int totalDamage = damage.GetValue() + strength.GetValue();
        if (CanCrit())
        {
            totalDamage = CalculateCritDamage(totalDamage);
        }
        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);//Apply damage to target

        DoMagicDamage(_targetStats);
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightningDamage = lightningDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightningDamage + inteligence.GetValue();
        totalMagicDamage = CheckTargetMagicResist(_targetStats, totalMagicDamage);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightningDamage) <= 0)
        {
            return;
        }

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

            if (Random.value < 0.5f && _lightningDamage > 0)
            {
                _canApplyShock = true;
                break;
            }
        }

        if (_canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }

        _targetStats.ApplyAilment(_canApplyIgnite, _canApplyChill, _canApplyShock);
    }

    private int CheckTargetMagicResist(CharacterStats targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= targetStats.magicResistance.GetValue() + targetStats.inteligence.GetValue() * 3;
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }

    public void ApplyAilment(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
        {
            return;//Don't stack Ailment
        }

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;

        if (isIgnited)
        {
            igniteTimer = igniteDuration;
            fx.IgniteFxFor(igniteDuration, igniteDamageCoolDown);
        }
        else if (isChilled)
        {
            chillTimer = chillDuration;
            fx.ChillFxFor(chillDuration);
        }
        else if (isShocked)
        {
            shockTimer = shockDuration;
            fx.ShockFxFor(shockDuration);
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
        int totalTargetEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        if (isShocked)
        {
            totalTargetEvasion += 20;
        }
        if (Random.Range(0, 100) <= totalTargetEvasion)
        {
            return true;
        }
        return false;
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCritChance)
        {
            return true;
        }
        return false;
    }

    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;
        float totalDamage = _damage * totalCritPower;
        return Mathf.RoundToInt(totalDamage);
    }

    public bool IsAlive()
    {
        return currentHealth > 0;
    }

    protected virtual void TakeDamage(int damage)
    {
        TakeDamageWithoutEffect(damage);
    }

    private void TakeDamageWithoutEffect(int damage)
    {
        if (!isAlive) return;//already call die()
        onHealthChanged?.Invoke();
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public int GetMaxHealth() => (maxHealth.GetValue() + vitality.GetValue() * 5);

    protected virtual void Die()
    {
        isAlive = false;
    }
}
