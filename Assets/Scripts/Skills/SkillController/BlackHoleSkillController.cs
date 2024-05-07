using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotKeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList = new List<KeyCode>();
    private List<KeyCode> keyCodeListCopied;
    public float maxSize;
    public float growSpeed;
    public bool canGrow;

    public int amountOfAttack = 4;
    private bool canAttack = false;
    public float xOffsetClone = 2f;
    public float cloneAttackCooldown = 0.3f;
    private float cloneAttackTimer = 0f;

    private List<Transform> targets = new List<Transform>();

    private void Awake()
    {
        keyCodeListCopied = new List<KeyCode>(keyCodeList.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        CheckCreateClone();

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }
    }

    private void CheckCreateClone()
    {
        cloneAttackTimer -= Time.deltaTime;

        if (targets.Count == 0) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (amountOfAttack > 0)
            {
                canAttack = true;
            }
        }

        if (cloneAttackTimer < 0 && canAttack)
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);
            int valueRandom = Random.Range(0, 100);
            Vector3 offset = new Vector3(valueRandom >= 50 ? xOffsetClone : -xOffsetClone, 0, 0);
            SkillManager.Instance.clone.CreateClone(targets[randomIndex], offset);
            amountOfAttack--;
            if (amountOfAttack < 0)
            {
                canAttack = false;
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
        if (keyCodeListCopied.Count <= 0)
        {
            Debug.LogWarning("Not enough hot key in list defined");
            return;
        }
        GameObject newHotkey = Instantiate(hotKeyPrefab, enemy.transform.position + new Vector3(0, 2), Quaternion.identity);

        KeyCode chosenKey = keyCodeListCopied[Random.Range(0, keyCodeListCopied.Count)];
        keyCodeListCopied.Remove(chosenKey);

        BlackHoleHotkeyController newHotkeyScript = newHotkey.GetComponent<BlackHoleHotkeyController>();
        newHotkeyScript.SetupHotkey(chosenKey, enemy.transform, this);
    }

    public void AddEnemyToTargetList(Transform _enemyTransform) => targets.Add(_enemyTransform);
}
