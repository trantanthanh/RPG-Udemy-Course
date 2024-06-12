using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Freeze Enemies Effect", menuName = "Data/Item Effect/Freeze Enemy")]
public class FreezeEnemies_Effect_SO : ItemEffect_SO
{
    [SerializeField] float duration;

    public override void ExecuteEffect(Transform _target)
    {
        if (!InventoryManager.Instance.CanUseArmorEffect()) return;
        
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_target.position, 2);
        foreach (Collider2D collider in colliders)
        {
            Enemy enemy = collider.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.FreezeTimerFor(duration);
            }
        }
    }
}
