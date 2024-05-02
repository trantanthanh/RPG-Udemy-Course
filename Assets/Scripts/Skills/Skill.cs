using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    protected float cooldownTimer = 0f;

    protected Player player;

    private void Awake()
    {
        player = PlayerManager.Instance.player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkil()
    {
        if (cooldownTimer < 0)
        {
            return true;
        }
        return false;
    }

    public virtual void UseSkill()
    {
        cooldownTimer = cooldown;
    }
}
