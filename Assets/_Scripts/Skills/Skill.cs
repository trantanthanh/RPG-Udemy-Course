using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    protected float cooldownTimer = 0f;

    protected Player player;

    private void Awake()
    {
        player = PlayerManager.Instance.player;
    }

    protected virtual void Start()
    {
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
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
