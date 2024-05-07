using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour
{
    [Header("Hotkey info")]
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList = new List<KeyCode>();
    private List<KeyCode> keyCodeListCopied;

    [Header("Black hole info")]
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    public bool canGrow;
    [Space]
    [SerializeField] private float shrinkSpeed;
    private bool canShrink = false;

    [Header("Clone attack info")]
    [SerializeField] private int amountOfAttack = 4;
    [SerializeField] private float xOffsetClone = 2f;
    [SerializeField] private float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer = 0f;
    private bool cloneAttackRelease = false;
    private bool canCreateHotkey = true;
    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkeys = new List<GameObject>();

    private void Awake()
    {
        keyCodeListCopied = new List<KeyCode>(keyCodeList.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
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

    private void CheckCreateClone()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R) && !canShrink)
        {
            canCreateHotkey = false;
            if (targets.Count > 0)
            {
                cloneAttackRelease = true;
            }
            else
            {
                //no target to attack
                cloneAttackRelease = false;
                canShrink = true;
            }
            DestroyHotkeys();
        }

        if (cloneAttackTimer < 0 && cloneAttackRelease)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);
            int valueRandom = Random.Range(0, 100);
            Vector3 offset = new Vector3(valueRandom >= 50 ? xOffsetClone : -xOffsetClone, 0, 0);
            SkillManager.Instance.clone.CreateClone(targets[randomIndex], offset);
            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                cloneAttackRelease = false;
                canShrink = true;
            }
        }
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
