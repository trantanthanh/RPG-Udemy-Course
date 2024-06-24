using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D circleCollider;

    private float crystalExistTimer;
    private bool canExplode;
    private bool canGrow = false;
    private float growSpeed;
    private bool canMoveToEnemy;
    private float moveSpeed;
    private Transform closestTarget;
    public bool isDestroying { get; private set; } = false;
    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    public void Explode()
    {
        isDestroying = true;
        canGrow = true;
        animator.SetBool("Explode", true);
        //Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void SetupCrystal(float _crystalDuration, bool _canExplode, float _growSpeed, bool _canMoveToEnemy, float _moveSpeed)
    {
        canGrow = false;
        this.canExplode = _canExplode;
        this.growSpeed = _growSpeed;
        this.canMoveToEnemy = _canMoveToEnemy;
        this.moveSpeed = _moveSpeed;
        this.crystalExistTimer = _crystalDuration;
        isDestroying = false;
        
        closestTarget = PlayerManager.Instance.player.FindClosestEnemy(transform.position, 25);
    }

    public void SetClosestEnemy(Transform _enemy)
    {
        closestTarget = _enemy;
    }

    private void Update()
    {
        crystalExistTimer -= Time.deltaTime;
        if (crystalExistTimer < 0 && !isDestroying)
        {
            if (canExplode)
            {
                Explode();
            }
            else
            {
                SelfDestroy();
            }

        }

        if (canMoveToEnemy && closestTarget != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, closestTarget.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestTarget.position) < 1)
            {
                canMoveToEnemy = false;
                Explode();
            }

        }

        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3, 3), growSpeed * Time.deltaTime);
        }
    }

    private void AnimationExplodeEvent()
    {
        PlayerManager.Instance.player.DoDamageEnemiesInCircle(transform.position, circleCollider.radius, true, 1f, transform);//Do magical damage
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (canExplode && !isDestroying)
    //    {
    //        if (collision.GetComponent<Enemy>() != null)
    //        {
    //            Explode();
    //        }
    //    }
    //}

    private void SelfDestroy() => Destroy(gameObject);
}
