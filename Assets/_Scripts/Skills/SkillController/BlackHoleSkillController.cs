using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour
{
    [Header("Hotkey info")]
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList = new List<KeyCode>();
    private List<KeyCode> keyCodeListCopied;
    private List<GameObject> createdHotkeys = new List<GameObject>();

    private List<Transform> targets = new List<Transform>();

    private float maxSize;
    private float blackHoleTimer;
    private float growSpeed;
    private bool canGrow;

    private float shrinkSpeed;
    private bool canShrink = false;

    private int amountOfAttack = 4;
    private float xOffsetClone = 2f;
    private float cloneAttackCooldown = 0.3f;

    private float cloneAttackTimer = 0f;
    private bool cloneAttackRelease = false;
    private bool canCreateHotkey = true;

    private Player player;
    public bool playerCanExitState { get; private set; } = false;

    public void SetupBlackHole(Player _player, float _blackHoleDuration, float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttack, float _cloneAttackCooldown, float _xOffsetClone)
    {
        playerCanExitState = false;
        this.player = _player;
        this.canGrow = true;
        this.maxSize = _maxSize;
        this.blackHoleTimer = _blackHoleDuration;
        this.growSpeed = _growSpeed;
        this.shrinkSpeed = _shrinkSpeed;
        this.amountOfAttack = _amountOfAttack;
        this.cloneAttackCooldown = _cloneAttackCooldown;
        this.xOffsetClone = _xOffsetClone;
    }

    private void Awake()
    {
        keyCodeListCopied = new List<KeyCode>(keyCodeList.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBlackHoleDuration();

        CheckCreateClone();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    private void UpdateBlackHoleDuration()
    {
        blackHoleTimer -= Time.deltaTime;

        if ((blackHoleTimer < 0 || Input.GetKeyDown(KeyCode.R)) && !cloneAttackRelease)
        {
            ReleaseCloneAttack();
        }
    }

    private void CheckCreateClone()
    {
        cloneAttackTimer -= Time.deltaTime;


        if (cloneAttackTimer < 0 && cloneAttackRelease)
        {
            cloneAttackTimer = cloneAttackCooldown;
            if (amountOfAttack > 0)
            {
                CreateCloneAttack();
            }
        }
    }

    private void CreateCloneAttack()
    {
        int randomIndex = Random.Range(0, targets.Count);
        int valueRandom = Random.Range(0, 100);
        Vector3 offset = new Vector3(valueRandom >= 50 ? xOffsetClone : -xOffsetClone, 0, 0);
        if (SkillManager.Instance.clone.CrystalInsteadOfClone)
        {
            SkillManager.Instance.crystal.CreateCrystal();
            SkillManager.Instance.crystal.SetClosestEnemy(targets[randomIndex]);
        }
        else
        {
            SkillManager.Instance.clone.CreateClone(targets[randomIndex], offset, targets[randomIndex]);
        }
        amountOfAttack--;
        if (amountOfAttack <= 0)
        {
            Invoke("BlackHoleDone", 1f);
        }
    }

    private void BlackHoleDone()
    {
        playerCanExitState = true;
        player.fx.MakeTransparent(false);
        canShrink = true;
    }

    private void ReleaseCloneAttack()
    {
        if (!SkillManager.Instance.clone.CrystalInsteadOfClone)
        {
            player.fx.MakeTransparent(true);
        }

        canCreateHotkey = false;
        if (targets.Count > 0)
        {
            cloneAttackRelease = true;
        }
        else
        {
            //no target to attack
            BlackHoleDone();
        }
        DestroyHotkeys();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.FreezeTimer(true);
            CreateHotkey(enemy);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.FreezeTimer(false);
        }
    }

    private void CreateHotkey(Enemy enemy)
    {
        if (!canCreateHotkey) return;
        if (keyCodeListCopied.Count <= 0)
        {
            Debug.LogWarning("Not enough hot key in list defined");
            return;
        }
        GameObject newHotkey = Instantiate(hotKeyPrefab, enemy.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotkeys.Add(newHotkey);

        KeyCode chosenKey = keyCodeListCopied[Random.Range(0, keyCodeListCopied.Count)];
        keyCodeListCopied.Remove(chosenKey);

        BlackHoleHotkeyController newHotkeyScript = newHotkey.GetComponent<BlackHoleHotkeyController>();
        newHotkeyScript.SetupHotkey(chosenKey, enemy.transform, this);
    }

    private void DestroyHotkeys()
    {
        if (createdHotkeys.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < createdHotkeys.Count; i++)
        {
            Destroy(createdHotkeys[i]);
        }
    }

    public void AddEnemyToTargetList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
