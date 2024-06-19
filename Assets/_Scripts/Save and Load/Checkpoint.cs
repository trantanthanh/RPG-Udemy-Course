using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator animator;
    private bool isLighter = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLighter)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                isLighter = true;
                animator.SetBool("active", true);
            }
        }
    }
}
