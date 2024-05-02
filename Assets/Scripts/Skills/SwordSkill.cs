using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkill : Skill
{
    [Header("Skill info")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] float launchForce;
    [SerializeField] float swordGravity;

    private Vector2 finalForce;
    public void CreatSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.attackCheck.transform.position, player.transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();
        newSwordScript.SetupSword(finalForce, swordGravity);
    }

    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            finalForce = AimDirection().normalized * launchForce;
        }

    }
}
