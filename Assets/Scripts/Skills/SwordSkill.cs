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
    [SerializeField] int amountOfBounce = 4;
    [SerializeField] float bouceGravity;

    [Header("Skill info")]
    [SerializeField] GameObject swordPrefab;
    [SerializeField] float launchForce;
    [SerializeField] float swordGravityRegular;
    private float swordGravity;

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
            newSwordScript.SetUpBouce(true, amountOfBounce);
        }
        newSwordScript.SetupSword(finalForce, swordGravity, player);

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
            if (swordType == SwordType.Regular)
            {
                swordGravity = swordGravityRegular;
            }
            else if (swordType == SwordType.Bounce)
            {
                swordGravity = bouceGravity;
            }
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
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
