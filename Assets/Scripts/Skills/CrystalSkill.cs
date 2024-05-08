using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] GameObject crystalPrefab;
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
        }
        else
        {
            player.transform.position = currentCrystal.transform.position;//teleport to crystal
            currentCrystal.GetComponent<CrystalSkillController>().Explode();
            Destroy(currentCrystal, 2f);
        }
    }

    protected override void Update()
    {
        base.Update();
    }
}
