using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skill
{
    [SerializeField] GameObject blackHolePrefab;
    [Header("Black hole info")]
    [SerializeField] private float blackHoleDuration = 2f;
    [SerializeField] private float maxSize = 15f;
    [SerializeField] private float growSpeed = 1f;
    [Space]
    [SerializeField] private float shrinkSpeed = 3f;

    [Header("Clone attack info")]
    [SerializeField] private int amountOfAttack = 4;
    [SerializeField] private float xOffsetClone = 1.5f;
    [SerializeField] private float cloneAttackCooldown = 0.3f;

    BlackHoleSkillController currentBlackHole;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    private void CreateBlackHole()
    {
        GameObject newBlackHole = Instantiate(blackHolePrefab, player.transform.position, Quaternion.identity);
        currentBlackHole = newBlackHole.GetComponent<BlackHoleSkillController>();
        currentBlackHole.SetupBlackHole(player, blackHoleDuration, maxSize, growSpeed, shrinkSpeed, amountOfAttack, cloneAttackCooldown, xOffsetClone);
    }

    public override void UseSkill()
    {
        base.UseSkill();
        CreateBlackHole();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool IsSkillCompleted()
    {
        if (currentBlackHole == null) return false;

        if (currentBlackHole.playerCanExitState)
        {
            currentBlackHole = null;
            return true;
        }

        return false;
    }
}
