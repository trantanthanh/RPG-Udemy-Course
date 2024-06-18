using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeSkill : Skill
{
    [Header("Dodge")]
    [SerializeField] private UI_SkillTreeSlot dodgeUnlockButton;
    [SerializeField] private int evasionBonus = 10;
    public bool dodgeUnlocked { get; private set; }

    [Header("Dodge Mirage")]
    [SerializeField] private UI_SkillTreeSlot dodgeMirageUnlockButton;
    public bool dodgeMirageUnlocked { get; private set; }

    private void OnEnable()
    {
        dodgeUnlockButton.onUpgradeSkill = UnlockDodge;
        dodgeMirageUnlockButton.onUpgradeSkill = () => dodgeMirageUnlocked = true;
    }

    private void UnlockDodge()
    {
        if (!dodgeUnlocked)
        {
            //player.stats.evasion.AddModifier(evasionBonus);
            StartCoroutine(AddEvasionModifierWhenReady());
        }
        dodgeUnlocked = true;
    }

    private IEnumerator AddEvasionModifierWhenReady()
    {
        // wait until player.stats is ready
        while (player.stats == null)
        {
            yield return null; // Wait to next frame
        }

        player.stats.evasion.AddModifier(evasionBonus);
    }

    public void CreateMirageOnDodge()
    {
        if (dodgeMirageUnlocked)
        {
            Transform closestEnemy = player.FindClosestEnemy(transform.position, 25);
            Vector3 offset = Vector3.zero;
            if (closestEnemy != null)
            {
                offset.y = player.transform.position.y - closestEnemy.transform.position.y;
            }
            SkillManager.Instance.clone.CreateClone(closestEnemy == null ? player.transform : closestEnemy, offset);
        }
    }
}
