using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeController : MonoBehaviour
{
    [SerializeField] CharacterStats targetStats;
    [SerializeField] float speed;

    private Animator animator;
    private bool isDone = false;

    // Start is called before the first frame update
    void Start()
    {
        isDone = false;
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDone || animator == null)
        {
            return;
        }
        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);
        transform.right = (targetStats.transform.position - transform.position).normalized;
        if (Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {
            transform.localRotation = Quaternion.identity;
            animator.transform.localRotation = Quaternion.identity;
            animator.transform.localPosition = new Vector3(0f, 0.5f);
            transform.localScale = new Vector3(3f, 3f);

            isDone = true;
            Invoke(nameof(DamageAndSelfDestroy), 0.2f);
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.TakeDamage(1);
        animator.SetTrigger("Hit");
        Destroy(gameObject, 0.4f);
    }
}
