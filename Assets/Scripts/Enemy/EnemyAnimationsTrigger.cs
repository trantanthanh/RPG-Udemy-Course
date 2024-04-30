using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationsTrigger : MonoBehaviour
{
    Enemy enemy => GetComponentInParent<Enemy>();
    IAnimationDoneTrigger enemyAnimTrigger => GetComponentInParent<IAnimationDoneTrigger>();
    private void AnimationDoneTrigger() => enemyAnimTrigger.AnimationDoneTrigger();

    private void AttackTrigger()
    {
        Collider2D[] collders = Physics2D.OverlapCircleAll(enemy.attackCheck.position, enemy.attackCheckRadius);
        foreach (Collider2D collider in collders)
        {
            Player player = collider.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }
    }

    private void OpenCounterAttack() => enemy.OpenCounterAttack();
    private void CloseCounterAttack() => enemy.CloseCounterAttack();
}
