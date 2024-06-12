using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ParrySkill : Skill
{
    [Header("Parry")]
    [SerializeField] UI_SkillTreeSlot parryUnlockButton;
    public bool parryUnlocked;

    [Header("Parry - Restore HP")]
    [SerializeField] UI_SkillTreeSlot parryRestoredUnlockButton;
    public bool parryRestoreUnlocked;

    [Header("Parry - Mirage Attack")]
    [SerializeField] UI_SkillTreeSlot parryMirageAttackUnlockButton;
    public bool parryMirageAttackUnlocked;
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
}
