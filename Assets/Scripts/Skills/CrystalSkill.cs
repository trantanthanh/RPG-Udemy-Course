using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkill : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] private float crystalDuration = 5f;

    [Header("Explode crystal")]
    [SerializeField] private bool canExplode;
    [SerializeField] private float growSpeed = 3.5f;

    [Header("Moving crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi stacking crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amoutOfStacks;
    [SerializeField] float multiStackCooldown;
    private List<GameObject> crystalsLeft = new List<GameObject>();
    private float multiStackTimer;

    private GameObject currentCrystal;

    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        if (CanUseMultiCrystal()) return;

        if (currentCrystal == null)
        {
            currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
            SpawnCrystal(currentCrystal);
        }
        else
        {
            if (canMoveToEnemy) return;
            if (!currentCrystal.GetComponent<CrystalSkillController>().isDestroying)
            {
                Vector2 playerPos = player.transform.position;
                player.transform.position = currentCrystal.transform.position;//teleport to crystal
                currentCrystal.transform.position = playerPos;//move the anim explode to player pos
                currentCrystal.GetComponent<CrystalSkillController>().Explode();
            }
        }
    }

    private void SpawnCrystal(GameObject _crystalSpawn)
    {
        _crystalSpawn.GetComponent<CrystalSkillController>().SetupCrystal(crystalDuration, canExplode, growSpeed, canMoveToEnemy, moveSpeed);
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            if (multiStackTimer < 0)
            {
                //respawnCrystal
                UseSkillMultiStacks();
                return true;
            }
            return false;

        }
        return false;
    }

    private void UseSkillMultiStacks()
    {
        if (crystalsLeft.Count <= 0)
        {
            RefillCrystal();//1st times
        }

        GameObject crystalToSpawn = crystalsLeft[crystalsLeft.Count - 1];
        GameObject newCrystall = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
        crystalsLeft.Remove(crystalToSpawn);
        SpawnCrystal(newCrystall);

        if (crystalsLeft.Count <= 0)
        {
            multiStackTimer = multiStackCooldown;
            RefillCrystal();
        }
    }

    private void RefillCrystal()
    {
        for (int i = 0; i < amoutOfStacks; i++)
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }

    protected override void Update()
    {
        base.Update();
        multiStackTimer -= Time.deltaTime;
    }
}
