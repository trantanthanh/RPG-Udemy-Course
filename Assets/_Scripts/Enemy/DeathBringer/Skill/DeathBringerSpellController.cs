using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DeathBringerSpellController : MonoBehaviour
{
    [SerializeField] Transform check;
    [SerializeField] Vector2 boxSize;

    private Enemy enemy;

    public void SetupSpell(Enemy _enemy, float _time)
    {
        this.enemy = _enemy;
        Destroy(gameObject, _time);
    }

    private void AnimationTrigger()
    {
        enemy.DoDamagePlayerInBox(check.position, boxSize);
    }

    //private void SelfDestroy()
    //{
    //    Destroy(gameObject);
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(check.position, boxSize);
    }
}
