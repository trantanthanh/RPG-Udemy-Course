using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;

    [Header("Bounce info")]
    [SerializeField] UI_SkillTreeSlot bouncySwordUnlockButton;
    public bool bouncySwordUnlocked { get; private set; }
    [SerializeField] int bounceAmount = 4;
    [SerializeField] float bounceSpeed = 20f;
    [SerializeField] float bounceRange = 10f;
    [SerializeField] float bounceGravity;

    [Header("Pierce info")]
    [SerializeField] UI_SkillTreeSlot bulletSwordUnlockButton;
    public bool bulletSwordUnlocked { get; private set; }
    [SerializeField] int pierceAmount;
    [SerializeField] float pierceGravity;

    [Header("Sword Skill info")]
    [SerializeField] UI_SkillTreeSlot swordThrowUnlockButton;
    public bool throwSwordUnlocked { get; private set; }
    [SerializeField] GameObject swordPrefab;
    [SerializeField] float launchForce;
    [SerializeField] float swordGravityRegular;
    [SerializeField] float freeTimeDuration = 0.7f;
    [SerializeField] float returnSpeed = 12f;
    private float swordGravity;

    [Header("Passive sword")]
    [SerializeField] UI_SkillTreeSlot timeStopUnlockButton;
    public bool timeStopUnlocked { get; private set; }
    [SerializeField] UI_SkillTreeSlot vulnerabilityUnlockButton;
    [Range(0.1f, 1f)]
    [SerializeField] float percentAmplifierDamage;
    public float PercentAmplifierDamage { get => percentAmplifierDamage; }
    public bool vulnerabilityUnlocked { get; private set; }

    [Header("Spin info")]
    [SerializeField] UI_SkillTreeSlot chainsawSwordUnlockButton;
    public bool chainsawSwordUnlocked { get; private set; }
    [SerializeField] float maxTravelDistance = 7f;
    [SerializeField] float spinDuration = 2f;
    [SerializeField] float spinHitCooldown = 0.35f;
    [SerializeField] float spinGravity = 1f;

    [Header("Aim dot")]
    [SerializeField] int numOfDots;
    [SerializeField] float spaceBetweenDots;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] Transform dotParent;
    private GameObject[] dots;

    private Vector2 finalForce;
    protected override void Start()
    {
        base.Start();
        GenerateDots();
        CallBackUnlock();
    }

    private void CallBackUnlock()
    {
        swordThrowUnlockButton.onUpgradeSkill = () => throwSwordUnlocked = true;
        timeStopUnlockButton.onUpgradeSkill = () => timeStopUnlocked = true;
        vulnerabilityUnlockButton.onUpgradeSkill = () => vulnerabilityUnlocked = true;
        bulletSwordUnlockButton.onUpgradeSkill = () => bulletSwordUnlocked = true;
        chainsawSwordUnlockButton.onUpgradeSkill = () => chainsawSwordUnlocked = true;
        bouncySwordUnlockButton.onUpgradeSkill = () => bouncySwordUnlocked = true;
    }

    private void ChooseSwordType()
    {
        swordType = SwordType.Regular;
        if (bulletSwordUnlocked)
        {
            swordType = SwordType.Pierce;
        }
        else if (chainsawSwordUnlocked)
        {
            swordType = SwordType.Spin;
        }
        else if (bouncySwordUnlocked)
        {
            swordType = SwordType.Bounce;
        }
    }

    public void CreatSword()
    {
        ChooseSwordType();
        GameObject newSword = Instantiate(swordPrefab, player.attackCheck.transform.position, player.transform.rotation);
        player.AssignNewSword(newSword);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Bounce)
        {
            newSwordScript.SetupBounce(true, bounceAmount, bounceSpeed, bounceRange);
        }
        else if (swordType == SwordType.Pierce)
        {
            newSwordScript.SetupPierce(true, pierceAmount);
        }
        else if (swordType == SwordType.Spin)
        {
            newSwordScript.SetupSpin(true, maxTravelDistance, spinDuration, spinHitCooldown);
        }

        newSwordScript.SetupSword(finalForce, swordGravity, player, freeTimeDuration, returnSpeed);

        DotsActive(false);
    }


    protected override void Update()
    {
        base.Update();
        UpdateAimSword();
    }

    private void UpdateAimSword()
    {
        if (throwSwordUnlocked)
        {
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                finalForce = AimDirection().normalized * launchForce;
            }

            if (Input.GetKey(KeyCode.Mouse1))
            {
                SetupGravity();

                for (int i = 0; i < dots.Length; i++)
                {
                    dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
                }

            }
        }
    }

    private void SetupGravity()
    {
        switch (swordType)
        {
            case SwordType.Regular:
                {
                    swordGravity = swordGravityRegular;
                    break;
                }
            case SwordType.Bounce:
                {
                    swordGravity = bounceGravity;
                    break;
                }
            case SwordType.Pierce:
                {
                    swordGravity = pierceGravity;
                    break;
                }
            case SwordType.Spin:
                {
                    swordGravity = spinGravity;
                    break;
                }
        }
    }

    #region Aim region
    public Vector2 AimDirection()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;
        return direction;
    }

    private void GenerateDots()
    {
        dots = new GameObject[numOfDots];
        for (int i = 0; i < numOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }

    public void DotsActive(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.attackCheck.transform.position + new Vector2(AimDirection().normalized.x, AimDirection().normalized.y) * launchForce * t + 0.5f * (Physics2D.gravity * swordGravity) * (t * t);
        return position;
    }
    #endregion
}
