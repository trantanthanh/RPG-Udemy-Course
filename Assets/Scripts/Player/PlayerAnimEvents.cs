using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
    Player player => GetComponentInParent<Player>();
    public void AnimationDoneTrigger() => player.AnimationDoneTrigger();
}
