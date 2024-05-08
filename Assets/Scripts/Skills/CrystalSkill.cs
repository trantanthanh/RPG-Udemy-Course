using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] float crystalDuration = 5f;
    private GameObject currentCrystal;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            currentCrystal.GetComponent<CrystalSkillController>().SetupCrystal(crystalDuration);
        }
        else
        {
            if (!currentCrystal.GetComponent<CrystalSkillController>().isDestroying)
            {
                player.transform.position = currentCrystal.transform.position;//teleport to crystal
                currentCrystal.GetComponent<CrystalSkillController>().Explode();
            }
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
