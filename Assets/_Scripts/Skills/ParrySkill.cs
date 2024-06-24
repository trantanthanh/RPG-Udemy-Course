using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked { get; private set; }

    [Header("Parry - Restore HP")]
    [SerializeField] UI_SkillTreeSlot parryRestoredUnlockButton;
    [Range(0.1f, 1f)]
    [SerializeField] float amountHPRestore;
    public bool parryRestoreUnlocked { get; private set; }

    [Header("Parry - Mirage Attack")]
    [SerializeField] UI_SkillTreeSlot parryMirageAttackUnlockButton;
    public bool parryMirageAttackUnlocked { get; private set; }

    private void OnEnable()
    {
        parryUnlockButton.onUpgradeSkill = () => parryUnlocked = true; ;
        parryRestoredUnlockButton.onUpgradeSkill = () => parryRestoreUnlocked = true; ;
        parryMirageAttackUnlockButton.onUpgradeSkill = () => parryMirageAttackUnlocked = true; ;
    }

    protected override void CheckUnlock()
    {
        base.CheckUnlock();

        parryUnlocked = parryUnlockButton.unlocked;
        parryRestoreUnlocked = parryRestoredUnlockButton.unlocked;
        parryMirageAttackUnlocked = parryMirageAttackUnlockButton.unlocked;
    }

    public void MakeMirageOnParry(Transform _enemyTarget)
    {
        ParrySuccess();
        if (parryMirageAttackUnlocked)
        {
            SkillManager.Instance.clone.CreateCloneOnCounterAttack(_enemyTarget);
        }
    }

    private void ParrySuccess()
    {
        if (parryRestoreUnlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealth() * amountHPRestore);
            //Debug.Log("restoreAmount " + restoreAmount);
            player.stats.RestoreHealthBy(restoreAmount);
        }
    }
}
