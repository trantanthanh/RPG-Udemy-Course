using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    Player player => GetComponentInParent<Player>();
    private void AnimationDoneTrigger() => player.AnimationDoneTrigger();

    private void AttackTrigger()
    {
        player.DoDamageEnemiesInCircle(player.attackCheck.position, player.attackCheckRadius);
    }

    private void ThrowSword()
    {
        player.skills.swordThrow.CreatSword();
    }
}
