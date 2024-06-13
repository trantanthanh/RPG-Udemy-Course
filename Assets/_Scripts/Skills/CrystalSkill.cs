using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalSkill : Skill
{
    [SerializeField] GameObject crystalPrefab;
    [SerializeField] private float crystalDuration = 5f;

    [Header("Crystal")]
    [SerializeField] UI_SkillTreeSlot unlockCrystalButton;
    public bool crystalUnlocked { get; private set; }

    [Header("Crystal Mirage")]
    [SerializeField] UI_SkillTreeSlot unlockCrystalMirageButton;
    public bool crystalMirageUnlocked { get; private set; }//cloneInsteadOfCrystal

    [Header("Crystal Explosion")]
    [SerializeField] UI_SkillTreeSlot unlockCrystalExplosionButton;
    [SerializeField] private float growSpeed = 3.5f;//Speed grow of explosion effect
    public bool canExplode { get; private set; }

    [Header("Moving crystal - Controlled Destruction")]
    [SerializeField] UI_SkillTreeSlot unlockCrystalMovingButton;
    [SerializeField] private float moveSpeed;
    public bool canMoveToEnemy { get; private set; }

    [Header("Multi stacking crystal - Multiple distruction")]
    [SerializeField] UI_SkillTreeSlot unlockMultiCrystalButton;
    [SerializeField] private int amoutOfStacks;
    [SerializeField] float multiStackCooldown;
    [SerializeField] float useTimeWindow = 2.5f;
    public bool canUseMultiStacks { get; private set; }


    private List<GameObject> crystalsLeft = new List<GameObject>();
    private GameObject currentCrystal;

    protected override void Start()
    {
        base.Start();
        unlockCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockCrystal);
        unlockCrystalMirageButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMirage);
        unlockCrystalExplosionButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalExplosion);
        unlockCrystalMovingButton.GetComponent<Button>().onClick.AddListener(UnlockCrystalMoving);
        unlockMultiCrystalButton.GetComponent<Button>().onClick.AddListener(UnlockMultiCrystal);

        //if (canUseMultiStacks)
        //{
        //    ResetAbility();//1st times init crystalsLeft
        //}
    }

    #region unlock zone
    private void UnlockCrystal()
    {
        if (unlockCrystalButton.unlocked)
        {
            crystalUnlocked = true;
        }
    }
    private void UnlockCrystalMirage()
    {
        if (unlockCrystalMirageButton.unlocked)
        {
            crystalMirageUnlocked = true;
        }
    }
    private void UnlockCrystalExplosion()
    {
        if (unlockCrystalExplosionButton.unlocked)
        {
            canExplode = true;
        }
    }
    private void UnlockCrystalMoving()
    {
        if (unlockCrystalMovingButton.unlocked)
        {
            canMoveToEnemy = true;
        }
    }
    private void UnlockMultiCrystal()
    {
        if (unlockMultiCrystalButton.unlocked)
        {
            canUseMultiStacks = true;
            ResetAbility(true);//1st times init crystalsLeft
        }
    }
    #endregion

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
            CreateCrystal();
        }
        else
        {
            if (canMoveToEnemy) return;

            if (!currentCrystal.GetComponent<CrystalSkillController>().isDestroying)
            {
                Vector2 playerPos = player.transform.position;
                player.transform.position = currentCrystal.transform.position;//teleport to crystal
                currentCrystal.transform.position = playerPos;//move the anim explode to player pos
                if (!crystalMirageUnlocked)
                {
                    currentCrystal.GetComponent<CrystalSkillController>().Explode();
                }
            }

            if (crystalMirageUnlocked)
            {
                player.skills.clone.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        SpawnCrystal(currentCrystal);
    }

    public void SetClosestEnemy(Transform _enemy)
    {
        currentCrystal.GetComponent<CrystalSkillController>().SetClosestEnemy(_enemy);
    }

    private void SpawnCrystal(GameObject _crystalSpawn)
    {
        _crystalSpawn.GetComponent<CrystalSkillController>().SetupCrystal(crystalDuration, canExplode, growSpeed, canMoveToEnemy, moveSpeed);
    }

    private bool CanUseMultiCrystal()
    {
        if (canUseMultiStacks)
        {
            //respawnCrystal
            if (crystalsLeft.Count > 0)
            {
                if (crystalsLeft.Count == amoutOfStacks)
                {
                    //if 1st crystal of multi is out but don't use all remain, it'll reset cooldown after useTimeWindow
                    Invoke(nameof(ResetAbility), useTimeWindow);
                }
                UseSkillMultiStacks();
                return true;
            }
        }
        return false;
    }

    private void UseSkillMultiStacks()
    {
        GameObject crystalToSpawn = crystalsLeft[crystalsLeft.Count - 1];
        GameObject newCrystall = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);
        crystalsLeft.Remove(crystalToSpawn);
        SpawnCrystal(newCrystall);

        if (crystalsLeft.Count <= 0)
        {
            cooldownTimer = multiStackCooldown;
            RefillCrystal();
        }
    }

    private void RefillCrystal()
    {
        int amoutToAdd = amoutOfStacks - crystalsLeft.Count;
        for (int i = 0; i < amoutOfStacks; i++)
        {
            crystalsLeft.Add(crystalPrefab);
        }
    }

    protected override void Update()
    {
        base.Update();
    }

    private void ResetAbility(bool isInit = false)
    {
        if (cooldownTimer > 0)
        {
            return;
        }

        RefillCrystal();

        if (!isInit)
        {
            cooldownTimer = multiStackCooldown;
        }
    }

}
