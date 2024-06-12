using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;

    [Header("Clone on Dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;

    [Header("Clone on arrival")]
    public bool cloneOnArrivalUnlocked;
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalButton;

    protected override void Start()
    {
        base.Start();

        dashUnlockButton.GetComponent<Button>().onClick.AddListener(this.UnlockDash);
        cloneOnDashUnlockButton.GetComponent<Button>().onClick.AddListener(this.UnlockCloneOnDash);
        cloneOnArrivalButton.GetComponent<Button>().onClick.AddListener(this.UnlockCloneOnArrival);
    }

    public override void UseSkill()
    {
        base.UseSkill();

        //Create clone trail behind
    }

    private void UnlockDash()
    {
        if (dashUnlockButton.unlocked)
        {
            dashUnlocked = true;
        }
    }

    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }

    private void UnlockCloneOnArrival()
    {
        if (cloneOnArrivalButton.unlocked)
        {
            cloneOnArrivalUnlocked = true;
        }
    }
}
