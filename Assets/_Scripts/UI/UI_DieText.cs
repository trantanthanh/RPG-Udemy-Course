using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_DieText : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Show();
    }

    private void Show()
    {
        animator.Play("Show", -1, 0f);
    }
}
