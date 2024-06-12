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

    [Header("Bouce info")]
    [SerializeField] int bounceAmount = 4;
    [SerializeField] float bounceSpeed = 20f;
    [SerializeField] float bounceRange = 10f;
    [SerializeField] float bounceGravity;

    [Header("Pierce info")]
    [SerializeField] int pierceAmount;
    [SerializeField] float pierceGravity;

    [Header("Skill info")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] float launchForce;
    [SerializeField] float swordGravityRegular;
    [SerializeField] float freeTimeDuration = 0.7f;
    [SerializeField] float returnSpeed = 12f;
    private float swordGravity;

    [Header("Spin info")]
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
    }
    public void CreatSword()
    {
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
