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
        enemy.DoDamagePlayerInCircle(enemy.attackCheck.position, enemy.attackCheckRadius);
    }

    private void OpenCounterAttack() => enemy.OpenCounterAttack();
    private void CloseCounterAttack() => enemy.CloseCounterAttack();
}
