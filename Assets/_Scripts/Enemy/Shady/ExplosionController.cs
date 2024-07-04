using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionController : MonoBehaviour
{
    Enemy enemy;
    float growSpeed;
    float growMaxSize;

    Animator animator;

    bool canGrow = true;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(growMaxSize, growMaxSize), growSpeed * Time.deltaTime);
        }

        if (growMaxSize - transform.localScale.x < 0.5f)
        {
            canGrow = false;
            animator.SetBool("Explode", true);
        }
    }

    public void SetupExplosion(Enemy _enemy, float _growSpeed, float _growMaxSize)
    {
        this.enemy = _enemy;
        this.growSpeed = _growSpeed;
        this.growMaxSize = _growMaxSize;
    }

    private void ExplodeEventTrigger()
    {
        enemy.DoDamagePlayerInCircle(transform.position, enemy.attackCheckRadius);
    }

    private void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
