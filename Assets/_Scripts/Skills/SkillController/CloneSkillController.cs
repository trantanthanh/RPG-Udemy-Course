using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    [SerializeField] Vector3 offsetPos;//align for anchor in sprite
    [SerializeField] float cloneColorLosingSpeed;
    private float cloneTimer;

    [SerializeField] Transform attackCheck;
    [SerializeField] float attackRadius;
    private Transform closestEnemy;
    private bool canDuplicateClone;
    private int percentToDuplicateClone;

    public Transform ClosestEnemy { get { return closestEnemy; } set { closestEnemy = value; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, Transform _targetToFacing, bool _canDuplicateClone, int _percentToDuplicateClone)
    {
        cloneTimer = _cloneDuration;
        this.canDuplicateClone = _canDuplicateClone;
        this.percentToDuplicateClone = _percentToDuplicateClone;
        transform.position = _newTransform.position + offsetPos + _offset;
        if (_canAttack)
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 4));//Random from 1 to 3
        }
        if (_targetToFacing != null)
        {
            SetFaceToEnemy(_targetToFacing);
        }
        else
        {
            FaceClosestEnemy();
        }
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;
        if (cloneTimer < 0)
        {
            spriteRenderer.color = new Color(1, 1, 1, spriteRenderer.color.a - Time.deltaTime * cloneColorLosingSpeed);

            if (spriteRenderer.color.a < 0)
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
        Transform hasEnemy = null;
        hasEnemy = PlayerManager.Instance.player.DoDamageEnemiesInCircle(attackCheck.position + offsetPos, attackRadius, false, 0.0f, 1 - PlayerManager.Instance.player.skills.clone.GetPercentMirageDamage());
        if (hasEnemy != null && canDuplicateClone)
        {
            int randomValue = Random.Range(0, 100);
            if (randomValue < percentToDuplicateClone)
            {
                SkillManager.Instance.clone.CreateCloneCanDuplicate(hasEnemy);
            }
        }
    }

    private void FaceClosestEnemy()
    {
        closestEnemy = PlayerManager.Instance.player.FindClosestEnemy(transform.position, 25);
        CheckFlipToFacing();
    }

    private void CheckFlipToFacing()
    {
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.transform.position.x)
            {
                transform.Rotate(0, 180, 0);
            }
        }
    }

    private void SetFaceToEnemy(Transform _transform)
    {
        closestEnemy = _transform;
        CheckFlipToFacing();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
}
