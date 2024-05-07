using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkill : Skill
{
    [SerializeField] GameObject blackHolePrefab;
    [Header("Black hole info")]
    [SerializeField] private float maxSize = 15f;
    [SerializeField] private float growSpeed = 1f;
    [Space]
    [SerializeField] private float shrinkSpeed = 3f;

    [Header("Clone attack info")]
    [SerializeField] private int amountOfAttack = 4;
    [SerializeField] private float xOffsetClone = 1.5f;
    [SerializeField] private float cloneAttackCooldown = 0.3f;

    public void CreateBlackHole(Transform _newTransform)
    {
        GameObject newBlackHole = Instantiate(blackHolePrefab, _newTransform.position,Quaternion.identity);
        newBlackHole.GetComponent<BlackHoleSkillController>().SetupBlackHole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, cloneAttackCooldown, xOffsetClone);
    }
}
