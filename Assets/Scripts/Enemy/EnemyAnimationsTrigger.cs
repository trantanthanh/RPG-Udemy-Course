using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationsTrigger : MonoBehaviour
{
    IAnimationDoneTrigger enemy => GetComponentInParent<IAnimationDoneTrigger>();
    public void AnimationDoneTrigger() => enemy.DoneTriggerAnim();
}
