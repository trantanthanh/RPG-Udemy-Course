using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashSkill : Skill
{
    [Header("Dash")]
    [SerializeField] private UI_SkillTreeSlot dashUnlockButton;
    public bool dashUnlocked { get; private set; }

    [Header("Clone on Dash")]
    [SerializeField] private UI_SkillTreeSlot cloneOnDashUnlockButton;
    public bool cloneOnDashUnlocked { get; private set; }

    [Header("Clone on arrival")]
    [SerializeField] private UI_SkillTreeSlot cloneOnArrivalButton;
    public bool cloneOnArrivalUnlocked { get; private set; }

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

    public void CreateCloneOnDashStart()
    {
        if (cloneOnDashUnlocked)
        {
            SkillManager.Instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CreateCloneOnDashOver()
    {
        if (cloneOnArrivalUnlocked)
        {
            SkillManager.Instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
