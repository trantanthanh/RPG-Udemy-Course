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
    protected override void Start()
    {
        base.Start();
        parryUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParry);
        parryRestoredUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryRestore);
        parryMirageAttackUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockParryMirage);
    }

    private void UnlockParry()
    {
        if (parryUnlockButton.unlocked)
        {
            parryUnlocked = true;
        }
    }

    private void UnlockParryRestore()
    {
        if (parryRestoredUnlockButton.unlocked)
        {
            parryRestoreUnlocked = true;
        }
    }

    private void UnlockParryMirage()
    {
        if (parryMirageAttackUnlockButton.unlocked)
        {
            parryMirageAttackUnlocked = true;
        }
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
