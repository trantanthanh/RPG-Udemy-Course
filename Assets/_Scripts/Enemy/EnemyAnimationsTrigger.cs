using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationsTrigger : MonoBehaviour
{
    Enemy enemy => GetComponentInParent<Enemy>();
    EnemyArcher archer => GetComponentInParent<EnemyArcher>();
    EnemyShady shady => GetComponentInParent<EnemyShady>();

    IAnimationDoneTrigger enemyAnimTrigger => GetComponentInParent<IAnimationDoneTrigger>();
    private void AnimationDoneTrigger() => enemyAnimTrigger.AnimationDoneTrigger();

    private void AttackTrigger()
    {
        enemy.DoDamagePlayerInCircle(enemy.attackCheck.position, enemy.attackCheckRadius);
    }

    private void OpenCounterAttack() => enemy.OpenCounterAttack();
    private void CloseCounterAttack() => enemy.CloseCounterAttack();

    #region enemy archer
    private void SpawnArrow()
    {
        if (archer != null)
        {
            archer.SpawnArrow();
        }
    }
    #endregion

    #region enemy shady
    private void ShadySpecificAttack()
    {
        if (shady != null)
        {
            shady.SpecificAttackTrigger();
        }
    }

    private void ShadySelfDestroy()
    {
        Destroy(shady.gameObject);
    }
    #endregion
}
