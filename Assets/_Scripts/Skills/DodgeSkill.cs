using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot dodgeUnlockButton;
    [SerializeField] private int evasionBonus = 10;
    public bool dodgeUnlocked {  get; private set; }

    [Header("Dodge Mirage")]
    [SerializeField] private UI_SkillTreeSlot dodgeMirageUnlockButton;
    public bool dodgeMirageUnlocked { get; private set; }

    private void OnEnable()
    {
        dodgeUnlockButton.onUpgradeSkill = UnlockDodge;
        dodgeMirageUnlockButton.onUpgradeSkill = UnlockDodgeMirage;
    }

    private void UnlockDodge()
    {
        if (dodgeUnlockButton.unlocked)
        {
            if (!dodgeUnlocked)
            {
                player.stats.evasion.AddModifier(evasionBonus);
            }
            dodgeUnlocked = true;
        }
    }

    private void UnlockDodgeMirage()
    {
        if (dodgeMirageUnlockButton.unlocked)
        {
            dodgeMirageUnlocked = true;
        }
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
        {
            SkillManager.Instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
