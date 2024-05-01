using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator animator;
    [SerializeField] Vector3 offsetPos;
    [SerializeField] float cloneColorLosingSpeed;
    private float cloneTimer;

    [SerializeField] Transform attackCheck;
    [SerializeField] float attackRadius;
    private Transform closestEnemy;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack)
    {
        cloneTimer = _cloneDuration;
        transform.position = _newTransform.position + offsetPos;
        if (_canAttack )
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 4));//Random from 1 to 3
        }
        FaceClosestEnemy();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime * cloneColorLosingSpeed);

            if (sr.color.a < 0)
            {
                Destroy(gameObject);
            }

        }
    }

    private void AnimationDoneTrigger()
    {
        cloneTimer = -0.1f;//Make immediately to fade out
    }


    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position + offsetPos, attackRadius);

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Damage();
            }
        }
    }

    private void FaceClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (closestDistance > distance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.transform;
                }

            }
        }

        if (closestEnemy != null)
        {
            if (transform.position.x >  closestEnemy.transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }
}
