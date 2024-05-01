using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill info")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] Vector2 launchDir;
    [SerializeField] float swordGravity;

    public void CreatSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.attackCheck.transform.position, player.transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();
        newSwordScript.SetupSword(launchDir, swordGravity);
    }
}
