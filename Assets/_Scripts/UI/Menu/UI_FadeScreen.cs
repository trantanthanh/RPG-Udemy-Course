using UnityEngine;

public class UI_FadeScreen : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void FadeIn()
    {
        animator.Play("FadeIn", -1, 0f);
    }

    public void FadeOut()
    {
        animator.Play("FadeOut", -1, 0f);
    }
}
