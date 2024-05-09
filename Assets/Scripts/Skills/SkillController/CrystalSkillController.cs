using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator animator;
    private CircleCollider2D circleCollider;

    private float crystalExistTimer;
    private bool canExplode = true;
    private bool canMove;
    private bool moveSpeed;
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
        animator.SetBool("Explode", true);
        //Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }

    public void SetupCrystal(float _crystalDuration)
    {
        this.crystalExistTimer = _crystalDuration;
        isDestroying = false;
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
    }

    private void AnimationExplodeEvent()
    {
        PlayerManager.Instance.player.DoDamageEnemiesInCircle(transform.position, circleCollider.radius);
    }

    private void SelfDestroy() => Destroy(gameObject);
}
